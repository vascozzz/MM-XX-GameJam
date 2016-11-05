using UnityEngine;
using System.Collections;

public class WallPlacer : MonoBehaviour {

    [SerializeField] private Camera cam;
    [SerializeField] private bool left;

	void Start ()
    {
        Vector3 newPosition;
        float z = transform.position.z;

        if(left)
        {
            newPosition = CamToWorldUtility.GetCameraBoundsInWorld(cam).left;
        }
        else
        {
            newPosition = CamToWorldUtility.GetCameraBoundsInWorld(cam).right;
        }

        newPosition.z = z;
        transform.position = newPosition;
    }
}
