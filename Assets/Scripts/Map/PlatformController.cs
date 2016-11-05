using UnityEngine;
using System.Collections;

public class PlatformController : MonoBehaviour
{
    public Vector2 movement;
    public float outsideY;

    void Update()
    {
        if (transform.position.y < outsideY)
        {
            Destroy(gameObject);
        }

        transform.position += (Vector3)movement * Time.deltaTime;
    }
}
