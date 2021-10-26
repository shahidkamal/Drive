
using UnityEngine;

public class CarController : MonoBehaviour
{
    public bool aiControlled;
    public float speed;
    public float steeringSpeet;

    private void Update()
    {
        var pos = transform.position;

        if (Input.GetKey(KeyCode.A))
        {
            pos.x = pos.x - speed / 100f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            pos.x = pos.x + speed /100f;
        }

        transform.position = pos;
    }
}
