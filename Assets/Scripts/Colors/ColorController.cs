using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class ColorController : MonoBehaviour {

    [SerializeField] private ColorType color;
    [SerializeField] private PlayerController player1;
    [SerializeField] private Transform player1Container;
    [SerializeField] private Image player1UI;
    [SerializeField] private PlayerController player2;
    [SerializeField] private Transform player2Container;
    [SerializeField] private Image player2UI;
    [SerializeField] private PlayerController startOwner;
    [SerializeField] private float travelTime = 0.5f;
    [SerializeField] private Ease ease = Ease.InOutQuart;
    [SerializeField] private KeyCode key;

    private PlayerController owner;

    //Traveling
    private bool travelling;
    private Vector3 targetStartPos;
    private Vector3 tweenPos;

	void Start ()
    {
        owner = startOwner;
        UpdateColorPosition();
        UpdateUI();
    }
	
	void Update ()
    {
        UpdateColorPosition();
        UpdateUI();

        if (Input.GetKeyDown(key))
        {
            ChangeOwner();
        }   
	}

    private void UpdateColorPosition()
    {
        if(travelling)
        {
            Vector3 deltaPos = GetOwnerContainer().position - targetStartPos;
            transform.position = tweenPos + deltaPos;
        }
        else
        {
            transform.position = GetOwnerContainer().position;
        }
    }

    private void UpdateUI()
    {
        if(owner == player1)
        {
            player1UI.enabled = true;
            player2UI.enabled = false;
        }

        if(owner == player2)
        {
            player2UI.enabled = true;
            player1UI.enabled = false;
        }

        if(travelling)
        {
            player1UI.enabled = false;
            player2UI.enabled = false;
        }
    }

    private void ChangeOwner()
    {
        if (travelling) return;
        travelling = true;

        //Get Start Pos (before changing owner)
        tweenPos = GetOwnerContainer().position;

        if (owner == player1) owner = player2;
        else if (owner == player2) owner = player1;

        //Get Target Pos (after changing owner)
        targetStartPos = GetOwnerContainer().position;

        //Create tweener to modify tweenPos
        DOTween.To(() => tweenPos, x => tweenPos = x, targetStartPos, travelTime).SetEase(ease).OnComplete(() => { travelling = false; });
    }

    //Utility
    private Transform GetOwnerContainer()
    {
        if (owner == player1) return player1Container;
        if (owner == player2) return player2Container;
        return null;
    }

    public PlayerController GetOwner()
    {
        if (travelling) return null;
        if (owner == player1) return player1;
        if (owner == player2) return player2;
        return null;
    }
}
