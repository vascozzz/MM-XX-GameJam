using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public static GameManager Instance = null;

    [Header("General")]
    [SerializeField] private Camera cam;
    [SerializeField] private float initialVelocity = 0.5f;
    [SerializeField] private float velocityIncrement = 0.05f;
    [SerializeField] private float velocityDecrementDeathPercent = 0.8f;

    [Header("Platforms")]
    [SerializeField] private GameObject[] platforms;
    [SerializeField] private GameObject platformsContainer;

    [Header("DeathRays")]
    [SerializeField] private GameObject[] deathRays;
    [SerializeField] private float deathRaysProbability = 0.05f;
    [SerializeField] private float deathRaysOffset = 1f;
    [SerializeField] private GameObject deathRaysContainer;

    [Header("Players")]
    [SerializeField] private PlayerController p1;
    [SerializeField] private PlayerController p2;
    [SerializeField] private float playerDeathDuration = 5f;
    [SerializeField] private GameObject playerDeathEffect;

    private PlatformGenerator p1PlatformsGen;
    private PlatformGenerator p2PlatformsGen;

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
        p1PlatformsGen = gameObject.AddComponent<PlatformGenerator>();
        p1PlatformsGen.Initialize(cam, platforms, platformsContainer, deathRays, deathRaysContainer, p1LeftCorner, p1RightCorner, startY, deathRaysProbability, deathRaysOffset, initialVelocity, velocityIncrement);
        p1PlatformsGen.Generate();

        // right player
        float p2LeftCorner = camBounds.up.x;
        float p2RightCorner = camBounds.right.x;
        p2PlatformsGen = gameObject.AddComponent<PlatformGenerator>();
        p2PlatformsGen.Initialize(cam, platforms, platformsContainer, deathRays, deathRaysContainer, p2LeftCorner, p2RightCorner, startY, deathRaysProbability, deathRaysOffset, initialVelocity, velocityIncrement);
        p2PlatformsGen.Generate();
    }

    void Update()
    {
        p1PlatformsGen.Generate();
        p2PlatformsGen.Generate();
    }

    public void OnPlayerDeath(PlayerController caller)
    {
        if (p1.playerDead && p2.playerDead)
        {
            Debug.Log("game over, should exit");
        }

        // death effect
        Instantiate(playerDeathEffect, caller.transform.position, Quaternion.identity);

        if (caller == p1)
        {
            p2PlatformsGen.DecrementVelocity(velocityDecrementDeathPercent);
        }
        else
        {
            p1PlatformsGen.DecrementVelocity(velocityDecrementDeathPercent);
        }

        RespawnPlayer(caller);
    }

    private IEnumerable RespawnPlayer(PlayerController player)
    {
        yield return new WaitForSeconds(playerDeathDuration);

        player.Reset();
    }
}
