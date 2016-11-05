using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InsertCoinController : MonoBehaviour {

    [SerializeField] private float duration = 0.5f;
    private Text text;

	void Start ()
    {
        text = GetComponent<Text>();
        StartCoroutine(Flicker());
	}

    IEnumerator Flicker()
    {
        yield return new WaitForSeconds(duration);

        Color color = text.color;
        color.a = color.a == 0f ? 1f : 0f;
        text.color = color;

        StartCoroutine(Flicker());
    }
}
