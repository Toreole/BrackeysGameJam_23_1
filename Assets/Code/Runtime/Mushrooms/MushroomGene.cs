using System;
using System.Text;
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
    private int bonusColonySize;
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
    public event Action OnGeneAcquired;

    public string Tooltip 
    { 
        get 
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"<u>{name}</u>");
            sb.AppendLine(description);
            if (parentGene) sb.AppendLine($"Requires: {parentGene.name}");
            if (conflictingGene) sb.AppendLine($"Incompatible with: {conflictingGene.name}");
            sb.AppendLine($"Cost: {cost}");
            return sb.ToString();
        } 
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        //setting myself above the silly IDE warning of "no null propagation" 
        //because these are never destroyed, theyre either null or valid.
        this.interactable = (parentGene?.Acquired ?? true) && (!conflictingGene?.Acquired ?? true) && !this.Acquired;
    }

    protected override void Start()
    {
        base.Start();
        if (parentGene) parentGene.OnGeneAcquired += OnAcquireParent;
        if (conflictingGene) conflictingGene.OnGeneAcquired += OnAcquireConflicting;
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
        ApplyChanges();
        Acquired = true;
        ColorBlock cb = this.colors;
        cb.disabledColor = Color.green;
        this.colors = cb;
        this.interactable = false;
        OnGeneAcquired?.Invoke();
    }

    private void ApplyChanges()
    {
        var ms = info.MushroomSettings;
        ms.maxSize += bonusSizeMax;
        ms.growTime += changeGrowTime;
        if (this.changeCapColor) ms.colorRange = this.overrideCapColor;
        if (this.overrideCapShape) ms.capShape = this.overrideCapShape;
        if (this.overrideCapTexture) ms.capTexture = this.overrideCapTexture;
        if (this.overrideStemShape) ms.stemShape = this.overrideStemShape;
        if (this.overrideStemTexture) ms.stemTexture = this.overrideStemTexture;

        var ss = info.SpeciesSettings;
        ss.colonySize += bonusColonySize;
        if (string.IsNullOrEmpty(overrideSpeciesPrefix) == false) ss.namePrefix = overrideSpeciesPrefix;
        if (string.IsNullOrEmpty(overrideSpeciesName) == false) ss.name = overrideSpeciesName;
        if (string.IsNullOrEmpty(overrideSpeciesSuffix) == false) ss.nameSuffix = overrideSpeciesSuffix;

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