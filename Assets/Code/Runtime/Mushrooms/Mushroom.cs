using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    [SerializeField]
    private Transform shroomTransform, stemTransform, capTransform;
    [SerializeField]
    private Mycelium mycelium;

    [SerializeField]
    private SpriteRenderer stemRenderer, capRenderer;
    [SerializeField]
    private SpriteMask stemMask, capMask;

    [SerializeField]
    private AnimationCurve growthCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private float maxSize;
    private float growthTime;

    private float growthPercentage = 0f;
    public float GrowthStage => growthPercentage;

    public void Init(MushroomInfo info, int id)
    {
        growthPercentage = 0;
        shroomTransform.localScale = Vector3.zero;

        capRenderer.sortingOrder = capMask.frontSortingOrder = id;
        capMask.backSortingOrder = id-1;

        stemRenderer.sortingOrder = stemMask.frontSortingOrder = -id;
        stemMask.backSortingOrder = -id - 1;

        stemTransform.localScale = Random.value < 0.5f ? new Vector3(-1, 1, 1) : Vector3.one;
        capTransform.localScale = Random.value < 0.5f ? new Vector3(-1, 1, 1) : Vector3.one;
        capTransform.localRotation = Quaternion.AngleAxis(Random.Range(-15, 15), Vector3.forward);

        var settings = info.MushroomSettings;

        float variance = settings.sizeVariance;
        capTransform.localScale *= Random.Range(1 - variance, 1 + variance);

        capRenderer.color = settings.colorRange.GetColor();
        capRenderer.sprite = settings.capTexture;
        capMask.sprite = settings.capShape;

        stemRenderer.sprite = settings.stemTexture;
        stemMask.sprite = settings.stemShape;

        maxSize = settings.maxSize + Random.value * settings.sizeVariance;
        growthTime = settings.growTime;
    }

    // Update is called once per frame
    void Update()
    {
        growthPercentage += Time.deltaTime / growthTime;
        growthPercentage = Mathf.Min(1, growthPercentage);
        float size = maxSize * growthCurve.Evaluate(growthPercentage);
        shroomTransform.localScale = new Vector3(size, size, size);
    }

    public Bounds CalculateMushroomBoundsRelativeTo(Transform space)
    {
        Bounds capBounds = capRenderer.bounds;
        Bounds stemBounds = stemRenderer.bounds;
        Vector2 min = capBounds.min, max = capBounds.max;
        Vector2 stemMin = stemBounds.min, stemMax = stemBounds.max;
        min.x = Mathf.Min(min.x, stemMin.x);
        min.y = Mathf.Min(min.y, stemMin.y);
        max.x = Mathf.Max(max.x, stemMax.x);
        max.y = Mathf.Max(max.y, stemMax.y);
        Vector2 localCenter = space.InverseTransformPoint(capBounds.center);
        return new Bounds(localCenter, max-min);
    }
}
