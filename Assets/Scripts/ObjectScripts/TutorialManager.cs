using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    // You can manually set these in the Unity editor by dragging each of your tutorial panel objects into this array.
    public GameObject[] tutorialPanels;

    public bool tutorial;

    public Button tutorialbutton;

    // This keeps track of the current panel index. When it equals the length of the tutorialPanels array, we've reached the end.
    private int currentPanelIndex = 0;

    private void Start()
    {
        tutorialbutton.onClick.AddListener(runTutorial);
    }

    public void runTutorial()
    {
        InformationPage informationPage = FindObjectOfType<InformationPage>();
        informationPage.CloseInformationPage();

        // At the start of the tutorial, make sure only the first panel is active.
        for (int i = 0; i < tutorialPanels.Length; i++)
        {
            tutorialPanels[i].SetActive(false);
        }
        tutorialPanels[0].SetActive(true);

        tutorial = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (tutorial && Input.GetMouseButtonDown(0))
        {
            // A mouse button (or screen tap) was pressed, so we go to the next panel.
            GoToNextPanel();
        }
    }

    private void GoToNextPanel()
    {
        // Turn off the current panel
        tutorialPanels[currentPanelIndex].SetActive(false);

        // Increment the panel index.
        currentPanelIndex++;

        // If we've reached the end of the tutorial, stop here.
        if (currentPanelIndex >= tutorialPanels.Length)
        {
            tutorial = false;
            //tutorial.SetActive(false);
            this.gameObject.SetActive(false);
            return;
        }

        // Otherwise, turn on the next panel.
        tutorialPanels[currentPanelIndex].SetActive(true);
    }
}