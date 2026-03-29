import pandas as pd
import re
import csv
from bs4 import BeautifulSoup
import numpy as np
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.metrics.pairwise import cosine_similarity
from rank_bm25 import BM25Okapi
from nltk.corpus import stopwords
from nltk.stem import WordNetLemmatizer
import nltk
import ssl
import gdown
from collections import defaultdict

# Отключение проверки SSL для NLTK
try:
    _create_unverified_https_context = ssl._create_unverified_context
except AttributeError:
    pass
else:
    ssl._create_default_https_context = _create_unverified_https_context

nltk.download('stopwords')
nltk.download('wordnet')
nltk.download('omw-1.4')

stop_words = set(stopwords.words('english'))
lemmatizer = WordNetLemmatizer()

# Функция очистки текста
def clean_text(text):
    if pd.isna(text):
        return ""
    text = BeautifulSoup(text, "html.parser").get_text()
    text = re.sub(r'<code>.*?</code>', '', text, flags=re.DOTALL)
    text = text.lower()
    text = re.sub(r'[^a-zA-Z\s]', '', text)
    text = re.sub(r'\s+', ' ', text).strip()
    words = text.split()
    words = [lemmatizer.lemmatize(word) for word in words if word not in stop_words]
    return ' '.join(words)

# Загрузка данных
questions_url = "https://drive.google.com/uc?id=1DP6yJRTjbklI7uM4VCQAQavffIqpf-YY"
answers_url = "https://drive.google.com/uc?id=1SskYtkXLLPIUIDT9kYL8VAYr2AXCCxMC"
tags_url = "https://drive.google.com/uc?id=1QDXBI5NsLryLfdQKTTDbSqlCty3nX4ZP"

questions_path = "Questions.csv"
answers_path = "Answers.csv"
tags_path = "Tags.csv"

print("Загрузка данных...")
gdown.download(questions_url, questions_path, quiet=False)
gdown.download(answers_url, answers_path, quiet=False)
gdown.download(tags_url, tags_path, quiet=False)

# Чтение данных
questions = pd.read_csv(questions_path, encoding='latin1', nrows=10000, quoting=csv.QUOTE_ALL, escapechar='\\', on_bad_lines='skip')
answers = pd.read_csv(answers_path, encoding='latin1', nrows=100000, quoting=csv.QUOTE_ALL, escapechar='\\', on_bad_lines='skip')
tags = pd.read_csv(tags_path, encoding='latin1', nrows=10000, quoting=csv.QUOTE_ALL, escapechar='\\', on_bad_lines='skip')

# Приведение типов
questions['Id'] = questions['Id'].astype(str)
answers['ParentId'] = answers['ParentId'].astype(str)

# Очистка текста
questions['Cleaned_Body'] = questions['Body'].apply(clean_text)
answers['Cleaned_Body'] = answers['Body'].apply(clean_text)

# Проверка ID
print("Первые 5 ID вопросов:", questions['Id'].head().tolist())
print("Первые 5 ParentId ответов:", answers['ParentId'].head().tolist())

# Фильтрация вопросов с ответами
questions_with_answers = questions[questions['Id'].isin(answers['ParentId'])]
print(f"Вопросов с ответами: {len(questions_with_answers)}")
print("Примеры вопросов с ответами:")
print(questions_with_answers[['Id', 'Title']].head(5))

# Проверка наличия ответов
print("\nПроверка ответов для первых 5 вопросов с ответами:")
for qid in questions_with_answers['Id'].head():
    relevant = answers[answers['ParentId'] == qid]
    print(f"Question ID {qid}: {'Ответы найдены' if not relevant.empty else 'Ответы не найдены'}")

# Сброс индексов для корректной работы поиска
questions_with_answers = questions_with_answers.reset_index(drop=True)

# Инвертированный индекс
def build_inverted_index(documents):
    inverted_index = defaultdict(list)
    for doc_id, text in enumerate(documents):
        words = text.split()
        word_counts = defaultdict(int)
        for word in words:
            word_counts[word] += 1
        for word, count in word_counts.items():
            inverted_index[word].append((doc_id, count))
    return inverted_index

inverted_index = build_inverted_index(questions_with_answers['Cleaned_Body'])
print("Инвертированный индекс построен.")

# TF-IDF векторизация
vectorizer = TfidfVectorizer()
tfidf_matrix = vectorizer.fit_transform(questions_with_answers['Cleaned_Body'])

# BM25
tokenized_corpus = [doc.split() for doc in questions_with_answers['Cleaned_Body']]
bm25 = BM25Okapi(tokenized_corpus)

