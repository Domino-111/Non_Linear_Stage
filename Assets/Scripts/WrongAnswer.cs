using UnityEngine;

public class WrongAnswer : Interactable
{
    public float speed;

    public override void Activate()
    {
        RigidbodyConstraints y = ~RigidbodyConstraints.FreezePositionY;

        foreach (var obj in GameObject.FindGameObjectsWithTag("Riddle"))
        {
            obj.GetComponent<BoxCollider>().enabled = false;
            obj.GetComponent<Rigidbody>().constraints = y & obj.GetComponent<Rigidbody>().constraints;
        }
        
        print("Interact works");
    }
}
