using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using UnityEngine.UI;

public class WeatherSystem : MonoBehaviour
{
    public string location;
    public string weatherDescription;
    public float temperatureCelsius;

    private string apiKey = "XXXXX"; // 必ずここに自分のAPIキーを設定してください
    
    private string lat = "XXXXXXXX"; // 緯度
    private string lon = "YYYYYYYY";// 経度

    void Start()
    {
        StartCoroutine(GetWeather());
    }

    IEnumerator GetWeather()
    {
        string url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={apiKey}&units=metric&lang=ja";
        // units=metricで摂氏

        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            // APIレスポンスの内容を確認する
            Debug.Log("API Response: " + request.downloadHandler.text);

            try
            {
                WeatherInfo weatherInfo = JsonUtility.FromJson<WeatherInfo>(request.downloadHandler.text);

                if (weatherInfo != null && weatherInfo.weather != null && weatherInfo.weather.Length > 0)
                {
                    // データをクラスの変数に格納
                    location = weatherInfo.name;
                    weatherDescription = weatherInfo.weather[0].main;
                    //weatherDescription = "Sunny";
                    //weatherDescription = "Rain";
                    temperatureCelsius = weatherInfo.main.temp;
                    //temperatureCelsius = 35;
                }
                else
                {
                    Debug.LogError("Parsed weatherInfo is null or does not contain expected data.");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Exception during JSON parsing: " + ex.Message);
            }
        }
    }

    [System.Serializable]
    public class WeatherInfo
    {
        public Main main;
        public Weather[] weather;
        public string name;

        [System.Serializable]
        public class Main
        {
            public float temp;
        }

        [System.Serializable]
        public class Weather
        {
            public string main;
            public string description;
        }
    }

    //public string url = "https://api.openweathermap.org/data/2.5/forecast?lat=33.973128831319976&lon=133.3022135039521&appid=b8240e602cf22acf45ac121c4ea564e5&lang=ja";


}
