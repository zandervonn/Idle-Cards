using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(ContentSizeFitter))]
public class MinHeightSetter : MonoBehaviour
{
    private RectTransform rectTransform;
    private ContentSizeFitter contentSizeFitter;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        contentSizeFitter = GetComponent<ContentSizeFitter>();
    }

    private void Update()
    {
        // Set the minimum height to the height of the screen
        float minHeight = Screen.height;

        // The preferred height is the height the ContentSizeFitter would normally set
        float preferredHeight = LayoutUtility.GetPreferredHeight(rectTransform);

        // Set the height to the greater of the minimum height and the preferred height
        float height = Mathf.Max(minHeight, preferredHeight);

        // Apply the height
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

        // Disable the ContentSizeFitter now that we've manually set the height
        contentSizeFitter.enabled = false;
    }
}