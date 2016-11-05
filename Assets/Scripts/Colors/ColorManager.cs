using UnityEngine;
using System.Collections;

public class ColorManager : MonoBehaviour {

    [HideInInspector]
    public static ColorManager Instance = null;

    [SerializeField] private ColorController redColor;
    [SerializeField] private ColorController yellowColor;
    [SerializeField] private ColorController greenColor;

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

    public PlayerController GetOwner(ColorType type)
    {
        switch(type)
        {
            case ColorType.Red:
                return redColor.GetOwner();

            case ColorType.Yellow:
                return yellowColor.GetOwner();

            case ColorType.Green:
                return greenColor.GetOwner();

            default:
                return null;
        }
    }

    public void TryChangeColorOwner(ColorType type, PlayerController caller)
    {
        switch (type)
        {
            case ColorType.Red:
                redColor.TryChangeOwner(caller);
                break;

            case ColorType.Yellow:
                yellowColor.TryChangeOwner(caller);
                break;

            case ColorType.Green:
                greenColor.TryChangeOwner(caller);
                break;

            default:
                break;
        }
    }
}
