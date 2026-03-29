# Disjoint Set Clan Management

## Tech stack
C++, data structures, disjoint set union, algorithms

## Overview
This project implements a clan management system based on the disjoint set union (union-find) data structure.

## Goal
Efficiently manage player groups by supporting clan creation, merging, membership checks, disbanding, and clan inspection.

## What was done
- implemented disjoint set union with path compression
- used union by rank for more efficient clan merging
- tracked clan members explicitly for inspection and reset
- supported merging two clans
- checked whether two users belong to the same clan
- implemented clan disbanding with reset of all members
- added test scenarios for clan operations

## Result
Built a group management system based on union-find, combining efficient set operations with practical clan-level functionality.
