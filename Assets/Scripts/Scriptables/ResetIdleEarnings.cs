using UnityEngine;

public class ResetIdleEarnings : MonoBehaviour
{
    public bool resetIdleEarnings;

    private void Update()
    {
        if (resetIdleEarnings)
        {
            PlayerPrefs.DeleteKey("LastQuitTime");
            resetIdleEarnings = false;
        }
    }
}