using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Genes change the stats of the mushroom.
/// </summary>
[RequireComponent(typeof(Image))]
public class MushroomGene : Selectable, ITooltip, IPointerClickHandler
{
    [Header("Mushroom Gene")]
    [SerializeField]
    private MushroomGene parentGene;
    [SerializeField]
    private MushroomGene conflictingGene;
    [SerializeField]
    private int cost = 1;
    [SerializeField]
    private new string name;
    [SerializeField]
    private string description;

    [Header("Gene Changes")]
    [SerializeField]
    private string overrideSpeciesPrefix;
    [SerializeField]
    private string overrideSpeciesSuffix;
    [SerializeField]
    private string overrideSpeciesName;
    [SerializeField]
    private float changeGrowTime;
    [SerializeField]
    private float bonusSizeMax;
    [SerializeField]
    private Sprite overrideStemShape, overrideStemTexture;
    [SerializeField]
    private bool changeStemColor = false;
    [SerializeField]
    private ColorRange overrideStemColor;
    [SerializeField]
    private Sprite overrideCapShape, overrideCapTexture;
    [SerializeField]
    private bool changeCapColor = false;
    [SerializeField]
    private ColorRange overrideCapColor;

    //injected
    private MushroomInfo info;

    public bool Acquired { get; private set; } = false;
    public event Action onGeneAcquired;

    public string Tooltip => $"<u>{name}</u>\n{description}";

    protected override void OnEnable()
    {
        base.OnEnable();
        if(parentGene) parentGene.onGeneAcquired += OnAcquireParent;
        if(conflictingGene) conflictingGene.onGeneAcquired += OnAcquireConflicting;
        //setting myself above the silly IDE warning of "no null propagation" 
        //because these are never destroyed, theyre either null or valid.
        this.interactable = (parentGene?.Acquired ?? true) && (!conflictingGene?.Acquired ?? true);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (parentGene) parentGene.onGeneAcquired -= OnAcquireParent;
        if (conflictingGene) conflictingGene.onGeneAcquired -= OnAcquireConflicting;
    }

    private void OnAcquireParent()
    {
        this.interactable = !conflictingGene?.Acquired ?? true;
    }

    private void OnAcquireConflicting()
    {
        this.interactable = false;
    }

    private void AcquireGene()
    {
        if (info.BioMassPoints < this.cost || this.Acquired)
            return;

        info.BioMassPoints -= this.cost;

        //TODO:
        //apply effects to mushroom info object.

        Acquired = true;
        onGeneAcquired?.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AcquireGene();
    }

    public void Init(MushroomInfo info)
    {
        this.info = info;
    }
}

[Serializable]
public struct ColorRange
{
    [SerializeField]
    private Color colorA, colorB;

    public Color GetColor() => Color.Lerp(colorA, colorB, UnityEngine.Random.value);
}