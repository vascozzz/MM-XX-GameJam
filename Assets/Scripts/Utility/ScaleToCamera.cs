using UnityEngine;
using System.Collections;

public class CamWorldBounds : MonoBehaviour
{
    [SerializeField] private Camera cam;
    
	void Start ()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        float backgroundWidth = sr.bounds.extents.x * 2f;
        float cameraWidth = cam.orthographicSize * 2.0f * Screen.width / Screen.height;
        float rate = cameraWidth / backgroundWidth;

        transform.localScale *= rate;
    }
}
