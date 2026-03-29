import folium
import numpy as np
import pandas as pd
from shapely.geometry import Point
import matplotlib.pyplot as plt



def haversine_distance(point1: Point, point2: Point) -> float:
    """
    Считаю расстояние между точками в метрах (гуглила формулу гаверсинусов)
    Не уверена правильно ли но вроде работает
    """
    lon1, lat1 = point1.x, point1.y
    lon2, lat2 = point2.x, point2.y
    R = 6371000  # Радиус Земли, нашла в интернете
    phi1 = np.radians(lat1)
    phi2 = np.radians(lat2)
    delta_phi = np.radians(lat2 - lat1)
    delta_lambda = np.radians(lon2 - lon1)
    a = np.sin(delta_phi / 2) ** 2 + np.cos(phi1) * np.cos(phi2) * np.sin(delta_lambda / 2) ** 2
    c = 2 * np.arctan2(np.sqrt(a), np.sqrt(1 - a))
    return R * c

def are_stations_collinear(stations: pd.DataFrame) -> bool:
    """
    Проверяю, не лежат ли станции на одной линии
    нашла про углы между точками
    Если угол почти 0 или 180 градусовто станции на одной линии
    """
    if len(stations) < 3:
        return False

    coords = stations[['lat', 'lon']].values
    p1, p2, p3 = coords[:3]
    v1 = p2 - p1
    v2 = p3 - p1
    cos_angle = np.dot(v1, v2) / (np.linalg.norm(v1) * np.linalg.norm(v2) + 1e-6)
    is_collinear = abs(cos_angle) > 0.999
    if is_collinear:
        print("Обнаружена коллинеарность станций")
    return is_collinear

def find_nearby_stations(
    lat: float,
    lon: float,
    stations: pd.DataFrame,
    R: float,
    max_stations: int = 5
) -> pd.DataFrame:
    """
    Найти ближайшие станции в радиусе R от точки
    """
    # Создаю точку для человека
    user_point = Point(lon, lat)

    # Список для станций, которые в радиусе R
    nearby = []
    current_R = R

    while len(nearby) < 3 and current_R <= 5000:
        for idx, station in stations.iterrows():
            station_point = Point(station['lon'], station['lat'])
            distance = haversine_distance(user_point, station_point)
            if distance <= current_R:
                nearby.append(station)
        current_R *= 2

    if nearby:
        nearby_df = pd.DataFrame(nearby)
        nearby_df['distance'] = nearby_df.apply(
            lambda row: haversine_distance(Point(row['lon'], row['lat']), user_point), axis=1
        )
        # Беру случайное число станций от 3 до max_stations
        num_to_take = np.random.randint(3, min(max_stations + 1, len(nearby_df) + 1))
        nearby_df = nearby_df.nsmallest(num_to_take, 'distance').drop(columns=['distance'])
        return nearby_df
    return pd.DataFrame()


def compute_distances(
    user_lat: float,
    user_lon: float,
    nearby_stations: pd.DataFrame
) -> np.ndarray:
    """
    Вычислить расстояния от точки до всех ближайших станций
    """
    if nearby_stations.empty:
        return np.array([])

    distances = []
    user_point = Point(user_lon, user_lat)

    for _, station in nearby_stations.iterrows():
        station_point = Point(station['lon'], station['lat'])
        distance = haversine_distance(user_point, station_point)
        noise = abs(np.random.normal(0, 2)) + distance * np.random.uniform(-0.02, 0.02)
        distances.append(distance + noise)

    return np.array(distances)

