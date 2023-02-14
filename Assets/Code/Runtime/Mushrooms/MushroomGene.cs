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

    public bool Acquired { get; private set; } = false;

    public string Tooltip => $"<u>{name}</u>\n{description}";

    public bool AcquireGene(MushroomInfo info)
    {
        if (parentGene.Acquired is false || info.BioMassPoints < this.cost)
            return false;

        Acquired = true;
        return true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }
}

[Serializable]
public struct ColorRange
{
    [SerializeField]
    private Color colorA, colorB;

    public Color GetColor() => Color.Lerp(colorA, colorB, UnityEngine.Random.value);
}