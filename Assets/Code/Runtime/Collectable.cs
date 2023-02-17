using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Collectable : MonoBehaviour, IInteractable, ITooltip
{
    [SerializeField]
    private string tooltip;
    [SerializeField]
    private MushroomInfo mushroomInfo;

    [SerializeField]
    private int bonusBioMass = 1;

    public string Tooltip => tooltip;

    public void Interact()
    {
        mushroomInfo.BioMassPoints += bonusBioMass;
        Destroy(this.gameObject);
    }
}
