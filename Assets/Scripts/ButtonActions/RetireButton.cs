using UnityEngine;
using UnityEngine.UI;

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
            popup.OpenOKDialog("Level must be over 0.01 to reset");
            popup.OnYes += () =>
            {
                gameManager.IncreaseBank(1);
                Debug.Log("retire");
            };
        }
        else
        {
            popup.OpenYesNoDialog("Are you sure you want to lock in your multiplier of " + removeCost + " and reset all other progress?");
            popup.OnYes += () =>
            {
                gameManager.retire();
            };
        }
    }
}