using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ModalDialog : MonoBehaviour
{
    public GameObject dialogBox;
    public Text dialogText;
    public Button yesButton;
    public Button noButton;
    public UnityEvent OnYes;
    public UnityEvent OnNo;
    public bool result;
    public static ModalDialog instance;

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
            OnYes.Invoke();
            dialogBox.SetActive(false);
        });

        noButton.onClick.AddListener(() =>
        {
            OnNo.Invoke();
            dialogBox.SetActive(false);
        });
    }
}