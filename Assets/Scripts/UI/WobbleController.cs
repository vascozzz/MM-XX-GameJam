using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class WobbleController : MonoBehaviour
{
    [SerializeField]
    private float wobble = 55f;
    [SerializeField, Range(0f, 2f)]
    private float speed = 0.5f;

    private RectTransform rectTransform;
    private Vector3 goal;
    private Vector3 init;

    private float amount = 0f;


    void Start()
    {
        rectTransform = this.GetComponent<RectTransform>();
        init = rectTransform.anchoredPosition3D;
        goal = init + new Vector3(0f, wobble, 0f);
    }


    void Update()
    {
        amount += speed * Time.deltaTime;

        if (amount > 1)
        {
            Vector3 temp = goal;
            goal = init;
            init = temp;
            amount = 0f;
        }

        rectTransform.anchoredPosition3D = Vector3.Lerp(init, goal, amount);
    }
}
