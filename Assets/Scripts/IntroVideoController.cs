using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IntroVideoController : MonoBehaviour {

    private MovieTexture movieTexture;

    void Start()
    {
        movieTexture = ((MovieTexture)GetComponent<RawImage>().material.mainTexture);
        StartCoroutine(DestroyVideo(movieTexture.duration));
        movieTexture.Play();
    }

    IEnumerator DestroyVideo(float time)
    {
        yield return new WaitForSeconds(time - 0.5f);

        Destroy(this.gameObject);
    }
}
