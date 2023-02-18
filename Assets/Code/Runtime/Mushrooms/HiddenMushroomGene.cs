using System.Collections;
using UnityEngine;

public class HiddenMushroomGene : MushroomGene
{
    protected override void Start()
    {
        base.Start();
        if (Application.isEditor && !Application.isPlaying)
            return;
        gameObject.SetActive(false);
    }
    protected override void OnAcquireParent()
    {
        base.OnAcquireParent();
        gameObject.SetActive(true);
    }
}