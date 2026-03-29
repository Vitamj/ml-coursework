# Max Flow and Bottleneck Analysis

## Overview
This project implements maximum flow computation on a directed graph and identifies bottleneck edges in a service network.

## Tech stack
C++, graph algorithms, BFS, max flow, Edmonds-Karp

## Goal
Model throughput limits in a networked system and determine the maximum possible flow from source to sink using the Edmonds-Karp algorithm.

## What was done
- implemented a directed graph with adjacency lists
- added forward and backward edges for residual capacity tracking
- implemented BFS-based augmenting path search
- implemented the Edmonds-Karp max-flow algorithm
- updated residual flows along augmenting paths
- identified saturated bottleneck edges after flow computation
- tested the solution on simple service-network scenarios

## Result
Built a graph-based throughput analysis tool for identifying system bottlenecks and computing maximum request capacity across service pipelines.
