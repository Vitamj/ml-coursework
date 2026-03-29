#include <sstream>
#include <string>
#include <cmath>
#include <limits>

enum Color {
    RED,
    BLACK
};

struct Node {
    std::string film_name;
    double sum_ratings;
    double avg_rating;
    int count_ratings;

    Color color;
    Node *left, *right, *parent;

    Node(const std::string &name, double rating)
        : film_name(name), sum_ratings(rating), avg_rating(rating), count_ratings(1),
        color(RED), left(nullptr), right(nullptr), parent(nullptr) {
    }

    /**
     * Обновить значение рейтиинга.
     *
     * @note При добавлении новой оценки к фильму:
     *
        - Обновите сумму оценок и увеличьте количество оценок.

        - Пересчитайте среднюю оценку.
        Если текущая средняя оценка равна `avg`,
        количество оценок равно `n`,
        а новая оценка равна `r`,
        то новое значение средней оценки вычисляется по формуле:

        `новое среднее = ((avg × n) + r) / (n + 1)`

     */
    void updateRating(double newRating) {
        sum_ratings += newRating;
        count_ratings++;
        avg_rating = sum_ratings / count_ratings;
    }
};


class RBTree {
public:
    Node *root;

    RBTree() : root(nullptr) {}

    /**
     * Левое вращение вокруг узла x.
     *
     * @param x нода, вокруг которой будет выполняться вращение
     */
    void leftRotate(Node *x) {
        Node *y = x->right;
        x->right = y->left;
        if (y->left != nullptr)
            y->left->parent = x;
        y->parent = x->parent;
        if (x->parent == nullptr)
            root = y;
        else if (x == x->parent->left)
            x->parent->left = y;
        else
            x->parent->right = y;
        y->left = x;
        x->parent = y;
    }

    /**
     * Правок вращение вокруг узла x.
     *
     * @param x нода, вокруг которой будет выполняться вращение
     */
    void rightRotate(Node *y) {
        Node *x = y->left;
        y->left = x->right;
        if (x->right != nullptr)
            x->right->parent = y;
        x->parent = y->parent;
        if (y->parent == nullptr)
            root = x;
        else if (y == y->parent->right)
            y->parent->right = x;
        else
            y->parent->left = x;
        x->right = y;
        y->parent = x;
    }

    /**
     * Вставить/обновить фильм и рейтинг
     *
     * @param film_name название фильма
     * @param rating рейтинг
     */
    void insert(const std::string &film_name, double rating) {
        Node *node = new Node(film_name, rating);
        if (root == nullptr) {
            root = node;
            root->color = BLACK;
            return;
        }
        Node *current = root;
        Node *parent = nullptr;
        while (current != nullptr) {
            parent = current;
            if (film_name < current->film_name)
                current = current->left;
            else if (film_name > current->film_name)
                current = current->right;
            else {
                current->updateRating(rating);
                delete node;
                return;
            }
        }

        node->parent = parent;
        if (film_name < parent->film_name)
            parent->left = node;
        else
            parent->right = node;

        fixInsert(node);
    }


    /**
     * Восстановить свойства красно-черного дерева после вставки.
     *
     * @param node вставленный узел, который нужно исправить
     */
    void fixInsert(Node *node) {
        while (node->parent != nullptr && node->parent->color == RED) {
            if (node->parent == node->parent->parent->left) {
                Node *uncle = node->parent->parent->right;
                if (uncle != nullptr && uncle->color == RED) {
                    node->parent->color = BLACK;
                    uncle->color = BLACK;
                    node->parent->parent->color = RED;
                    node = node->parent->parent;
                } else {
                    if (node == node->parent->right) {
                        node = node->parent;
                        leftRotate(node);
                    }
                    node->parent->color = BLACK;
                    node->parent->parent->color = RED;
                    rightRotate(node->parent->parent);
                }
            } else {
                Node *uncle = node->parent->parent->left;
                if (uncle != nullptr && uncle->color == RED) {
                    node->parent->color = BLACK;
                    uncle->color = BLACK;
                    node->parent->parent->color = RED;
                    node = node->parent->parent;
                } else {
                    if (node == node->parent->left) {
                        node = node->parent;
                        rightRotate(node);
                    }
                    node->parent->color = BLACK;
                    node->parent->parent->color = RED;
                    leftRotate(node->parent->parent);
                }
            }
        }
        root->color = BLACK;
    }

    /**
     * Найти фильм по названию
     *
     * @param film_name название фильма
     *
     * @return указатель на ноду, содержащий фильм. nullptr если не найдено.
     */
    Node *search(const std::string &film_name) {
        Node* current = root;

        while (current) {
            if (film_name == current -> film_name)
                return current;

            else if (film_name < current -> film_name)
                current = current -> left;
            else
                current = current -> right;

        }
    }

    /**
     * Обход по порядку для поиска узла со средним рейтингом, наиболее близким к target_rating.
     *
     * @param node
     * @param target_rating таргетный рейтинг
     *
     * @param bestMatch наиболее подходящий узел, найденный на данный момент.
     * @param bestDiff наименьшая разница между целевым и фактическим рейтингами, найденными на данный момент.
     */
    void inOrderRecommend(Node *node, double target_rating, Node *&bestMatch, double &bestDiff) {
        if (node == nullptr)
            return;
        inOrderRecommend(node->left, target_rating, bestMatch, bestDiff);
        double diff = std::abs(node->avg_rating - target_rating);
        if (diff < bestDiff) {
            bestDiff = diff;
            bestMatch = node;
        }
        inOrderRecommend(node->right, target_rating, bestMatch, bestDiff);
    }

    /**
     * Получить рекомендацию — фильм со средним рейтингом, наиболее близким к target_rating.
     *
     * @param target_rating таргет тейтинг
     *
     * @return нода на лучшее совпадение. если не найдено - nullptr
     */
    Node *recommend(double target_rating) {
        Node *bestMatch = nullptr;
        double bestDiff = std::numeric_limits<double>::max();
        inOrderRecommend(root, target_rating, bestMatch, bestDiff);
        return bestMatch;
    }
};