import pandas as pd
import re
import csv
from bs4 import BeautifulSoup
import numpy as np
from nltk.corpus import stopwords
from nltk.stem import WordNetLemmatizer
import nltk
import ssl
import gdown
from sentence_transformers import SentenceTransformer
import chromadb
from chromadb.config import Settings
import os
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
    if pd.isna(text) or not isinstance(text, str):
        return ""
    # Проверяем, содержит ли текст HTML-теги
    if '<' in text and '>' in text:
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

# Проверка данных
print("Проверка данных Questions.csv:")
print(questions['Id'].describe())
print("Уникальных Id в Questions:", questions['Id'].nunique())
print("Примеры Id:", questions['Id'].head(10).tolist())
print("Некорректные Id:", questions[~questions['Id'].str.match(r'^\d+$', na=False)]['Id'].tolist())

# Приведение типов и очистка
questions['Id'] = questions['Id'].astype(str)
answers['ParentId'] = answers['ParentId'].astype(str)
tags['Id'] = tags['Id'].astype(str)

# Удаляем записи с некорректными Id
questions = questions[questions['Id'].str.match(r'^\d+$')]
questions = questions.reset_index(drop=True)
print(f"Вопросов после очистки: {len(questions)}")

# Очистка текста
questions['Cleaned_Body'] = questions['Body'].apply(clean_text)
answers['Cleaned_Body'] = answers['Body'].apply(clean_text)

# Фильтрация вопросов с ответами
questions_with_answers = questions[questions['Id'].isin(answers['ParentId'])]
questions_with_answers = questions_with_answers.drop_duplicates(subset=['Id']).reset_index(drop=True)
print(f"Вопросов с ответами: {len(questions_with_answers)}")

# Проверка Id в questions_with_answers
print("Примеры Id в questions_with_answers:", questions_with_answers['Id'].head(10).tolist())
print("Проверка уникальности Id в questions_with_answers:")
duplicate_ids = questions_with_answers[questions_with_answers['Id'].duplicated()]['Id'].tolist()
print(f"Дублирующиеся Id: {duplicate_ids}")

# Извлечение лучшего ответа
def get_best_answer(question_id):
    relevant_answers = answers[answers['ParentId'] == str(question_id)]
    if relevant_answers.empty or relevant_answers['Score'].isna().all():
        print(f"Нет ответов или все Score NaN для вопроса ID: {question_id}")
        return ""
    best_answer = relevant_answers.loc[relevant_answers['Score'].idxmax()]
    return best_answer['Cleaned_Body'][:500]

# Объединение вопроса и ответа
questions_with_answers['Best_Answer'] = questions_with_answers['Id'].apply(get_best_answer)
questions_with_answers['Combined_Text'] = questions_with_answers.apply(
    lambda row: f"{row['Cleaned_Body']} {row['Best_Answer']}", axis=1
)

# Получение тегов
def get_tags(question_id):
    relevant_tags = tags[tags['Id'] == str(question_id)]['Tag']
    return ','.join(relevant_tags) if not relevant_tags.empty else ""

questions_with_answers['Tags'] = questions_with_answers['Id'].apply(get_tags)

# Генерация эмбеддингов
print("Загрузка модели SentenceTransformers...")
model = SentenceTransformer('all-MiniLM-L6-v2')
print("Генерация эмбеддингов...")
embeddings = model.encode(questions_with_answers['Combined_Text'].tolist(), batch_size=32, show_progress_bar=True)
print(f"Размерность эмбеддингов: {embeddings.shape}")

# Инициализация Chroma DB
print("Инициализация Chroma DB...")
chroma_client = chromadb.PersistentClient(path="./chroma_db")
collection = chroma_client.get_or_create_collection(name="StackOverflowQnA")

# Загрузка эмбеддингов в Chroma
print("Загрузка эмбеддингов в Chroma...")
batch_size = 100
for i in range(0, len(questions_with_answers), batch_size):
    batch = questions_with_answers[i:i + batch_size]
    batch_embeddings = embeddings[i:i + batch_size].tolist()
    batch_ids = [f"question_{id}" for id in batch['Id']]
    batch_metadatas = [
        {
            "question_text": row['Cleaned_Body'],
            "answer_text": row['Best_Answer'],
            "title": row['Title'],
            "tags": row['Tags']
        } for _, row in batch.iterrows()
    ]
    print(f"Загрузка батча {i//batch_size + 1}, IDs: {batch_ids[:5]}")
    collection.add(
        ids=batch_ids,
        embeddings=batch_embeddings,
        metadatas=batch_metadatas
    )

