#include <vector>
#include <iostream>

class ClanWars {
private:
    std::vector<int> leader; // Кто лидер каждого пользователя
    std::vector<int> height; // Высота дерева для оптимизации
    std::vector<std::vector<int>> clans; // Кто в каком клане

public:
    // Создаём n пользователей, каждый в своём клане
    ClanWars(int n) {
        leader.resize(n);
        height.resize(n, 0);
        clans.resize(n);
        for (int i = 0; i < n; ++i) {
            leader[i] = i; // Каждый сам себе лидер
            clans[i] = {i}; // Клан из одного человека
        }
    }

    // Ищем лидера клана
    int findClan(int user) {
        if (leader[user] != user) {
            leader[user] = findClan(leader[user]); // Сжимаем путь
        }
        return leader[user];
    }

    // Объединяем два клана 
    void uniteClans(int user1, int user2) {
        int clan1 = findClan(user1);
        int clan2 = findClan(user2);
        
        if (clan1 == clan2) return; // Они уже вместе

        // Простое объединение по высоте
        if (height[clan1] < height[clan2]) {
            leader[clan1] = clan2;
        } else {
            leader[clan2] = clan1;
            if (height[clan1] == height[clan2]) {
                height[clan1]++;
            }
        }
        
        // Перемещаем всех из одного клана в другой
        clans[clan1].insert(clans[clan1].end(), clans[clan2].begin(), clans[clan2].end());
        clans[clan2].clear();
    }

    // Проверяем, в одном ли клане
    bool areInSameClan(int user1, int user2) {
        return findClan(user1) == findClan(user2);
    }

    // Распускаем клан, каждый сам по себе
    void disbandClan(int user) {
        int clan = findClan(user);
        
        // Копирую список, чтобы нич не сломалось в цикле
        std::vector<int> members;
        for (int m : clans[clan]) {
            members.push_back(m);
        }
        
        for (int member : members) {
            leader[member] = member; // Каждый сам себе лидер
            height[member] = 0;     // Сбрасываем высоту
            clans[member] = {member}; // Новый клан
        }
        clans[clan].clear(); // Очищаем старый клан
    }

    // Показываем кто в клане
    void printClan(int user) {
        int clan = findClan(user);
        std::cout << "Клан пользователя " << user << " (" << clans[clan].size() << " чел.): ";
        for (int member : clans[clan]) {
            std::cout << member << " ";
        }
        std::cout << std::endl;
    }

    // Надо посчитат сколько всего кланов
    void printClanCount() {
        int count = 0;
        for (int i = 0; i < leader.size(); ++i) {
            if (leader[i] == i) count++;
        }
        std::cout << "Всего кланов: " << count << std::endl;
    }
};

// Проверяем как всё работает
void testClanWars() {
    std::cout << "Тестирую кланы!\n\n";
    ClanWars game(5); // 5 пользователей: 0, 1, 2, 3, 4

    std::cout << "Сначала все отдельно:\n";
    for (int i = 0; i < 5; ++i) {
        game.printClan(i);
    }
    game.printClanCount();

    std::cout << "\nОбъединяю 0 и 1:\n";
    game.uniteClans(0, 1);
    game.printClan(0);

    std::cout << "\nОбъединяю 2 и 3:\n";
    game.uniteClans(2, 3);
    game.printClan(2);

    std::cout << "\nОбъединяю кланы 1 и 2:\n";
    game.uniteClans(1, 2);
    game.printClan(0);
    game.printClanCount();

    std::cout << "\nПроверяю, вместе ли 0 и 3: ";
    std::cout << (game.areInSameClan(0, 3) ? "Да" : "Нет") << std::endl;

    std::cout << "\nПроверяю, вместе ли 0 и 4: ";
    std::cout << (game.areInSameClan(0, 4) ? "Да" : "Нет") << std::endl;

    std::cout << "\nРаспускаю клан пользователя 0:\n";
    game.disbandClan(0);
    for (int i = 0; i < 5; ++i) {
        game.printClan(i);
    }
    game.printClanCount();
}

int main() {
    testClanWars();
    return 0;
}