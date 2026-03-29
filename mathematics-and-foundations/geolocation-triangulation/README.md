# Geolocation Triangulation

## Overview
This project estimates user location from nearby stations using distance calculations and triangulation.

## Tech stack
Python, pandas, NumPy, folium, shapely, matplotlib

## Goal
Approximate geographic coordinates from station positions and noisy distance measurements, then evaluate localization error.

## What was done
- computed distances between geographic points using the Haversine formula
- selected nearby stations within an adaptive radius
- simulated noisy distance measurements
- handled edge cases with one, two, or multiple stations
- detected collinear station configurations
- estimated user coordinates with an iterative triangulation procedure
- visualized true and predicted locations on an interactive map
- analyzed localization error with summary statistics and plots

## Result
Built a geolocation pipeline that combines distance-based localization, triangulation, mapping, and error analysis for station-based positioning.
