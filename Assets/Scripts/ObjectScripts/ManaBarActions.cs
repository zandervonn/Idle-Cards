//8
using UnityEngine;
using UnityEngine.UI;

public class ManaBarActions : MonoBehaviour
{
    private GameManager gameManager;
    private Slider slider;
    public Text manaValueText;
    private float pauseEndTime = -1f;
    public Image manaBarImage;
    public Color defaultColor;
    public Color pauseColor;

    private void Start()
    {
        gameManager = GameManager.Instance;

        slider = GetComponent<Slider>();
        slider.maxValue = gameManager.maxMana;
        slider.value = gameManager.mana;
}

    private void Update()
    {
        if (Time.time > pauseEndTime)
        {
            float currentManaLossRate = gameManager.ManaLossRate * Time.deltaTime;

            gameManager.DecreaseMana(currentManaLossRate);
            slider.value = gameManager.mana;
            manaValueText.text = gameManager.mana.ToString("F2");

            if (gameManager.mana <= 0)
            {
                gameManager.OnManaDepleted();
            }

            // Restore the color and outline
            manaBarImage.color = defaultColor;
        }
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
        if(slider.value > slider.maxValue)
        {
            slider.value = slider.maxValue;
        }
    }

    public float CalculateTimeToDepleteMana()
    {
        gameManager = GameManager.Instance;

        float t = gameManager.maxMana / gameManager.ManaLossRate;
        return t;
    }

    public void PauseManaDecrease(float time)
    {
        pauseEndTime = Time.time + time;

        // Change the color and disable the outline
        manaBarImage.color = pauseColor;
    }


}