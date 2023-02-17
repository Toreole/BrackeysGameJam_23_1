using System.Collections;
using UnityEngine;

public class HiddenMushroomGene : MushroomGene
{
    protected override void Start()
    {
        base.Start();
        gameObject.SetActive(false);
    }
    protected override void OnAcquireParent()
    {
        base.OnAcquireParent();
        gameObject.SetActive(true);
    }
}