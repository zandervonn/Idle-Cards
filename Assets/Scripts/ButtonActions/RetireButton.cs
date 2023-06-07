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
        GameManager gameManager = GameManager.Instance;
        ModalDialog popup = ModalDialog.instance;
        float removeCost = gameManager.CurrentMultiplier;

        // Clear previous listeners
        popup.ClearListeners();

        if (gameManager.CalculateLevel() < 0.01)
        {
            popup.OpenDialog("Level must be over 0.01 to reset. \n Do you like this game?");
            popup.OnYes += () =>
            {
                gameManager.IncreaseBank(1);
                Debug.Log("retire");
            };
        }
        else
        {
            popup.OpenDialog("Are you sure you want to lock in your multiplier of " + removeCost + " and reset all other progress?");
            popup.OnYes += () =>
            {
                gameManager.retire();
            };
        }
    }
}