using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    [SerializeField]
    private Transform shroomTransform, stemTransform, capTransform;

    [SerializeField]
    private SpriteRenderer stemRenderer, capRenderer;

    [SerializeField]
    private float growthTime = 16f;
    [SerializeField]
    private AnimationCurve growthCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [SerializeField]
    private float maxSize = 1.2f;

    private float growthPercentage = 0f;

    public float GrowthStage => growthPercentage;

    // Start is called before the first frame update
    void Start()
    {
        maxSize += Random.Range(-0.3f, 0.5f);
        stemTransform.localScale = Random.value < 0.5f? new Vector3(-1, 1, 1): Vector3.one;
        capTransform.localScale = Random.value < 0.5f ? new Vector3(-1, 1, 1) : Vector3.one;
        capTransform.localScale *= Random.Range(0.85f, 1.15f);
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
