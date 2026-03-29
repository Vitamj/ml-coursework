
#include <iostream>
#include <fstream>
#include <vector>
#include <string>
#include <sstream>
#include <cmath>
#include <algorithm> 

const int HASH_DIM = 1000; 

struct Sample {
    std::vector<double> features;
    int label;
};

std::vector<double> text_to_features(const std::string& text) {
    std::vector<double> feats(HASH_DIM, 0.0); 
    std::string my_text = text;

    for (int i = 0; i < my_text.length(); i++) {
        my_text[i] = std::tolower(my_text[i]);
    }

    std::istringstream iss(my_text);
    while (iss >> word) {
        uint32_t hash;
        MurmurHash3(word.c_str(), (int)word.length(), 0, &hash); 
        feats[index] = feats[index] + 1; 
    }

    return feats; 
}

class LogisticRegression {
public:
    std::vector<double> weights; // веса модели
    double learning_rate = 0.0001; // скорость обучения
    int epochs = 200; // количество итераций
    double class_weight_0 = 1.0; // вес для класса 0 (ham)
    double class_weight_1 = 0.1; // вес для класса 1 (spam)

    LogisticRegression() {
        weights.resize(HASH_DIM); // задаем размер
        for (int i = 0; i < HASH_DIM; i++) {
            weights[i] = 0.0; // обнуляем веса
        }
    }

    void train(const std::vector<Sample>& trainData, const std::vector<Sample>& validData) {

        for (int e = 0; e < epochs; e++) {
            std::cout << 'Starting epoch' << e << std::endl;
            for (int i = 0; i < trainData.size(); i++) {
                double pred = sigmoid(dot_product(weights, trainData[i].features));
                double error = pred - trainData[i].label;
                double weight = (trainData[i].label == 0) ? class_weight_0 : class_weight_1;

                
                for (int j = 0; j < HASH_DIM; j++) {
                    weights[j] = weights[j] - learning_rate * error * trainData[i].features[j] * weight;
                }
            }
        }
    }

    std::vector<double> evaluate(const std::vector<Sample>& data) {
        std::vector<double> metrics(4); // TP, TN, FP, FN
        metrics[0] = 0; metrics[1] = 0; metrics[2] = 0; metrics[3] = 0;

        for (int i = 0; i < data.size(); i++) {
            int pred = predict(data[i].features);
            if (pred == 1 && data[i].label == 1) metrics[0] = metrics[0] + 1; // TP
            if (pred == 0 && data[i].label == 0) metrics[1] = metrics[1] + 1; // TN
            if (pred == 1 && data[i].label == 0) metrics[2] = metrics[2] + 1; // FP
            if (pred == 0 && data[i].label == 1) metrics[3] = metrics[3] + 1; // FN
        }
        return metrics;
    }

    int predict(const std::vector<double>& feats) {
        double val = sigmoid(dot_product(weights, feats));
        if (val >= 0.5) return 1;
        return 0;
    }

private:
    double dot_product(const std::vector<double>& w, const std::vector<double>& x) {
        double res = 0.0;
        for (int i = 0; i < w.size(); i++) {
            res = res + w[i] * x[i];
        }
        return res;
    }

    double sigmoid(double z) {
        return 1.0 / (1.0 + exp(-z));
    }
};

bool read_csv(const std::string& filename, std::vector<Sample>& data) {
    std::ifstream file(filename);
    if (!file.is_open()) {
        std::cout << "Не могу открыть файл " << filename << "!\n";
        return false;
    }

    std::string line;
    int line_num = 0;
    if (filename == "entrypoint/data_train.csv") {
        std::getline(file, line); 
        std::cout << "Заголовок в " << filename << ": " << line << "\n";
        line_num++;
    }

    while (std::getline(file, line)) {
        line_num++;
        if (line == "") {
            std::cout << "Пустая строка в " << filename << " на " << line_num << "\n";
            continue;
        }

        std::istringstream iss(line);
        std::string label_str, text;
        int label = -1;

        if (std::getline(iss, label_str, ',')) {
            if (label_str == "spam") label = 1;
            else if (label_str == "ham") label = 0;
            else {
                std::cout << "Странная метка в " << filename << " на строке " << line_num << ": " << label_str << "\n";
                continue;
            }
            std::getline(iss, text);
            if (text != "") {
                Sample s;
                s.features = text_to_features(text);
                s.label = label;
                data.push_back(s);
            }
        } else {
            iss.clear();
            iss.str(line);
            if (std::getline(iss, label_str, '\t')) {
                if (label_str == "spam") label = 1;
                else if (label_str == "ham") label = 0;
                else {
                    std::cout << "Странная метка в " << filename << " на строке " << line_num << ": " << label_str << "\n";
                    continue;
                }
                std::getline(iss, text);
                if (text != "") {
                    Sample s;
                    s.features = text_to_features(text);
                    s.label = label;
                    data.push_back(s);
                }
            } else {
                std::cout << "Ошибка формата в " << filename << " на строке " << line_num << ": " << line << "\n";
            }
        }
    }

    file.close();
    return true;
}
