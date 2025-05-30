using UnityEngine;

public class WrongAnswer : Interactable
{
    public float speed;

    void Start()
    {

    }

    void Update()
    {

    }

    public override void Activate()
    {
        RigidbodyConstraints y = ~RigidbodyConstraints.FreezePositionY;

        foreach (var obj in GameObject.FindGameObjectsWithTag("Riddle"))
        {
            obj.GetComponent<Rigidbody>().constraints = y & obj.GetComponent<Rigidbody>().constraints;
        }
        
        print("Interact works");
    }
}