def triangulate(
    stations: pd.DataFrame,
    distances: np.ndarray
) -> np.ndarray | None:
    """
    Решить задачу триангуляции по заданным координатам станции и измерениями до точки
    Возвращает предсказанные координаты [lat, lon]
    """
    if len(stations) < 3:
        if len(stations) == 1:
            print("Только одна вышка, беру её координаты")
            return np.array([stations.iloc[0]['lat'], stations.iloc[0]['lon']])
        elif len(stations) == 2:
            print("Две вышки, беру взвешенное среднее")
            weights = 1 / (distances + 1e-6)
            weights /= weights.sum()
            return np.average(stations[['lat', 'lon']].values, axis=0, weights=weights)
        print("Нет вышек, возвращаю None")
        return None

    if are_stations_collinear(stations):
        print("Станции на одной линии, не могу посчитать, возвращаю None")
        return None

    initial_guess = np.mean(stations[['lat', 'lon']].values, axis=0)
    x, y = initial_guess[0] * 111000, initial_guess[1] * 111000 * np.cos(np.radians(initial_guess[0]))

    stations_coords = stations[['lat', 'lon']].values
    learning_rate = 0.0001
    max_iterations = 500
    threshold = 1e-4
    prev_error = float('inf')

    for _ in range(max_iterations):
        grad_x, grad_y = 0, 0
        total_error = 0
        for (sx, sy), d in zip(stations_coords, distances):
            sx_m = sx * 111000
            sy_m = sy * 111000 * np.cos(np.radians(sx))
            calc_dist = np.sqrt((x - sx_m) ** 2 + (y - sy_m) ** 2)
            if calc_dist > 0:
                error = calc_dist - d
                total_error += error ** 2
                grad_x += error * (x - sx_m) / calc_dist
                grad_y += error * (y - sy_m) / calc_dist
        x_new = x - learning_rate * grad_x
        y_new = y - learning_rate * grad_y

        temp_error = 0
        for (sx, sy), d in zip(stations_coords, distances):
            sx_m = sx * 111000
            sy_m = sy * 111000 * np.cos(np.radians(sx))
            calc_dist = np.sqrt((x_new - sx_m) ** 2 + (y_new - sy_m) ** 2)
            if calc_dist > 0:
                temp_error += (calc_dist - d) ** 2
        if temp_error > prev_error:
            learning_rate *= 0.5
        else:
            x, y = x_new, y_new
            if abs(prev_error - total_error) < threshold:
                break
            prev_error = total_error

    pred_lat = x / 111000
    pred_lon = y / (111000 * np.cos(np.radians(pred_lat)))

    if abs(pred_lat) > 90 or abs(pred_lon) > 180:
        print("Координаты странные, возвращаю None")
        return None

    return np.array([pred_lat, pred_lon])


def plot_triangulation(
    true_lat: float,
    true_lon: float,
    pred_lat: float,
    pred_lon: float,
    nearby_stations: pd.DataFrame = None
) -> folium.Map:
    """
    Отрисовывает карту с истинным местоположением абонента, Предсказанным и ближайшими станциями
    """
    # Создаю карту, центрирую на настоящих координатах
    m = folium.Map(location=[true_lat, true_lon], zoom_start=12)

    folium.CircleMarker(
        location=[true_lat, true_lon],
        radius=3,
        color='green',
        fill=True,
        popup="Настоящее местоположение"
    ).add_to(m)

    if pred_lat is not None and pred_lon is not None:
        folium.CircleMarker(
            location=[pred_lat, pred_lon],
            radius=3,
            color='red',
            fill=True,
            popup="Предсказанное местоположение"
        ).add_to(m)
        folium.PolyLine(
            locations=[[true_lat, true_lon], [pred_lat, pred_lon]],
            color='purple',
            weight=2,
            popup="Ошибка"
        ).add_to(m)

    if nearby_stations is not None and not nearby_stations.empty:
        for _, station in nearby_stations.iterrows():
            folium.CircleMarker(
                location=[station['lat'], station['lon']],
                radius=5,
                color='blue',
                fill=True,
                popup=f"Вышка {station['stationid']}"
            ).add_to(m)

    return m

