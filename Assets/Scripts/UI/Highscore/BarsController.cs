using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BarsController : MonoBehaviour {

    [SerializeField] private float offset = 100f;
    [SerializeField] private float duration = 1f;

    private RectTransform rectTransf;

	void Start ()
    {
        rectTransf = GetComponent<RectTransform>();

        Sequence seq = DOTween.Sequence();
        seq.Append(rectTransf.DOAnchorPosY(offset, duration).SetEase(Ease.Linear));
        seq.Append(rectTransf.DOAnchorPosY(-offset, duration).SetEase(Ease.Linear));
        seq.SetLoops(-1);
    }
}
