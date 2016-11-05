using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using InControl;

public class SkipSceneController : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    [SerializeField] private float changeSceneDelay = 15f;

    private float changeSceneTime;

    void Start()
    {
        changeSceneTime = Time.time + changeSceneDelay;
    }

    void Update()
    {
        if (Time.time > changeSceneTime)
        {
            SceneManager.LoadScene(nextSceneName);
        }

        foreach (InputDevice device in InputManager.Devices)
        {
            if (device.Action1.WasPressed)
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }
}
