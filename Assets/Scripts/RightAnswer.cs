using UnityEngine;

public class RightAnswer : Interactable
{
    public Transform door;
    private bool opened = false;
    public float speed;
    public Vector3 openDirection = new Vector3 (0f, 0f, 0f);


    void Update()
    {
        if (opened == true)
        {
            door.position = Vector3.MoveTowards(door.position, openDirection, speed * Time.deltaTime);
        }
    }

    public override void Activate()
    {
        opened = true;
    }
}
