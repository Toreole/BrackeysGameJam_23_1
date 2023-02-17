using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class InWorldTooltip : MonoBehaviour, ITooltip
{
    [SerializeField, TextArea]
    private string tooltip;

    public string Tooltip => tooltip;
}