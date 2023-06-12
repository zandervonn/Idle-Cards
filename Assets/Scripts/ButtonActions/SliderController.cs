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
        return (((2 * x + y) * (2 * x + y)) / 100) + 10; //(((2x + y) ^2)/100) + 10
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