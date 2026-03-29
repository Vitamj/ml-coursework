# Go LFU Cache

## Overview
This project implements an LFU (Least Frequently Used) cache in Go with support for frequency-based eviction and key lookup.

## Tech stack
Go, cache design, linked lists, generic types, data structures

## Goal
Build an efficient cache that tracks access frequency, updates item priority on reads and writes, and evicts the least frequently used element when capacity is reached.

## What was done
- implemented a generic LFU cache in Go
- supported `Get`, `Put`, `Size`, `Capacity`, and iteration over cache contents
- tracked key access frequency through frequency buckets
- implemented eviction of the least frequently used item
- updated item position when frequency changed
- used linked-list-based bucket organization for efficient reordering
- handled missing keys and capacity constraints

## Result
Built a generic LFU cache with frequency-aware eviction and efficient internal bucket-based organization.
