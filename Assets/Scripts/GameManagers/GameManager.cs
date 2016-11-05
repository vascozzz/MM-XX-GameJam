using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public static GameManager Instance = null;

    [Header("General")]
    [SerializeField] private Camera cam;
    [SerializeField] private float initialVelocity = 0.5f;
    [SerializeField] private float velocityIncrement = 0.05f;

    [Header("Platforms")]
    [SerializeField] private GameObject[] platforms;
    [SerializeField] private GameObject platformsContainer;

    [Header("DeathRays")]
    [SerializeField] private GameObject[] deathRays;
    [SerializeField] private float deathRaysProbability = 0.05f;
    [SerializeField] private float deathRaysOffset = 1f;
    [SerializeField] private GameObject deathRaysContainer;

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
        GeneratePlatforms();
    }

    void GeneratePlatforms()
    {
        CamToWorldUtility.CameraBounds camBounds = CamToWorldUtility.GetCameraBoundsInWorld(cam);
        float startY = camBounds.down.y;

        // left player
        float p1LeftCorner = camBounds.left.x;
        float p1RightCorner = camBounds.up.x;
        genLeft = gameObject.AddComponent<PlatformGenerator>();
        genLeft.Initialize(cam, platforms, platformsContainer, deathRays, deathRaysContainer, p1LeftCorner, p1RightCorner, startY, deathRaysProbability, deathRaysOffset, initialVelocity, velocityIncrement);
        genLeft.Generate();

        // right player
        float p2LeftCorner = camBounds.up.x;
        float p2RightCorner = camBounds.right.x;
        genRight = gameObject.AddComponent<PlatformGenerator>();
        genRight.Initialize(cam, platforms, platformsContainer, deathRays, deathRaysContainer, p2LeftCorner, p2RightCorner, startY, deathRaysProbability, deathRaysOffset, initialVelocity, velocityIncrement);
        genRight.Generate();
    }

    void Update()
    {
        genLeft.Generate();
        genRight.Generate();
    }
}
