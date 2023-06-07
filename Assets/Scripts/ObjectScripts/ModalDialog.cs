using UnityEngine;
using UnityEngine.UI;

public class ModalDialog : MonoBehaviour
{
    public GameObject dialogBox;
    public Text dialogText;
    public Button yesButton;
    public Button noButton;
    public bool result;
    public static ModalDialog instance;

    // Define delegate event for Yes and No
    public delegate void DialogResponse();
    public event DialogResponse OnYes;
    public event DialogResponse OnNo;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Modal Dialog found!");
            return;
        }

        instance = this;
    }

    public void OpenDialog(string text)
    {
        dialogText.text = text;
        dialogBox.SetActive(true);

        // Clear previous listeners
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();

        yesButton.onClick.AddListener(() =>
        {
            OnYes?.Invoke();
            dialogBox.SetActive(false);
        });

        noButton.onClick.AddListener(() =>
        {
            OnNo?.Invoke();
            dialogBox.SetActive(false);
        });
    }

    public void ClearListeners()
    {
        OnYes = null;
        OnNo = null;
    }
}