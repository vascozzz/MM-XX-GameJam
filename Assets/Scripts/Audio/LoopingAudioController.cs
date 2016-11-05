using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class LoopingAudioController : MonoBehaviour
{
    [SerializeField] private AudioClip initialClip;
    [SerializeField] private AudioClip loopingClip;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(playEngineSound());
    }

    IEnumerator playEngineSound()
    {
        audioSource.clip = initialClip;
        audioSource.Play();

        yield return new WaitForSeconds(audioSource.clip.length);

        audioSource.loop = true;
        audioSource.clip = loopingClip;
        audioSource.Play();
    }
}
