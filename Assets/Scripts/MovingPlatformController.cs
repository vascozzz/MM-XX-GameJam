using UnityEngine;
using System.Collections;

public class MovingPlatformController : MonoBehaviour
{
    [SerializeField] private Vector2 movement;

    void Update()
    {
        transform.position += (Vector3)movement * Time.deltaTime;
    }
}
