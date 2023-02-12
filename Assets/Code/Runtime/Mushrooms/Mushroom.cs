using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    [SerializeField]
    private Transform shroomTransform, stemTransform, capTransform;

    [SerializeField]
    private SpriteRenderer stemRenderer, capRenderer;

    [SerializeField]
    private float growthRate = 0.05f;

    [SerializeField]
    private float maxSize = 1.2f;

    private float size = 0f;

    // Start is called before the first frame update
    void Start()
    {
        maxSize = maxSize + Random.Range(-0.3f, 0.5f);
        stemTransform.localScale = Random.value < 0.5f? new Vector3(-1, 1, 1): Vector3.one;
        capTransform.localScale = Random.value < 0.5f ? new Vector3(-1, 1, 1) : Vector3.one;
        capTransform.localScale *= Random.Range(0.85f, 1.15f);
    }

    // Update is called once per frame
    void Update()
    {
        size += growthRate * Time.deltaTime;
        size = Mathf.Min(size, maxSize);

        shroomTransform.localScale = new Vector3(size, size, size);
    }
}
