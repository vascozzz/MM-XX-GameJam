using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public static GameManager Instance = null;

    [SerializeField] private Camera cam;
    [SerializeField] private GameObject[] platforms;
    [SerializeField] private GameObject platformsContainer;
    [SerializeField] private GameObject[] deathRays;
    [SerializeField] private GameObject deathRaysContainer;
    [SerializeField] private float startVelocity = 1f;

    private PlatformGenerator genLeft;
    private PlatformGenerator genRight;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        CamToWorldUtility.CameraBounds camBounds = CamToWorldUtility.GetCameraBoundsInWorld(cam);
        float startY = camBounds.down.y;

        // left player
        float p1LeftCorner = camBounds.left.x;
        float p1RightCorner = camBounds.up.x;
        genLeft = gameObject.AddComponent<PlatformGenerator>();
        genLeft.Initialize(cam, platforms, platformsContainer, deathRays, deathRaysContainer, p1LeftCorner, p1RightCorner, startY);
        genLeft.SetVelocity(startVelocity);
        genLeft.Generate();

        // right player
        float p2LeftCorner = camBounds.up.x;
        float p2RightCorner = camBounds.right.x;
        genRight = gameObject.AddComponent<PlatformGenerator>();
        genRight.Initialize(cam, platforms, platformsContainer, deathRays, deathRaysContainer, p2LeftCorner, p2RightCorner, startY);
        genRight.SetVelocity(startVelocity + 1f);
        genRight.Generate();
    }

    void Update()
    {
        genLeft.Generate();
        genRight.Generate();
    }

   
}
