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

        float maxY = - ((childEle.rect.height - 2500) / 20);
        
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
        Debug.Log("min max y = " + minY + '-' + maxY);
    }
}

//public class TopAlignScroll : MonoBehaviour
//{
//    public RectTransform element; // Your public variable element (with RectTransform)
//    public ScrollRect scrollRect;

//    private float maxY; // Maximum Y position of the scroll

//    private void Start()
//    {
//        // Set the pivot to the top
//        scrollRect.content.pivot = new Vector2(scrollRect.content.pivot.x, 1f);

//        // Calculate the maximum Y position based on the height of your element
//        maxY = element.rect.height / scrollRect.content.rect.height;

//        // Set the initial scroll position
//        scrollRect.normalizedPosition = new Vector2(0.5f, Mathf.Clamp(1f, 0f, maxY));
//    }

//    private void Update()
//    {
//        // Check if the ScrollRect's y position is greater than maxY
//        if (scrollRect.normalizedPosition.y < maxY)
//        {
//            // Reset it to maxY
//            scrollRect.normalizedPosition = new Vector2(scrollRect.normalizedPosition.x, maxY);
//        }
//    }
//}