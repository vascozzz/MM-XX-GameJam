using UnityEngine;
using System.Collections;

public class PlatformGenerator : MonoBehaviour {

    private GameObject[] platforms;
    private GameObject platformsContainer;
    private GameObject[] deathRays;
    private GameObject deathRaysContainer;
    private float padding = 5f;
    private float deathRaysProbability;
    private float deathRaysOffset;
    private float currentVelocity;
    private float velocityIncrement;

    private float currentGenY;
    private Transform lastGen = null;

    private CamToWorldUtility.CameraBounds camBounds;
    private float leftCorner;
    private float rightCorner;

    public void Initialize(Camera cam, GameObject[] platforms, GameObject platformsContainer, GameObject[] deathRays, GameObject deathRaysContainer, 
        float leftCorner, float rightCorner, float startY, float deathRaysProbability, float deathRaysOffset, float initialVelocity, float velocityIncrement)
    {
        this.camBounds = CamToWorldUtility.GetCameraBoundsInWorld(cam);
        this.platforms = platforms;
        this.platformsContainer = platformsContainer;
        this.deathRays = deathRays;
        this.deathRaysContainer = deathRaysContainer;
        this.leftCorner = leftCorner;
        this.rightCorner = rightCorner;
        this.currentGenY = startY;
        this.deathRaysProbability = deathRaysProbability;
        this.deathRaysOffset = deathRaysOffset;
        this.currentVelocity = initialVelocity;
        this.velocityIncrement = velocityIncrement;
    }

    public void Generate()
    {
        GeneratePlatform();
    }

    private void GeneratePlatform()
    {
        if (lastGen != null) currentGenY = lastGen.position.y;

        float topEdge = camBounds.up.y + padding;
        float randomGap = 1f;

        while (currentGenY + randomGap < topEdge)
        {
            // only increment velocity whenever a new platform is generated
            UpdateVelocity();

            // generate a deathray every so often
            if (Random.value < deathRaysProbability)
            {
                GenerateDeathRay();
            }

            currentGenY += randomGap;

            GameObject platform = platforms[Random.Range(0, platforms.Length)];
            float platformWidth = platform.GetComponent<BoxCollider2D>().size.x;

            float leftBorder = leftCorner + platformWidth / 2;
            float rightBorder = rightCorner - platformWidth / 2;

            float platformPosX = Random.Range(leftBorder, rightBorder);

            GameObject platformObj = Instantiate(platform, new Vector3(platformPosX, currentGenY, 0), Quaternion.identity, platformsContainer.transform) as GameObject;

            PlatformController platCtrl = platformObj.GetComponent<PlatformController>();
            platCtrl.movement = new Vector2(0f, -currentVelocity);
            platCtrl.outsideY = camBounds.down.y - padding;

            lastGen = platformObj.transform;
        }
    }

    private void GenerateDeathRay()
    {
        float topEdge = camBounds.up.y + padding;

        GameObject deathRay = deathRays[Random.Range(0, deathRays.Length)];

        // offset to separate rays from platforms, as they're generated at the same time
        Vector3 deathRayPosOffset = new Vector3(0f, Random.Range(0f, deathRaysOffset), 0f);
        Vector3 deathRayPos = new Vector3(Mathf.Lerp(leftCorner, rightCorner, 0.5f), topEdge, 0f) + deathRayPosOffset;

        float deathRayNewScaleX = (camBounds.right.x - camBounds.left.x) / 2f;

        GameObject deathRayObj = Instantiate(deathRay, deathRayPos, Quaternion.identity, deathRaysContainer.transform) as GameObject;

        PlatformController platCtrl = deathRayObj.GetComponent<PlatformController>();
        platCtrl.movement = new Vector2(0f, -currentVelocity);
        platCtrl.outsideY = camBounds.down.y - padding;

        Vector3 deathRayNewScale = deathRayObj.transform.localScale;
        deathRayNewScale.x = deathRayNewScaleX;
        deathRayObj.transform.localScale = deathRayNewScale;
    }

    public void UpdateVelocity()
    {
        currentVelocity += velocityIncrement * Time.deltaTime;
        Debug.Log(currentVelocity);
    }
}
