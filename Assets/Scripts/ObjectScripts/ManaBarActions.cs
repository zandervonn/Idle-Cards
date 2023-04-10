//8
using UnityEngine;
using UnityEngine.UI;

public class ManaBarActions : MonoBehaviour
{
    private float maxMana = 100f; //should be game manager value
    private float manaLossRate = 0.05f;
    private float manaAcceleration = 3f;
    private float elapsedTimeSinceRoundStart = 0f;

    private GameManager gameManager;
    private Slider slider;

    private void Start()
    {
        gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene.");
        }

        slider = GetComponent<Slider>();
        slider.maxValue = maxMana;
        slider.value = gameManager.mana;

        maxMana = 100f;
        manaLossRate = 0.05f;
        manaAcceleration = 3f;
}

    private void Update()
    {
        elapsedTimeSinceRoundStart += Time.deltaTime;
        float currentManaLossRate = manaLossRate * manaAcceleration * elapsedTimeSinceRoundStart;

        gameManager.DecreaseMana(currentManaLossRate * Time.deltaTime);
        slider.value = gameManager.mana;

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


}