# RAG Question Answering

## Tech stack
Python, pandas, BeautifulSoup, NLTK, SentenceTransformers, Chroma DB, Google Gemini API, NumPy

## Overview
This project implements a retrieval-augmented generation pipeline for technical question answering on Stack Overflow data.

## Goal
Combine semantic retrieval and LLM-based generation to answer user questions using relevant context from a technical knowledge base.

## What was done
- loaded and cleaned Stack Overflow question and answer data
- generated sentence embeddings for combined question-answer text
- stored embeddings in Chroma DB
- retrieved relevant context using semantic search
- built prompts from retrieved documents
- generated answers with a large language model
- evaluated retrieval quality and compared the pipeline with earlier retrieval-based approaches

## Result
Built an end-to-end RAG-style question answering workflow combining vector retrieval and LLM generation for technical Q&A search.
