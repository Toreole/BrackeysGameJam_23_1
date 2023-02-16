using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTooltip : MonoBehaviour, ITooltip
{
    [SerializeField]
    private string tooltip;

    public string Tooltip => tooltip;
}