def main():
    stations_df = pd.read_csv("stations.csv")
    users_df = pd.read_csv("users_public.csv")

    R = 500  # Начальный радиус, проверяющий сказал увеличить

    errors = []
    total_users = len(users_df)
    valid_predictions = 0
    errors_by_stations = {}

    m = folium.Map(location=[users_df['lat'].mean(), users_df['lon'].mean()], zoom_start=12)

    legend_html = """
        <div style="position: fixed; bottom: 50px; left: 50px; background-color:white; padding: 10px; border: 1px solid grey;">
        <p><span style="color:green;">●</span> Настоящее местоположение</p>
        <p><span style="color:red;">●</span> Предсказанное местоположение</p>
        <p><span style="color:blue;">●</span> Вышка</p>
        <p><span style="color:purple;">—</span> Ошибка</p>
        </div>
        """
    m.get_root().html.add_child(folium.Element(legend_html))

    for idx, user in users_df.iterrows():
        nearby_stations = find_nearby_stations(user['lat'], user['lon'], stations_df, R)
        num_stations = len(nearby_stations)
        print(f"Пользователь {user['uuid']}: {num_stations} станций")

        distances = compute_distances(user['lat'], user['lon'], nearby_stations)

        pred_coords = triangulate(nearby_stations, distances)
        if pred_coords is None:
            print(f"Пользователь {user['uuid']}: нет предсказания")
            continue

        valid_predictions += 1
        pred_lat, pred_lon = pred_coords

        true_point = Point(user['lon'], user['lat'])
        pred_point = Point(pred_lon, pred_lat)
        error = haversine_distance(true_point, pred_point)
        errors.append(error)

        if num_stations not in errors_by_stations:
            errors_by_stations[num_stations] = []
        errors_by_stations[num_stations].append(error)

        user_map = plot_triangulation(user['lat'], user['lon'], pred_lat, pred_lon, nearby_stations)
        for feature in user_map._children.values():
            m.add_child(feature)

    m.save("../map.html")
    print("Сохранила карту в map.html")

    if errors:
        valid_errors = [e for e in errors if not np.isnan(e)]
        mean_error = np.mean(valid_errors)
        median_error = np.median(valid_errors)
        max_error = np.max(valid_errors)
        success_rate = np.mean([1 if e <= 50 else 0 for e in valid_errors]) * 100
        print(f"Всего пользователей: {total_users}")
        print(f"Валидных предсказаний: {valid_predictions}")
        print(f"Средняя ошибка: {mean_error:.2f} метров")
        print(f"Медианная ошибка: {median_error:.2f} метров")
        print(f"Максимальная ошибка: {max_error:.2f} метров")
        print(f"Процент успешных предсказаний (ошибка <= 50 м): {success_rate:.2f}%")

        print("\nОшибки по числу станций:")
        for num_stations, station_errors in sorted(errors_by_stations.items()):
            mean_station_error = np.mean(station_errors)
            print(f"{num_stations} станций: ошибка {mean_station_error:.2f} м, {len(station_errors)} случаев")
    else:
        print("Нет валидных предсказаний")

    plt.figure(figsize=(6, 4))
    plt.hist(valid_errors, bins=20, color='blue')
    plt.xlabel('Ошибка (метры)')
    plt.ylabel('Количество')
    plt.title('Гистограмма ошибок')
    plt.savefig("../errors.png")
    print("Сохранила гистограмму в errors.png")
    plt.close()

    plt.figure(figsize=(6, 4))
    station_counts = sorted(errors_by_stations.keys())
    mean_errors = [np.mean(errors_by_stations[n]) for n in station_counts]
    plt.bar(station_counts, mean_errors, color='blue')
    plt.xlabel('Число станций')
    plt.ylabel('Средняя ошибка (метры)')
    plt.title('Ошибка по числу станций')
    plt.savefig("../errors_by_stations.png")
    print("Сохранила график в errors_by_stations.png")
    plt.close()

if __name__ == '__main__':
    main()