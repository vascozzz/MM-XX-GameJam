using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public static GameManager Instance = null;

    [Header("General")]
    [SerializeField] private Camera cam;
    [SerializeField] private float startVelocity = 1f;

    [Header("Platforms")]
    [SerializeField] private GameObject[] platforms;
    [SerializeField] private GameObject platformsContainer;

    [Header("DeathRays")]
    [SerializeField] private GameObject[] deathRays;
    [SerializeField] private GameObject deathRaysContainer;

    [Header("Players")]
    [SerializeField] private GameObject[] players;
    [SerializeField] private Vector2[] playerSpawns;

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
        SpawnPlayers();
    }

    void SpawnPlayers()
    {
        for (int i = 0; i < players.Length; i++)
        {
            GameObject playerObj = Instantiate(players[i], playerSpawns[i], Quaternion.identity) as GameObject;

            PlayerInput playerInput = playerObj.GetComponentInChildren<PlayerInput>();
            playerInput.Initialize(i);
        }
    }

    void GeneratePlatforms()
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
