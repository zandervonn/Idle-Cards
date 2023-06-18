using UnityEngine;
using UnityEngine.UI;  // Required for Button

public class InformationPage : MonoBehaviour
{
    public GameObject informationPage;
    private Button myButton;

    public bool isInformationVisible = false;

    private void Start()
    {
        informationPage.SetActive(false);
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(ToggleInformationPage);
    }

    public void ToggleInformationPage() 
    {
        isInformationVisible = !isInformationVisible;
        informationPage.SetActive(isInformationVisible);
    }
}