# Fantasy Battle Simulator

## Overview
This project implements an object-oriented battle simulator for fantasy creatures with configurable abilities, modifiers, spells, and combat behavior.

## Goal
Build a modular combat engine that supports creature creation, combat-time state changes, table-level customization, and extensible battle mechanics.

## Tech stack
C#, object-oriented programming, SOLID, structural patterns, creational patterns, testing

## What was done
- designed an object-oriented model for creatures, modifiers, spells, and player tables
- implemented battle logic with alternating attacks between players
- separated creature states across catalog, player table, and battle context
- supported configurable creature creation through dedicated configuration abstractions
- implemented creature modifiers such as magic shield and double attack
- implemented spells that permanently affect creatures on the player table
- modeled unique creature abilities with different attack and damage-handling behavior
- covered combat scenarios with unit and functional tests

## Result
Built a modular fantasy combat simulator with extensible creature configuration, layered combat state, and support for battle-specific mechanics and modifiers.