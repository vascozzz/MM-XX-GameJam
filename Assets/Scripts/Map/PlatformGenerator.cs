using UnityEngine;
using System.Collections;

public class PlatformGenerator : MonoBehaviour {

    private GameObject[] platforms;
    private GameObject platformsContainer;
    private GameObject[] deathRays;
    private GameObject deathRaysContainer;
    private float velocity = 0.8f;
    private float padding = 5f;

    private float currentGenY;
    private Transform lastGen = null;

    private Camera cam;
    private float leftCorner;
    private float rightCorner;

    public void Initialize(Camera cam, GameObject[] platforms, GameObject platformsContainer, GameObject[] deathRays,  GameObject deathRaysContainer, float leftCorner, float rightCorner, float startY)
    {
        this.cam = cam;
        this.platforms = platforms;
        this.platformsContainer = platformsContainer;
        this.deathRays = deathRays;
        this.deathRaysContainer = deathRaysContainer;
        this.leftCorner = leftCorner;
        this.rightCorner = rightCorner;
        this.currentGenY = startY;
    }

    public void Generate()
    {
        GeneratePlatform();

        if (Random.Range(0f, 1000f) < 20f)
        {
            GenerateDeathRay();
        }
    }

    private void GeneratePlatform()
    {
        CamToWorldUtility.CameraBounds camBounds = CamToWorldUtility.GetCameraBoundsInWorld(cam);

        if (lastGen != null) currentGenY = lastGen.position.y;

        float topEdge = camBounds.up.y + padding;
        float randomGap = 1f;

        while (currentGenY + randomGap < topEdge)
        {
            currentGenY += randomGap;

            GameObject platform = platforms[Random.Range(0, platforms.Length)];
            float platformWidth = platform.GetComponent<BoxCollider2D>().size.x;

            float leftBorder = leftCorner + platformWidth / 2;
            float rightBorder = rightCorner - platformWidth / 2;

            float platformPosX = Random.Range(leftBorder, rightBorder);

            GameObject platformObj = Instantiate(platform, new Vector3(platformPosX, currentGenY, 0), Quaternion.identity, platformsContainer.transform) as GameObject;

            PlatformController platCtrl = platformObj.GetComponent<PlatformController>();
            platCtrl.movement = new Vector2(0f, -velocity);
            platCtrl.outsideY = camBounds.down.y - padding;

            lastGen = platformObj.transform;
        }
    }

    private void GenerateDeathRay()
    {
        CamToWorldUtility.CameraBounds camBounds = CamToWorldUtility.GetCameraBoundsInWorld(cam);
        float topEdge = camBounds.up.y + padding;

        GameObject deathRay = deathRays[Random.Range(0, deathRays.Length)];

        Vector3 deathRayPos = new Vector3(Mathf.Lerp(leftCorner, rightCorner, 0.5f), topEdge, 0f);
        float deathRayNewScaleX = (camBounds.right.x - camBounds.left.x) / 2f;

        GameObject deathRayObj = Instantiate(deathRay, deathRayPos, Quaternion.identity, deathRaysContainer.transform) as GameObject;

        PlatformController platCtrl = deathRayObj.GetComponent<PlatformController>();
        platCtrl.movement = new Vector2(0f, -velocity);
        platCtrl.outsideY = camBounds.down.y - padding;

        Vector3 deathRayNewScale = deathRayObj.transform.localScale;
        deathRayNewScale.x = deathRayNewScaleX;
        deathRayObj.transform.localScale = deathRayNewScale;
    }


    public void SetVelocity(float velocity)
    {
        this.velocity = velocity;
    }
}
