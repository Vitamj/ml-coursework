# Message Routing System

## Overview
This project implements an object-oriented corporate message distribution system with support for users, topics, recipient groups, archiving, notifications, filtering, and logging.

## Tech stack
C#, object-oriented programming, SOLID, GRASP, structural patterns, mocking, testing


## Goal
Model a flexible message delivery system with multiple recipient types, importance-based filtering, extensible integrations, and testable behavior.

## What was done
- designed an object-oriented model for messages, topics, users, and recipients
- implemented message delivery to users, groups, archive services, and notification systems
- supported message importance levels and recipient-side filtering
- implemented per-user message status tracking (read / unread)
- added logging decorators for recipient processing
- integrated formatting and archiving abstractions
- isolated external integrations for notifications and storage
- covered delivery and status scenarios with functional tests and mocks

## Result
Built a modular message routing system with extensible recipient handling, filtering, logging, and integration-friendly architecture.