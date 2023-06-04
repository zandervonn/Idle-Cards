using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class AutoResizeText : MonoBehaviour
{
    private Text textComponent;
    private RectTransform rectTransform;

    private void Awake()
    {
        textComponent = GetComponent<Text>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        FitToContainer();
    }

    public void FitToContainer()
    {
        // Ensure the canvas updates its layout before we measure the text size
        Canvas.ForceUpdateCanvases();

        float padding = 2f; // You can adjust this padding to create a bit of space around the text

        while (true)
        {
            // Determine the size of the text block
            var textSizeHorizontal = textComponent.cachedTextGenerator.GetPreferredWidth(textComponent.text, textComponent.GetGenerationSettings(rectTransform.rect.size));
            var textSizeVertical = textComponent.cachedTextGenerator.GetPreferredHeight(textComponent.text, textComponent.GetGenerationSettings(rectTransform.rect.size));

            // If the text width or height is larger than the container's, reduce the font size
            if (textSizeHorizontal + padding > rectTransform.rect.width || textSizeVertical + padding > rectTransform.rect.height)
            {
                textComponent.fontSize--;
            }
            else
            {
                break;
            }
        }
    }
}