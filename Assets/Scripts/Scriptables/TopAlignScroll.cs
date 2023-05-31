using UnityEngine;
using UnityEngine.UI;

public class TopAlignScroll : MonoBehaviour
{
    public RectTransform childEle; // Your public variable element (with RectTransform)
    public RectTransform scrollEle;
    public ScrollRect scrollRect;
    private float minY = 0f;
    private float maxY = -20f;

    private void Update()
    {

        float maxY = - ((childEle.rect.height - 2000) / 22);
        
        // Check if the ScrollRect's y position is greater than 0.5f
        if (scrollRect.normalizedPosition.y > minY)
        {
            // Reset it to 0.5f
            scrollRect.normalizedPosition = new Vector2(scrollRect.normalizedPosition.x, minY);
        }

        // Check if the ScrollRect's y position is greater than 0.5f
        if (scrollRect.normalizedPosition.y < maxY)
        {
            // Reset it to 0.5f
            scrollRect.normalizedPosition = new Vector2(scrollRect.normalizedPosition.x, maxY);
        }
    }
}