using UnityEngine;

public class ResetIdleEarnings : MonoBehaviour
{
    public bool resetIdleEarnings;

    private void Update()
    {
        if (resetIdleEarnings)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("PlayerPrefs cleared.");
        }
    }
}