# ATM System

## Overview
This project implements an ATM system with account operations, session handling, transaction history, and a web-based interface built with layered architecture principles.

## Goal
Build a modular banking application that supports account management, authenticated sessions, transaction tracking, and clean separation between business logic, storage, and presentation layers.

## What was done
- implemented account creation and session-based authorization
- supported balance checks, deposits, withdrawals, and operation history
- recorded transaction history for all account operations
- designed the system using hexagonal architecture principles
- separated business logic from repositories and web presentation
- used dependency injection through Microsoft.Extensions.DependencyInjection
- implemented an ASP.NET controller-based API
- kept persistence abstract and independent from in-memory storage details
- covered business operations with tests using mocked repositories

## Tech stack
C#, ASP.NET, hexagonal architecture, dependency injection, mocking, testing

## Result
Built a modular ATM application with session-based access, account operations, transaction history, and architecture designed for separation of concerns and testability.