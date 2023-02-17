using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MushroomInfo : MonoBehaviour
{
    [SerializeField]
    private int bioMassPoints = 0;

    [SerializeField]
    private MushroomSettings mushroomSettings;
    [SerializeField]
    private MyceliumSettings myceliumSettings;
    [SerializeField]
    private SpeciesSettings speciesSettings;

    public int BioMassPoints 
    { 
        get => bioMassPoints; 
        internal set
        {
            if (bioMassPoints == value)
                return;
            OnBioMassPointsChanged?.Invoke(value);
            bioMassPoints = value; 
        } 
    }
    public MyceliumSettings MyceliumSettings => myceliumSettings;
    public MushroomSettings MushroomSettings => mushroomSettings;
    public SpeciesSettings SpeciesSettings => speciesSettings;

    public event Action<int> OnBioMassPointsChanged;

    private void Awake()
    {
        foreach (var x in FindObjectsOfType<MushroomGene>()) x.Init(this);
    }
}

[Serializable]
public class MushroomSettings
{
    public float growTime;
    public float maxSize;
    public float sizeVariance;
    public ColorRange colorRange;
    public Sprite capTexture, capShape;
    public Sprite stemTexture, stemShape;

}

[Serializable]
public class SpeciesSettings
{
    public string namePrefix, nameSuffix, name;
    public int colonySize;
    public int colonyCount;
    public float mushroomSpacing = 0.4f;

    public string FullName
    {
        get
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(namePrefix))
            {
                sb.Append(namePrefix);
                sb.Append(' ');
            }
            if (!string.IsNullOrEmpty(name))
                sb.Append(name);
            if (!string.IsNullOrEmpty(nameSuffix))
            {
                sb.Append(' ');
                sb.Append(nameSuffix);
            }
            return sb.ToString();
        }
    }
}

[Serializable]
public class MyceliumSettings
{
    public float growthRate;
}
