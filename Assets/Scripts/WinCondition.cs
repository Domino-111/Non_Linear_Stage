using UnityEngine;

public class WinCondition : Interactable
{
    public GameObject controls, winScreen;

    public override void Activate()
    {
        controls.SetActive(false);
        winScreen.SetActive(true);

        Time.timeScale = 0f;
    }
}
