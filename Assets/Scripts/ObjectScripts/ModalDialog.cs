using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class ModalDialog : MonoBehaviour
{
    public GameObject dialogBox;
    public Text dialogText;
    public GameObject yesButtonObj;
    public GameObject noButtonObj;
    public GameObject OKButtonObj;
    public Button yesButton;
    public Button noButton;
    public Button OKButton;
    public bool result;
    public static ModalDialog instance;

    // Define delegate event for Yes and No
    public delegate void DialogResponse();
    public event DialogResponse OnYes;
    public event DialogResponse OnNo;
    public event DialogResponse OnOK;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Modal Dialog found!");
            return;
        }

        instance = this;

        dialogBox.SetActive(false);
        OKButtonObj.SetActive(false);
        yesButtonObj.SetActive(false);
        noButtonObj.SetActive(false);
    }

    public void OpenYesNoDialog(string text)
    {
        dialogText.text = text;
        dialogBox.SetActive(true);
        yesButtonObj.SetActive(true);
        noButtonObj.SetActive(true);
        OKButtonObj.SetActive(false);

        // Clear previous listeners
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();
        OKButton.onClick.RemoveAllListeners();

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

    public void OpenOKDialog(string text)
    {
        dialogText.text = text;
        dialogBox.SetActive(true);
        yesButtonObj.SetActive(false);
        noButtonObj.SetActive(false);
        OKButtonObj.SetActive(true);

        // Clear previous listeners
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();
        OKButton.onClick.RemoveAllListeners();

        OKButton.onClick.AddListener(() =>
        {
            OnOK?.Invoke();
            dialogBox.SetActive(false);
        });
    }

    public void ClearListeners()
    {
        OnYes = null;
        OnNo = null;
        OnOK = null;
    }
}