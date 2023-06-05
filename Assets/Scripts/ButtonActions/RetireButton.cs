using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class RetireButton : MonoBehaviour
{
    private GameManager gameManager;
    private Button myButton;

    private void Start()
    {
        gameManager = GameManager.Instance;
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(retire);
    }


    public void retire()
    {
        Debug.Log("retire");
        GameManager gameManager = GameManager.Instance;
        ModalDialog popup = ModalDialog.instance;
        float removeCost = gameManager.CurrentMultiplier;

        if (gameManager.CalculateLevel() < 0.1)
        {
            popup.OpenDialog("Level must be over 0.1 to reset. \n Do you like dogs?");
            popup.OnYes.AddListener(() =>
            {
                gameManager.IncreaseBank(1);
            });
        } else
        {
            popup.OpenDialog("Are you sure you want to lock in your multiplier of " + removeCost + " and reset all other progress?");

            popup.OnYes.AddListener(() =>
            {
                gameManager.retire();
            });
        }
    }
}