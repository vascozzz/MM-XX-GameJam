using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [SerializeField] private float camDeathShakeDuration = 0.3f;
    [SerializeField] private float camDeathShakeAmount = 0.05f;
    [SerializeField] private GameObject playerDeathEffect;
    [SerializeField] private float changeSceneDelay = 3f;

    [Header("Score")]
    [SerializeField] private int singleScoreIncrement = 100;
    [SerializeField] private int doubleScoreIncrement = 500;
    [SerializeField] private float deathScoreKeepPercent = 0.9f;
    [SerializeField] private Text uiScore;

    private PlatformGenerator p1PlatformsGen;
    private PlatformGenerator p2PlatformsGen;
    private float currentScore = 0f;
    private bool gameOver = false;

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

        if (gameOver)
        {
            return;
        }

        if (p1.playerDead || p2.playerDead)
        {
            currentScore += singleScoreIncrement * Time.deltaTime;
        }
        else
        {
            currentScore += doubleScoreIncrement * Time.deltaTime;
        }

        uiScore.text = "Score: " + (int)currentScore;
    }

    public void OnPlayerDeath(PlayerController caller)
    {
        // death effect
        Instantiate(playerDeathEffect, caller.transform.position, Quaternion.identity);

        // shake camera
        StartCoroutine(ShakeCamera(camDeathShakeDuration, camDeathShakeAmount));

        // both died, reset
        if (p1.playerDead && p2.playerDead)
        {
            gameOver = true;
            StartCoroutine(ChangeScene());
            return;
        }

        // update other player's velocity
        if (caller == p1)
        {
            p2PlatformsGen.DecrementVelocity(velocityDecrementDeathPercent);
        }
        else
        {
            p1PlatformsGen.DecrementVelocity(velocityDecrementDeathPercent);
        }

        // update overall score
        currentScore *= deathScoreKeepPercent;

        // respawn player
        StartCoroutine(RespawnPlayer(caller));
    }

    private IEnumerator RespawnPlayer(PlayerController player)
    {
        yield return new WaitForSeconds(playerDeathDuration);

        if (!gameOver)
        {
            player.Reset();
        }
    }

    private IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(changeSceneDelay);

        HighScoreManager.Instance.AddScore((int) currentScore, "MM-XX-GameJam");
        SceneManager.LoadScene("Highscore");
    }

    private IEnumerator ShakeCamera(float shakeDuration, float shakeAmount)
    {
        Vector3 originalPos = cam.transform.position;

        while (shakeDuration > 0)
        {
            Vector3 shake = Random.insideUnitSphere * shakeAmount;
            shake.z = 0;

            cam.transform.position = originalPos + shake;
            shakeDuration -= Time.deltaTime;

            yield return null;
        }

        cam.transform.position = originalPos;
    }
}
