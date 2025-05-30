using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public void Click()
    {
        Activate();
    }

    public abstract void Activate();
}
