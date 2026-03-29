#include <iostream>
#include <vector>
#include <queue>
#include <thread>
#include <chrono>
#include <random>
#include <atomic>
#include <mutex>

using namespace std;

struct Graph {
    int V;
    vector<vector<pair<int, int>>> adj;
    vector<vector<pair<int, int>>> rev_adj;

    Graph(int vertices) : V(vertices) {
        adj.resize(V);
        rev_adj.resize(V);
    }

    void addEdge(int u, int v, int weight) {
        adj[u].push_back({v, weight});
        rev_adj[v].push_back({u, weight});
    }
};

vector<int> dijkstra(const Graph& graph, int start) {
    vector<int> dist(graph.V, INT_MAX);
    priority_queue<pair<int, int>, vector<pair<int, int>>, greater<pair<int, int>>> pq;

    dist[start] = 0;
    pq.push({0, start});

    while (!pq.empty()) {
        int u = pq.top().second;
        pq.pop();

        for (auto edge : graph.adj[u]) {
            int v = edge.first;
            int weight = edge.second;

            if (dist[u] + weight < dist[v]) {
                dist[v] = dist[u] + weight;
                pq.push({dist[v], v});
            }
        }
    }
    return dist;
}

struct ThreadData {
    const Graph& graph;
    int start; 
    vector<int>& dist;
    vector<bool>& visited;
    atomic<bool>& stop;
    atomic<int>& best_dist;
    mutex& mtx;
    int& meeting_node;

    ThreadData(const Graph& g, int s, vector<int>& d, vector<bool>& v,
               atomic<bool>& st, atomic<int>& bd, mutex& m, int& mn)
        : graph(g), start(s), dist(d), visited(v), stop(st), best_dist(bd), mtx(m), meeting_node(mn) {}
};

void dijkstra_thread(ThreadData& data, vector<int>& other_dist, bool forward) {
    priority_queue<pair<int, int>, vector<pair<int, int>>, greater<pair<int, int>>> pq;
    data.dist[data.start] = 0;
    pq.push({0, data.start});

    while (!pq.empty() && !data.stop.load()) {
        int u = pq.top().second;
        int current_dist = pq.top().first;
        pq.pop();

        if (data.visited[u]) continue;
        data.visited[u] = true;


        if (other_dist[u] != INT_MAX) {
            lock_guard<mutex> lock(data.mtx);
            int total = current_dist + other_dist[u];
            if (total < data.best_dist.load()) {
                data.best_dist.store(total);
                data.meeting_node = u;
                data.stop.store(true);
            }
        }

        if (current_dist > data.best_dist.load()) {
            continue;
        }

        const auto& edges = forward ? data.graph.adj[u] : data.graph.rev_adj[u];
        for (auto edge : edges) {
            int v = edge.first;
            int weight = edge.second;

            if (data.dist[v] > current_dist + weight) {
                data.dist[v] = current_dist + weight;
                pq.push({data.dist[v], v});
            }
        }
    }
}

int bidirectionalDijkstra(const Graph& graph, int start, int end, int& meeting_node, vector<int>& dist_start, vector<int>& dist_end) {
    dist_start.assign(graph.V, INT_MAX);
    vector<bool> visited_start(graph.V, false);
    dist_end.assign(graph.V, INT_MAX);
    vector<bool> visited_end(graph.V, false);

    atomic<bool> stop(false);
    atomic<int> best_dist(INT_MAX);
    mutex mtx;
    meeting_node = -1;

    ThreadData data_start(graph, start, dist_start, visited_start, stop, best_dist, mtx, meeting_node);
    ThreadData data_end(graph, end, dist_end, visited_end, stop, best_dist, mtx, meeting_node);

    thread t1(dijkstra_thread, ref(data_start), ref(dist_end), true);
    thread t2(dijkstra_thread, ref(data_end), ref(dist_start), false);

    t1.join();
    t2.join();


    int final_dist = best_dist.load();
    for (int u = 0; u < graph.V; ++u) {
        if (dist_start[u] != INT_MAX && dist_end[u] != INT_MAX) {
            int total = dist_start[u] + dist_end[u];
            if (total < final_dist) {
                final_dist = total;
                meeting_node = u;
            }
        }
    }

    return final_dist != INT_MAX ? final_dist : -1;
}

int main() {
    cout << "Тест двунаправленного поиска Дейкстры\n\n";

    const int V = 1000;
    const int num_tests = 50;
    Graph g(V);

    // Генерация графа
    random_device rd;
    mt19937 gen(rd());
    uniform_int_distribution<> vertex_dis(0, V - 1);
    uniform_int_distribution<> weight_dis(1, 100);

    for (int i = 0; i < V - 1; ++i) {
        g.addEdge(i, i + 1, weight_dis(gen));
    }

    const int edges = V * 5;
    for (int i = 0; i < edges; ++i) {
        int u = vertex_dis(gen);
        int v = vertex_dis(gen);
        if (u != v) {
            g.addEdge(u, v, weight_dis(gen));
        }
    }

    vector<double> single_times;
    vector<double> bi_times;
    vector<int> single_dists;
    vector<int> bi_dists;
    vector<int> meeting_nodes;

    for (int i = 0; i < num_tests; ++i) {
        // Однопоток
        auto start = chrono::high_resolution_clock::now();
        vector<int> dist = dijkstra(g, 0);
        int single_dist = dist[V - 1];
        auto end = chrono::high_resolution_clock::now();
        single_times.push_back(chrono::duration_cast<chrono::nanoseconds>(end - start).count() / 1000.0);
        single_dists.push_back(single_dist);

        // Двупоток
        vector<int> dist_start, dist_end;
        int meeting_node;
        start = chrono::high_resolution_clock::now();
        int bi_dist = bidirectionalDijkstra(g, 0, V - 1, meeting_node, dist_start, dist_end);
        end = chrono::high_resolution_clock::now();
        bi_times.push_back(chrono::duration_cast<chrono::nanoseconds>(end - start).count() / 1000.0);
        bi_dists.push_back(bi_dist);
        meeting_nodes.push_back(meeting_node);

        if (single_dist != bi_dist) {
            cout << "Ошибка: итерация " << i << ", однопоточное: " << single_dist
                 << ", двупоточное: " << bi_dist << ", узел встречи: " << meeting_node << endl;
            cout << "dist_start[" << meeting_node << "] = " << dist_start[meeting_node]
                 << ", dist_end[" << meeting_node << "] = " << dist_end[meeting_node] << endl;
        }
    }

    cout << "\nОднопоточные результаты:\n";
    cout << "Времена (мкс): ";
    for (double t : single_times) cout << t << " ";
    cout << "\nРасстояния: ";
    for (int d : single_dists) cout << d << " ";
    cout << endl;

    cout << "\nДвупоточные результаты:\n";
    cout << "Времена (мкс): ";
    for (double t : bi_times) cout << t << " ";
    cout << "\nРасстояния: ";
    for (int d : bi_dists) cout << d << " ";
    cout << "\nУзлы встречи: ";
    for (int n : meeting_nodes) cout << n << " ";
    cout << endl;

    return 0;
}