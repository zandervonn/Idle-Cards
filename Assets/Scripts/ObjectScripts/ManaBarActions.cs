//8
using UnityEngine;
using UnityEngine.UI;

public class ManaBarActions : MonoBehaviour
{
     //should be game manager value
    public float manaLossRate = 0.05f;
    private float manaAcceleration = 3f;
    private float elapsedTimeSinceRoundStart = 0f;

    private GameManager gameManager;
    private Slider slider;
    public Text manaValueText;

    private void Start()
    {
        gameManager = GameManager.Instance;

        slider = GetComponent<Slider>();
        slider.maxValue = gameManager.maxMana;
        slider.value = gameManager.mana;

        gameManager.maxMana = 100f;
        manaLossRate = 0.05f;
        manaAcceleration = 3f;
}

    private void Update()
    {
        elapsedTimeSinceRoundStart += Time.deltaTime;
        float currentManaLossRate = manaLossRate * manaAcceleration * elapsedTimeSinceRoundStart;

        gameManager.DecreaseMana(currentManaLossRate * Time.deltaTime);
        slider.value = gameManager.mana;
        manaValueText.text = gameManager.mana.ToString("F2");

        if (gameManager.mana <= 0)
        {
            gameManager.OnManaDepleted();
        }
    }

    public void ResetElapsedTimeSinceRoundStart()
    {
        elapsedTimeSinceRoundStart = 0f;
    }

    public void SetSliderValue(float value)
    {
        gameManager.mana = (int)value;
        slider.value = gameManager.mana;
    }

    public void UpdateSlider()
    {
        slider.value = gameManager.mana;
    }

    public void UpdateSliderMaxValue()
    {
        slider.maxValue = gameManager.maxMana;
    }

    public float CalculateTimeToDepleteMana()
    {
        float a = 0.5f * manaAcceleration;
        float b = manaLossRate;
        float c = -gameManager.maxMana;

        if (a != 0)
        {
            float t = (Mathf.Sqrt(2 * a * c + b * b) - b) / a;
            return t;
        }
        else if (b != 0)
        {
            float t = c / b;
            return t;
        }
        else
        {
            return 0;
        }
    }


}