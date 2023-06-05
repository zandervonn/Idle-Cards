using UnityEngine;
using UnityEngine.UI;  // Required for Button

public class InformationPage : MonoBehaviour
{
    public GameObject informationPage;
    private Button myButton;

    public bool isDeckVisible = false;

    private void Start()
    {
        informationPage.SetActive(false);
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(ToggleInformationPage);
    }

    public void ToggleInformationPage() 
    {
        isDeckVisible = !isDeckVisible;
        informationPage.SetActive(isDeckVisible);
    }
}