print(f"Загружено записей в Chroma: {collection.count()}")

# Функция семантического поиска
def semantic_search(query, top_k=5):
    query_embedding = model.encode([clean_text(query)])[0].tolist()
    results = collection.query(
        query_embeddings=[query_embedding],
        n_results=top_k
    )
    output = []
    for id, metadata, distance in zip(results['ids'][0], results['metadatas'][0], results['distances'][0]):
        output.append({
            'Id': id.replace('question_', ''),
            'Title': metadata['title'],
            'Question': metadata['question_text'],
            'Answer': metadata['answer_text'],
            'Tags': metadata['tags'],
            'Distance': distance
        })
    return pd.DataFrame(output)

# Оценка Precision@5
def evaluate_precision_at_k(test_queries, k=5):
    precision_scores = []
    for query, expected_id in test_queries:
        results = semantic_search(query, top_k=k)
        relevant = str(expected_id) in results['Id'].values
        precision = 1.0 if relevant else 0.0
        precision_scores.append(precision)
    return np.mean(precision_scores) if precision_scores else 0.0

# Тестовые запросы
test_queries = [
    ("python matrix operations", "211160"),
    ("python unit testing", "217900"),
]

precision = evaluate_precision_at_k(test_queries)
print(f"Precision@5 (Semantic Search): {precision:.2f}")

# Пример поиска
query = "how to sort a list in python"
print("\nПример семантического поиска:")
results = semantic_search(query)
print(results[['Id', 'Title', 'Distance', 'Answer']])

# Сравнение с ЛР4
print("\nСравнение с ЛР4:")
for query, expected_id in test_queries + [(query, None)]:
    print(f"\nЗапрос: {query}")
    print("Семантический поиск:")
    results_semantic = semantic_search(query)
    print(results_semantic[['Id', 'Title', 'Distance']])
    print("TF-IDF/BM25 (ЛР4): См. report.txt из ЛР4")

# Анализ хранения
chroma_dir = "./chroma_db"
if os.path.exists(chroma_dir):
    size = sum(os.path.getsize(os.path.join(chroma_dir, f)) for f in os.listdir(chroma_dir) if os.path.isfile(os.path.join(chroma_dir, f))) / (1024 ** 2)
    print(f"\nРазмер хранилища Chroma: {size:.2f} MB")

# Создание отчёта
with open('report.txt', 'w', encoding='utf-8') as f:
    f.write("# Лабораторная работа №5\n\n")
    f.write("## Описание\n")
    f.write("Реализован семантический поиск по датасету StackSample с использованием SentenceTransformers и Chroma DB. "
            "Эмбеддинги генерируются для объединённого текста вопроса и ответа, хранятся в Chroma, выполняется поиск по смысловому сходству.\n\n")
    f.write("## Метрики\n")
    f.write(f"- Precision@5 (Semantic Search): {precision:.2f}\n")
    f.write("**Примечание**: Использованы запросы, связанные с Python (матрицы и unit testing).\n\n")
    f.write("## Плюсы и минусы\n")
    f.write("- **Плюсы**: Учёт семантики, поддержка синонимов, высокая релевантность для сложных запросов.\n")
    f.write("- **Минусы**: Ограниченная выборка (371 вопрос), высокие требования к памяти, медленная генерация эмбеддингов.\n\n")
    f.write("## Сравнение с ЛР4\n")
    f.write("Семантический поиск лучше учитывает смысл запроса, находя релевантные вопросы даже без точного совпадения слов. "
            "Например, для 'how to sort a list in python' находятся вопросы по Python, но не точные из-за ограничений выборки.\n")
    f.write(f"Размер хранилища Chroma: {size:.2f} MB. Chroma использует ANN-индексы для быстрого поиска.\n\n")
    f.write("## Пример\n")
    f.write(f"Запрос: {query}\n")
    f.write(results[['Id', 'Title', 'Distance', 'Answer']].to_string())