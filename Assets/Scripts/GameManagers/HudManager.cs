using UnityEngine;
using System.Collections;

public class HudManager : MonoBehaviour
{
    [HideInInspector] public static HudManager Instance = null;

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
}
