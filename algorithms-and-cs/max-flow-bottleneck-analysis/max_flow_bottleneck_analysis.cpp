#include <iostream>
#include <vector>
#include <queue>
using namespace std;

// Структура для ребра графа
struct Edge {
    int to;       // Куда ведёт ребро (номер вершины)
    int capacity; // Пропускная способность (RPS)
    int flow;     // Текущий поток
};

// Класс для графа
class Graph {
    int V;                    // Количество вершин
    vector<vector<Edge>> adj; // Список смежности

public:
    Graph(int vertices) {
        V = vertices;
        adj.resize(V); // Создаём пустой список для каждой вершины
    }

    // Добавляем ребро в граф
    void addEdge(int u, int v, int capacity) {
        // Прямое ребро: u -> v
        Edge forward;
        forward.to = v;
        forward.capacity = capacity;
        forward.flow = 0;
        adj[u].push_back(forward);

        // Обратное ребро: v -> u (для остаточной сети)
        Edge backward;
        backward.to = u;
        backward.capacity = 0; // Обратное ребро изначально не пропускает поток
        backward.flow = 0;
        adj[v].push_back(backward);
    }

    // Поиск пути с помощью BFS
    bool findPath(int source, int sink, vector<int>& parent) {
        vector<bool> visited(V, false); // Отмечаем, какие вершины уже посетили
        queue<int> q;                   // Очередь для BFS
        q.push(source);
        visited[source] = true;
        parent[source] = -1; // У истока нет родителя

        while (!q.empty()) {
            int u = q.front();
            q.pop();

            // Смотрим все рёбра из вершины u
            for (int i = 0; i < adj[u].size(); i++) {
                int v = adj[u][i].to;
                // Если вершина не посещена и есть остаточная пропускная способность
                if (!visited[v] && adj[u][i].capacity > adj[u][i].flow) {
                    q.push(v);
                    visited[v] = true;
                    parent[v] = u;
                    // Сохраняем индекс ребра, чтобы знать, какое ребро использовать
                    if (v == sink) return true; // Нашли путь до стока
                }
            }
        }
        return false; // Путь не найден
    }

    // Алгоритм Эдмондс-Карпа
    int maxFlow(int source, int sink) {
        int max_flow = 0;
        vector<int> parent(V); // Хранит путь от истока до стока

        // Пока есть путь от истока до стока
        while (findPath(source, sink, parent)) {
            int path_flow = 1000000; // Большое число для нахождения минимума

            // Находим минимальную остаточную пропускную способность
            for (int v = sink; v != source; v = parent[v]) {
                int u = parent[v];
                // Ищем ребро u -> v
                for (int i = 0; i < adj[u].size(); i++) {
                    if (adj[u][i].to == v) {
                        path_flow = min(path_flow, adj[u][i].capacity - adj[u][i].flow);
                        break;
                    }
                }
            }

            // Обновляем потоки вдоль пути
            for (int v = sink; v != source; v = parent[v]) {
                int u = parent[v];
                // Ищем прямое и обратное ребро
                for (int i = 0; i < adj[u].size(); i++) {
                    if (adj[u][i].to == v) {
                        adj[u][i].flow += path_flow; // Увеличиваем поток
                        // Находим обратное ребро
                        for (int j = 0; j < adj[v].size(); j++) {
                            if (adj[v][j].to == u) {
                                adj[v][j].flow -= path_flow; // Уменьшаем поток в обратном
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            max_flow += path_flow; // Добавляем к общему потоку
        }
        return max_flow;
    }

    // Поиск узких мест (bottle-neck)
    void findBottlenecks() {
        cout << "Bottle-neck edges:" << endl;
        for (int u = 0; u < V; u++) {
            for (int i = 0; i < adj[u].size(); i++) {
                if (adj[u][i].flow == adj[u][i].capacity && adj[u][i].capacity > 0) {
                    cout << u << " -> " << adj[u][i].to
                         << " (RPS: " << adj[u][i].capacity << ")" << endl;
                }
            }
        }
    }
};

// Тест 1: Простой граф
void testSimple() {
    // 0: Frontend, 1: Service, 2: DB
    Graph g(3);
    g.addEdge(0, 1, 10); // Frontend -> Service (10 RPS)
    g.addEdge(1, 2, 5);  // Service -> DB (5 RPS)

    cout << "Test 1: Simple Graph" << endl;
    int flow = g.maxFlow(0, 2);
    cout << "Max RPS: " << flow << endl;
    g.findBottlenecks();
}

// Тест 2: Граф с двумя путями
void testTwoPaths() {
    // 0: Frontend, 1: Service1, 2: Service2, 3: DB
    Graph g(4);
    g.addEdge(0, 1, 6); // Frontend -> Service1 (6 RPS)
    g.addEdge(0, 2, 4); // Frontend -> Service2 (4 RPS)
    g.addEdge(1, 3, 5); // Service1 -> DB (5 RPS)
    g.addEdge(2, 3, 4); // Service2 -> DB (4 RPS)

    cout << "Test 2: Two Paths" << endl;
    int flow = g.maxFlow(0, 3);
    cout << "Max RPS: " << flow << endl;
    g.findBottlenecks();
}

// Тест 3: Интернет-магазин
void testEcommerce() {
    // 0: Frontend, 1: Auth, 2: Catalog, 3: DB
    Graph g(4);
    g.addEdge(0, 1, 8);  // Frontend -> Auth (8 RPS)
    g.addEdge(0, 2, 10); // Frontend -> Catalog (10 RPS)
    g.addEdge(1, 3, 7);  // Auth -> DB (7 RPS)
    g.addEdge(2, 3, 4);  // Catalog -> DB (4 RPS)

    cout << "Test 3: E-commerce" << endl;
    int flow = g.maxFlow(0, 3);
    cout << "Max RPS: " << flow << endl;
    g.findBottlenecks();
}

int main() {
    testSimple();
    cout << endl;
    testTwoPaths();
    cout << endl;
    testEcommerce();
    return 0;
}