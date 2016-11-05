using UnityEngine;
using System.Collections;

public class CamToWorldUtility : MonoBehaviour {

    public class CameraBounds
    {
        public Vector3 up, down, left, right;
    }

    public static CameraBounds GetCameraBoundsInWorld(Camera cam)
    {
        CameraBounds output = new CameraBounds();

        float halfVertLen = cam.orthographicSize;
        float halfHorLen = halfVertLen * Screen.width / Screen.height;

        output.up = cam.transform.position + new Vector3(0f, halfVertLen, 0f);
        output.down = cam.transform.position + new Vector3(0f, -halfVertLen, 0f);
        output.left = cam.transform.position + new Vector3(-halfHorLen, 0f, 0f);
        output.right = cam.transform.position + new Vector3(halfHorLen, 0f, 0f);

        return output;
    }
    
}
