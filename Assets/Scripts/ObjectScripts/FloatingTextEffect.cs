using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextEffect : MonoBehaviour
{
    [SerializeField] private float floatSpeed = 1f;
    [SerializeField] private float fadeSpeed = 1f;
    [SerializeField] private float duration = 2f;
    [SerializeField] private float wobbleAmplitude = 0.1f;
    [SerializeField] private float wobbleFrequency = 1f;

    private Text originalText;
    private string lastTextValue;

    private void Awake()
    {
        originalText = GetComponent<Text>();
        if (originalText == null)
        {
            Debug.LogError("Text component not found on the same GameObject as FloatingTextEffect.");
        }
        lastTextValue = originalText.text;
    }

    private void Update()
    {
        if (originalText.text != lastTextValue)
        {
            CreateFloatingText();
            lastTextValue = originalText.text;
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
                startPosition.x + wobbleAmplitude * Mathf.Sin(wobbleFrequency * elapsedTime),
                startPosition.y + floatSpeed * elapsedTime,
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