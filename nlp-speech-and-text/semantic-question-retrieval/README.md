# Semantic Question Retrieval

## Tech stack
Python, pandas, BeautifulSoup, NLTK, SentenceTransformers, Chroma DB, NumPy

## Overview
This project implements semantic question retrieval on Stack Overflow data using sentence embeddings and vector search.

## Goal
Retrieve semantically relevant technical questions and answers for a user query using embedding-based search instead of keyword-only matching.

## What was done
- loaded and cleaned Stack Overflow question and answer data
- combined question and answer text into a unified retrieval corpus
- generated sentence embeddings with SentenceTransformers
- stored vector representations in Chroma DB
- implemented semantic search over technical Q&A data
- evaluated retrieval quality with Precision@5
- compared semantic retrieval with earlier TF-IDF and BM25 approaches
- 
## Result
Built an embedding-based retrieval pipeline for technical Q&A search and explored the practical difference between lexical and semantic retrieval methods.
