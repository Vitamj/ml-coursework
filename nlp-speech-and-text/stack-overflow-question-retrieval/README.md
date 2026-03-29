# Stack Overflow Question Retrieval

## Tech stack
Python, pandas, BeautifulSoup, NLTK, scikit-learn, rank-bm25, NumPy

## Overview
This project implements a simple question retrieval system on Stack Overflow data using text preprocessing, inverted indexing, TF-IDF, and BM25 ranking.

## Goal
Retrieve the most relevant questions for a user query and extract the best available answers from the dataset.

## What was done
- loaded and merged Stack Overflow question and answer data
- cleaned and normalized raw HTML text
- built an inverted index for question text
- implemented retrieval using TF-IDF and BM25
- ranked top matching questions by similarity
- extracted the highest-scored answer for each retrieved question
- evaluated retrieval quality with Precision@5

## Result
Built a lightweight question retrieval pipeline for technical Q&A data and compared TF-IDF and BM25 approaches on sample search queries.
