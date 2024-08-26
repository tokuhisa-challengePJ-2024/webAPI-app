using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeatherMotion : MonoBehaviour
{
    private Animator animator;
    private WeatherSystem weatherSystem; // WeatherSystemスクリプトを参照する変数
    private bool dataLogged = false; // データがログされたかどうかを追跡するフラグ

    private GameObject ubmllera; // "ubmllera"オブジェクトを参照する変数
    private GameObject RainParticle; // パーティクルオブジェクトを参照する変数
    // UI Text指定用
    public Text cityNameText;
    public Text tempratureText; 

    [SerializeField] Material SunnySkyMaterial;
    [SerializeField] Material CloudSkyMaterial;

    void Start()
    {
        animator = GetComponent<Animator>();

        // "WeatherSystem"という名前のオブジェクトを探す
        GameObject obj = GameObject.Find("WeatherSystem");

        if (obj != null)
        {
            // WeatherSystemスクリプトを取得
            weatherSystem = obj.GetComponent<WeatherSystem>();
        }
        else
        {
            Debug.LogError("GameObject with the name 'WeatherSystem' is not found.");
        }

        // "ubmllera"というタグのオブジェクトを探す
        ubmllera = GameObject.FindGameObjectWithTag("ubmllera");
         

        if (ubmllera == null)
        {
            Debug.LogError("GameObject with the tag 'ubmllera' is not found. Make sure you have assigned the correct tag in the Inspector.");
        }
        else{
            ubmllera.SetActive(false); // "ubmllera"オブジェクトを非アクティブにする
        }
        // "ubmllera"というタグのオブジェクトを探す
        RainParticle = GameObject.FindGameObjectWithTag("Rain");
        if (RainParticle == null)
        {
            Debug.LogError("GameObject with the tag 'RainParticle' is not found. Make sure you have assigned the correct tag in the Inspector.");
        }
        else{
            RainParticle.SetActive(false); // "RainParticle"オブジェクトを非アクティブにする
        }
    }

    void Update()
    {
        if (weatherSystem != null && !dataLogged)
        {
            if (!string.IsNullOrEmpty(weatherSystem.location) && !string.IsNullOrEmpty(weatherSystem.weatherDescription))
            {
                Debug.Log("Location: " + weatherSystem.location);
                cityNameText.text = weatherSystem.location;
                Debug.Log("Weather: " + weatherSystem.weatherDescription);
                Debug.Log("Temperature: " + weatherSystem.temperatureCelsius + "°C");
                tempratureText.text = weatherSystem.temperatureCelsius + "°C";
                dataLogged = true; // データが表示されたことを記録
            }
        }

        if (weatherSystem != null && weatherSystem.weatherDescription.ToLower() == "rain")
        {
            animator.SetBool("is_rain", true);
            
            RenderSettings.skybox = CloudSkyMaterial;
            if (ubmllera != null)
            {
                ubmllera.SetActive(true); // "ubmllera"オブジェクトをアクティブにする
            }
            if (RainParticle != null)
            {
                RainParticle.SetActive(true); // "RainParticle"オブジェクトをアクティブにする
            }
            
        }
        else if(weatherSystem != null && weatherSystem.weatherDescription.ToLower() != "rain" && weatherSystem.temperatureCelsius >= 35)
        {
            animator.SetBool("is_hot", true);
            Debug.Log("35度以上の猛暑");
            animator.SetBool("is_rain", false);

            RenderSettings.skybox = SunnySkyMaterial;
        }
    }
}
