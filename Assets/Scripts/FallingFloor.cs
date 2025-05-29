using UnityEngine;

public class FallingFloor : MonoBehaviour
{
    public Vector3 fallingDirection = new Vector3(0f, 0f, 0f);

    private bool stepped = false;

    public float speed;

    void Start()
    {
        
    }

    void Update()
    {
        if (stepped == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, fallingDirection, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            stepped = true;
        }
    }
}
