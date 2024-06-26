using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public Slider lowerSlider;
    public Slider upperSlider;
    public float costMultiplier;

    private void Start()
    {
        // Ensure lowerSlider value is always less than upperSlider value
        lowerSlider.onValueChanged.AddListener((value) => {
            if (value > upperSlider.value)
            {
                lowerSlider.value = upperSlider.value;
            }
            UpdateCostMultiplier();
        });

        // Ensure upperSlider value is always more than lowerSlider value
        upperSlider.onValueChanged.AddListener((value) => {
            if (value < lowerSlider.value)
            {
                upperSlider.value = lowerSlider.value;
            }
            UpdateCostMultiplier();
        });

        UpdateCostMultiplier();
    }

    private void UpdateCostMultiplier()
    {
        costMultiplier = CalculateCostMultiplier(lowerSlider.value, upperSlider.value);
    }

    private float CalculateCostMultiplier(float x, float y)
    {
        return ((((3 * x + y) * (3 * x + y)) / 500) + 10) / 10; // 100 > 1000 > 500 //2 > 3
    }

    public float GetLowerBound()
    {
        return lowerSlider.value;
    }

    public float GetUpperBound()
    {
        return upperSlider.value;
    }
}