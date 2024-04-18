using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    // Instancia estática del ScoreController
    private static ScoreController _instance;

    // Propiedad para acceder a la instancia pública
    public static ScoreController Instance
    {
        get
        {
            // Si la instancia no existe, busca un objeto ScoreController en la escena
            if (_instance == null)
            {
                _instance = FindObjectOfType<ScoreController>();

                // Si no se encontró, crea un nuevo objeto ScoreController en la escena
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(ScoreController).Name);
                    _instance = singletonObject.AddComponent<ScoreController>();
                }
            }
            return _instance;
        }
    }

    // Aquí van las variables y métodos que quieras acceder de forma estática
    [SerializeField] private TextMeshProUGUI Score;
    [SerializeField] private TextMeshProUGUI multiplierText;
    [SerializeField] private Slider multiplierSlider;
    private int pointScore = 0;
    private int multiplier = 1;
    private float target = 0;
    [SerializeField, Range(0f, 100f, order = 1)] private float currentValue = 0f;
    [SerializeField, Range(0f, 50f, order = 3)] private float speedDecrease = 1f;
    private bool isUpdatingMultiplier = false;

    private void Start() {
        multiplierText.text = "x1";
        multiplierSlider.value = 0;
    }
    private void Update() {        
        if(!isUpdatingMultiplier){
            if(currentValue > 0f)
            currentValue -= speedDecrease * Time.deltaTime;
        }
        if(currentValue >= 100f){
            isUpdatingMultiplier = true;
            currentValue = Mathf.Lerp(currentValue, 0, 5f * Time.deltaTime);
            if(currentValue <= 0.1f && isUpdatingMultiplier){
                AddMultiplier();
                isUpdatingMultiplier = false;
            }
        }
        multiplierSlider.value = currentValue;
    }

    public void IncreaseMultiplier(float target){
        StartCoroutine(UpdateSlider(target));
    }

    // Corrutina para actualizar gradualmente el valor del slider
    private IEnumerator UpdateSlider(float targetValue)
    {
        isUpdatingMultiplier = true;

        float timer = 0f;        
        float duration = 1f;        
        target += targetValue;
        float initialValue = currentValue; // Guardar el valor inicial
        float finalValue = initialValue + targetValue; // Calcular el valor final

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / duration);
            // Usar Lerp para interpolar entre el valor inicial y final
            currentValue = Mathf.Lerp(initialValue, finalValue, progress);
            yield return null;
        }        

        isUpdatingMultiplier = false;
    }

    public int GetSliderValue(){
        return (int)MathF.Round(multiplierSlider.value);
    }

    public void AddMultiplier(){
        multiplier += 1;
        multiplierText.text = "x"+multiplier.ToString();
    }
    public void AddScore(int points)
    {
        pointScore += points * multiplier;
        Score.text = pointScore.ToString("000000000000");
    }

    private void Awake()
    {
        // Si la instancia ya existe y no es esta misma, destruye este objeto
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        // Establece esta instancia como la única instancia válida
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
