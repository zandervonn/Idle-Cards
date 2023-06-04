using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextEffect : MonoBehaviour
{
    [SerializeField] private float floatSpeed = 0.1f;
    //[SerializeField] private float fadeSpeed = 1f;
    [SerializeField] private float duration = 2f;
    //[SerializeField] private float wobbleAmplitude = 0.1f;
    //[SerializeField] private float wobbleFrequency = 1f;

    private Text originalText;
    private string lastTextValue;

    private float updateInterval = 0.1f;
    private float updateTimer = 0f;

    private void Awake()
    {
        originalText = GetComponent<Text>();
        lastTextValue = originalText.text;
    }

    private void Update()
    {
        updateTimer += Time.deltaTime;
        if (updateTimer >= updateInterval)
        {
            updateTimer = 0f;
            if (originalText.text != lastTextValue)
            {
                CreateFloatingText();
                lastTextValue = originalText.text;
            }
        }
    }

    private void CreateFloatingText()
    {
        GameObject floatingTextObj = new GameObject("FloatingText");
        floatingTextObj.transform.SetParent(transform.parent, false);
        floatingTextObj.transform.position = transform.position;

        Text floatingText = floatingTextObj.AddComponent<Text>();
        floatingText.font = originalText.font;
        floatingText.fontSize = originalText.fontSize;
        floatingText.alignment = originalText.alignment;

        // Add ContentSizeFitter and configure it
        ContentSizeFitter contentSizeFitter = floatingTextObj.AddComponent<ContentSizeFitter>();
        contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;


        string oldValueStr = lastTextValue.TrimStart('$');
        string newValueStr = originalText.text.TrimStart('$');

        int oldValue, newValue;
        if (int.TryParse(oldValueStr, out oldValue) && int.TryParse(newValueStr, out newValue))
        {
            int difference = newValue - oldValue;
            bool hasDollarSign = lastTextValue.StartsWith("$") || originalText.text.StartsWith("$");
            string differenceStr = difference.ToString("+#;-#;0");
            if (hasDollarSign) differenceStr = "$" + differenceStr;

            floatingText.text = differenceStr;
            floatingText.color = difference >= 0 ? Color.green : Color.red;
        }
        else
        {
            floatingText.text = originalText.text;
            floatingText.color = originalText.color;
        }

        StartCoroutine(FloatAndFade(floatingTextObj));
    }

    private IEnumerator FloatAndFade(GameObject floatingTextObj)
    {
        Text floatingText = floatingTextObj.GetComponent<Text>();
        float elapsedTime = 0f;

        Vector3 startPosition = floatingTextObj.transform.position;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Float upwards
            floatingTextObj.transform.position = new Vector3(
                startPosition.x,
                startPosition.y += floatSpeed,
                startPosition.z
            );

            // Fade out
            Color color = floatingText.color;
            color.a = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            floatingText.color = color;

            yield return null;
        }

        Destroy(floatingTextObj);
    }
}