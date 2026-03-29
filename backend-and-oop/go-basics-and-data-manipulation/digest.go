package digest

import (
	"math/cmplx"
	"strings"
	"unsafe"
)

// GetCharByIndex returns the i-th character from the given string.
func GetCharByIndex(str string, idx int) rune {
	if idx < 0 {
		panic("index out of bounds")
	}

	i := 0
	for _, r := range str {
		if i == idx {
			return r
		}
		i++
	}

	panic("index out of bounds")
}

// GetStringBySliceOfIndexes returns a string formed by concatenating specific characters from the input string based
// on the provided indexes.
func GetStringBySliceOfIndexes(str string, indexes []int) string {
	runes := []rune(str)

	var b strings.Builder
	b.Grow(len(indexes))

	for _, idx := range indexes {
		b.WriteRune(runes[idx])
	}

	return b.String()
}

// ShiftPointer shifts the given pointer by the specified number of bytes using unsafe.Add.
func ShiftPointer(pointer **int, shift int) {
	*pointer = (*int)(unsafe.Add(unsafe.Pointer(*pointer), shift))
}

// IsComplexEqual compares two complex numbers and determines if they are equal.
func IsComplexEqual(a, b complex128) bool {
	const eps = 1e-6
	return a == b || cmplx.Abs(a-b) < eps
}

// GetRootsOfQuadraticEquation returns two roots of a quadratic equation ax^2 + bx + c = 0.
func GetRootsOfQuadraticEquation(a, b, c float64) (complex128, complex128) {
	d := cmplx.Sqrt(complex(b*b-4*a*c, 0))

	bb := complex(b, 0)
	twoA := 2 * complex(a, 0)

	x1 := (-bb + d) / twoA
	x2 := (-bb - d) / twoA

	return x1, x2
}

// Sort sorts in-place the given slice of integers in ascending order.
func Sort(source []int) {
	n := len(source)

	for i := n/2 - 1; i >= 0; i-- {
		siftDown(source, i, n)
	}

	for end := n - 1; end > 0; end-- {
		source[0], source[end] = source[end], source[0]
		siftDown(source, 0, end)
	}
}

func siftDown(a []int, i, n int) {
	for {
		left := 2*i + 1

		if left >= n {
			return
		}

		largest := left
		right := left + 1

		if right < n && a[right] > a[left] {
			largest = right
		}

		if a[i] >= a[largest] {
			return
		}

		a[i], a[largest] = a[largest], a[i]
		i = largest
	}
}

// ReverseSliceOne in-place reverses the order of elements in the given slice.
func ReverseSliceOne(s []int) {
	for i, j := 0, len(s)-1; i < j; i, j = i+1, j-1 {
		s[i], s[j] = s[j], s[i]
	}
}

// ReverseSliceTwo returns a new slice of integers with elements in reverse order compared to the input slice.
// The original slice remains unmodified.
func ReverseSliceTwo(s []int) []int {
	if s == nil {
		return []int{}
	}

	result := make([]int, len(s))
	copy(result, s)

	ReverseSliceOne(result)

	return result
}

// SwapPointers swaps the values of two pointers.
func SwapPointers(a, b *int) {
	*a, *b = *b, *a
}

// IsSliceEqual compares two slices of integers and returns true if they contain the same elements in the same order.
func IsSliceEqual(a, b []int) bool {
	if len(a) != len(b) {
		return false
	}

	for i := range a {
		if a[i] != b[i] {
			return false
		}
	}

	return true
}

// DeleteByIndex deletes the element at the specified index from the slice and returns a new slice.
// The original slice remains unmodified.
func DeleteByIndex(s []int, idx int) []int {
	result := make([]int, 0, len(s)-1)

	result = append(result, s[:idx]...)
	result = append(result, s[idx+1:]...)

	return result
}