# Spam Classification with Hash Features

## Overview
This project implements spam classification by converting raw text into hashed numerical features and training a logistic regression model.

## Goal
Build a lightweight text classification pipeline that transforms messages into fixed-size feature vectors and predicts whether a message is spam or ham.

## What was done
- tokenized raw text data from CSV files
- converted text into hashed feature vectors using MurmurHash3
- represented each message as a fixed-length numerical vector
- implemented logistic regression training from scratch
- handled class imbalance with class weights
- evaluated predictions with classification metrics
- analyzed hash collisions and their effect on feature quality

## Tech stack
C++, hashing, logistic regression, text classification, algorithms

## Result
Built a compact text classification pipeline combining feature hashing and logistic regression for spam detection.
