# Bidirectional Dijkstra

## Overview
This project implements and compares single-source Dijkstra and bidirectional Dijkstra on a weighted directed graph.

## Tech stack
C++, graph algorithms, Dijkstra, bidirectional search, multithreading

## Goal
Find shortest paths more efficiently by running search simultaneously from the source and the target, then compare the result with the standard one-directional approach.

## What was done
- implemented a weighted directed graph with forward and reverse adjacency lists
- implemented standard Dijkstra with a priority queue
- implemented bidirectional Dijkstra with two concurrent search threads
- synchronized shared state with atomic variables and a mutex
- tracked the meeting node of forward and backward search
- compared shortest-path distances between single-threaded and bidirectional versions
- measured execution time across multiple randomized test runs

## Result
Built and benchmarked a bidirectional shortest-path solver, combining graph algorithms with concurrent search and performance comparison.
