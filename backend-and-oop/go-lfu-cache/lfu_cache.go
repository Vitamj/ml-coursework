package lfucache

import (
	"errors"
	"iter"

	"github.com/igoroutine-courses/gonature.lfucache/internal/linkedlist"
)

var ErrKeyNotFound = errors.New("key not found")

const DefaultCapacity = 5

type Cache[K comparable, V any] interface {
	Get(key K) (V, error)
	Put(key K, value V)
	All() iter.Seq2[K, V]
	Size() int
	Capacity() int
	GetKeyFrequency(key K) (int, error)
}

type cacheImpl[K comparable, V any] struct {
	capacity int
	size     int

	items   map[K]*entry[K, V]
	buckets linkedlist.List[*freqBucket[K, V]]

	freeEntries []*entry[K, V]
	freeBuckets []*freqBucket[K, V]
}

type entry[K comparable, V any] struct {
	node   linkedlist.Node[*entry[K, V]]
	key    K
	value  V
	bucket *freqBucket[K, V]
}

type freqBucket[K comparable, V any] struct {
	node  linkedlist.Node[*freqBucket[K, V]]
	freq  int
	items linkedlist.List[*entry[K, V]]
}

func New[K comparable, V any](capacity ...int) *cacheImpl[K, V] {
	capacityValue := DefaultCapacity
	if len(capacity) > 0 {
		capacityValue = capacity[0]
	}
	if capacityValue <= 0 {
		panic("capacity must be positive")
	}

	l := &cacheImpl[K, V]{
		capacity:    capacityValue,
		items:       make(map[K]*entry[K, V], capacityValue),
		freeEntries: make([]*entry[K, V], 0, capacityValue),
		freeBuckets: make([]*freqBucket[K, V], 0, capacityValue),
	}

	entries := make([]entry[K, V], capacityValue)
	for i := range entries {
		item := &entries[i]
		item.node.Value = item
		l.freeEntries = append(l.freeEntries, item)
	}

	buckets := make([]freqBucket[K, V], capacityValue)
	for i := range buckets {
		bucket := &buckets[i]
		bucket.node.Value = bucket
		l.freeBuckets = append(l.freeBuckets, bucket)
	}

	return l
}

func (l *cacheImpl[K, V]) Get(key K) (V, error) {
	item, ok := l.items[key]
	if !ok {
		var zero V
		return zero, ErrKeyNotFound
	}

	l.bump(item)

	return item.value, nil
}

func (l *cacheImpl[K, V]) Put(key K, value V) {
	if item, ok := l.items[key]; ok {
		item.value = value
		l.bump(item)
		return
	}

	if l.size == l.capacity {
		l.evict()
	}

	bucket := l.ensureFreqOneBucket()
	item := l.takeEntry()
	item.key = key
	item.value = value
	item.bucket = bucket

	bucket.items.PushFrontNode(&item.node)
	l.items[key] = item
	l.size++
}

func (l *cacheImpl[K, V]) All() iter.Seq2[K, V] {
	return func(yield func(K, V) bool) {
		for bucketNode := l.buckets.Back(); bucketNode != nil; bucketNode = bucketNode.Prev() {
			bucket := bucketNode.Value
			for itemNode := bucket.items.Front(); itemNode != nil; itemNode = itemNode.Next() {
				item := itemNode.Value
				if !yield(item.key, item.value) {
					return
				}
			}
		}
	}
}

func (l *cacheImpl[K, V]) Size() int {
	return l.size
}

func (l *cacheImpl[K, V]) Capacity() int {
	return l.capacity
}

func (l *cacheImpl[K, V]) GetKeyFrequency(key K) (int, error) {
	item, ok := l.items[key]
	if !ok {
		return 0, ErrKeyNotFound
	}

	return item.bucket.freq, nil
}

func (l *cacheImpl[K, V]) bump(item *entry[K, V]) {
	current := item.bucket
	nextNode := current.node.Next()
	newFreq := current.freq + 1

	if nextNode != nil && nextNode.Value.freq == newFreq {
		nextBucket := nextNode.Value
		current.items.Remove(&item.node)
		nextBucket.items.PushFrontNode(&item.node)
		item.bucket = nextBucket

		if current.items.Len() == 0 {
			l.releaseBucket(current)
		}

		return
	}

	if current.items.Len() == 1 {
		current.freq = newFreq
		return
	}

	nextBucket := l.takeBucket(newFreq)
	l.buckets.InsertAfterNode(&current.node, &nextBucket.node)

	current.items.Remove(&item.node)
	nextBucket.items.PushFrontNode(&item.node)
	item.bucket = nextBucket
}

func (l *cacheImpl[K, V]) evict() {
	minBucketNode := l.buckets.Front()
	minBucket := minBucketNode.Value

	itemNode := minBucket.items.Back()
	item := itemNode.Value

	minBucket.items.Remove(itemNode)
	delete(l.items, item.key)
	l.size--

	if minBucket.items.Len() == 0 {
		l.releaseBucket(minBucket)
	}

	l.releaseEntry(item)
}

func (l *cacheImpl[K, V]) ensureFreqOneBucket() *freqBucket[K, V] {
	front := l.buckets.Front()
	if front != nil && front.Value.freq == 1 {
		return front.Value
	}

	bucket := l.takeBucket(1)
	l.buckets.PushFrontNode(&bucket.node)

	return bucket
}

func (l *cacheImpl[K, V]) takeEntry() *entry[K, V] {
	last := len(l.freeEntries) - 1
	item := l.freeEntries[last]
	l.freeEntries = l.freeEntries[:last]

	item.node = linkedlist.Node[*entry[K, V]]{Value: item}
	item.bucket = nil

	var zeroKey K
	var zeroValue V
	item.key = zeroKey
	item.value = zeroValue

	return item
}

func (l *cacheImpl[K, V]) releaseEntry(item *entry[K, V]) {
	item.node = linkedlist.Node[*entry[K, V]]{Value: item}
	item.bucket = nil

	var zeroKey K
	var zeroValue V
	item.key = zeroKey
	item.value = zeroValue

	l.freeEntries = append(l.freeEntries, item)
}

func (l *cacheImpl[K, V]) takeBucket(freq int) *freqBucket[K, V] {
	last := len(l.freeBuckets) - 1
	bucket := l.freeBuckets[last]
	l.freeBuckets = l.freeBuckets[:last]

	bucket.node = linkedlist.Node[*freqBucket[K, V]]{Value: bucket}
	bucket.freq = freq
	bucket.items = linkedlist.List[*entry[K, V]]{}

	return bucket
}

func (l *cacheImpl[K, V]) releaseBucket(bucket *freqBucket[K, V]) {
	l.buckets.Remove(&bucket.node)

	bucket.node = linkedlist.Node[*freqBucket[K, V]]{Value: bucket}
	bucket.freq = 0
	bucket.items = linkedlist.List[*entry[K, V]]{}

	l.freeBuckets = append(l.freeBuckets, bucket)
}