# Функция поиска
def search(query, method='tfidf', top_k=5):
    cleaned_query = clean_text(query)
    if method == 'tfidf':
        query_vector = vectorizer.transform([cleaned_query])
        similarities = cosine_similarity(query_vector, tfidf_matrix).flatten()
    elif method == 'bm25':
        tokenized_query = cleaned_query.split()
        similarities = bm25.get_scores(tokenized_query)
    top_indices = similarities.argsort()[-top_k:][::-1]
    top_scores = similarities[top_indices]
    results = questions_with_answers.iloc[top_indices][['Id', 'Title', 'Cleaned_Body']].copy()
    results['Similarity'] = top_scores
    return results

# Извлечение лучшего ответа
def get_best_answer(question_id):
    relevant_answers = answers[answers['ParentId'] == str(question_id)]
    if not relevant_answers.empty:
        best_answer = relevant_answers.loc[relevant_answers['Score'].idxmax()]
        return best_answer['Cleaned_Body'][:500]  # Ограничиваем длину
    return "Ответ не найден (ParentId не существует в answers)."

# Поиск с ответами
def search_with_answers(query, method='tfidf', top_k=5):
    results = search(query, method, top_k)
    results['Best_Answer'] = results['Id'].apply(get_best_answer)
    return results

# Оценка качества (Precision@5)
def evaluate_precision_at_k(test_queries, method='tfidf', k=5):
    precision_scores = []
    for query, expected_question_id in test_queries:
        if str(expected_question_id) not in questions_with_answers['Id'].values:
            print(f"Ошибка: вопрос с ID {expected_question_id} не найден в датасете.")
            precision_scores.append(0.0)
            continue
        results = search(query, method, k)
        relevant = str(expected_question_id) in results['Id'].values
        precision = 1.0 if relevant else 0.0
        precision_scores.append(precision)
    return np.mean(precision_scores) if precision_scores else 0.0

# Тестовые запросы
test_queries = [
    ("python matrix operations", "211160"),
    ("python unit testing", "217900"),
]

precision_tfidf = evaluate_precision_at_k(test_queries, method='tfidf')
precision_bm25 = evaluate_precision_at_k(test_queries, method='bm25')
print(f"Precision@5 (TF-IDF): {precision_tfidf:.2f}")
print(f"Precision@5 (BM25): {precision_bm25:.2f}")

# Пример поиска
query = "how to sort a list in python"
print("\nПример поиска (TF-IDF):")
results_tfidf = search_with_answers(query, method='tfidf')
print(results_tfidf[['Id', 'Title', 'Similarity', 'Best_Answer']])

print("\nПример поиска (BM25):")
results_bm25 = search_with_answers(query, method='bm25')
print(results_bm25[['Id', 'Title', 'Similarity', 'Best_Answer']])

# Проверка, найдены ли ответы
print("\nСтатистика ответов (TF-IDF):")
print(results_tfidf['Best_Answer'].value_counts())
print("\nСтатистика ответов (BM25):")
print(results_bm25['Best_Answer'].value_counts())

# Создание отчёта
with open('report.txt', 'w', encoding='utf-8') as f:
    f.write("# Лабораторная работа №4\n\n")
    f.write("## Описание\n")
    f.write("Реализован поиск по датасету StackSample с использованием TF-IDF и BM25. "
            "Построен инвертированный индекс, выполняется поиск топ-5 вопросов, извлекаются лучшие ответы.\n\n")
    f.write("## Метрики\n")
    f.write(f"- Precision@5 (TF-IDF): {precision_tfidf:.2f}\n")
    f.write(f"- Precision@5 (BM25): {precision_bm25:.2f}\n")
    f.write("**Примечание**: Метрики зависят от тестовых запросов. "
            "Использованы запросы, связанные с Python (матрицы и unit testing).\n\n")
    f.write("## Плюсы и минусы\n")
    f.write("- **Плюсы**: Быстрая обработка, нахождение реальных ответов, высокая точность BM25.\n")
    f.write("- **Минусы**: Ограниченная выборка (371 вопрос), отсутствие вопросов о сортировке списков.\n\n")
    f.write("## Пример\n")
    f.write(f"Запрос: {query}\n")
    f.write("### TF-IDF\n")
    f.write(results_tfidf[['Id', 'Title', 'Similarity', 'Best_Answer']].to_string())
    f.write("\n### BM25\n")
    f.write(results_bm25[['Id', 'Title', 'Similarity', 'Best_Answer']].to_string())