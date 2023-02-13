using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class MushroomColony : MonoBehaviour
{
    [SerializeField]
    private GameObject mushroomPrefab;

    [SerializeField]
    private int mushroomCount = 8;
    [SerializeField]
    private Vector2 spawnRectSize = new Vector2(5, 2);
    [SerializeField]
    private float spacing = 0.37f;
    [SerializeField]
    private float spawnRate = 0.6f; //per second.
    [SerializeField]
    private BoxCollider2D boundsCollider;

    private float timeBetweenSpawns;
    private float spawnDeltaTime = 0;
    private Vector2 spawnRectExtents;
    private float sqrSpacing;

    private List<Mushroom> mushrooms;

    private void Start()
    {
        timeBetweenSpawns = 1f / spawnRate;
        mushrooms = new List<Mushroom>(mushroomCount);
        spawnRectExtents = 0.5f * spawnRectSize;
        sqrSpacing = spacing * spacing;
        boundsCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        spawnDeltaTime += Time.deltaTime;
        if(spawnDeltaTime > timeBetweenSpawns && mushrooms.Count < mushroomCount)
        {
            SpawnMushroom();
        }
        if((Time.frameCount % 2) == 1) //only update collider bounds on odd frames.
            UpdateColliderBounds();
    }

    private void UpdateColliderBounds()
    {
        //Bounds bounds = GetLocalBounds(boundsCollider);
        Vector3 min = Vector3.positiveInfinity, max = Vector3.negativeInfinity;
        if(mushrooms.Count == 0)
        {
            boundsCollider.size = Vector2.zero;
            boundsCollider.offset = Vector2.zero;
            return;
        }
        foreach(Mushroom m in mushrooms)
        {
            Bounds mbounds = m.CalculateMushroomBoundsRelativeTo(this.transform);
            Vector3 mmin = mbounds.min, mmax = mbounds.max;
            min.x = Mathf.Min(min.x, mmin.x);
            min.y = Mathf.Min(min.y, mmin.y);
            min.z = Mathf.Min(min.z, mmin.z);

            max.x = Mathf.Max(max.x, mmax.x);
            max.y = Mathf.Max(max.y, mmax.y);
            max.z = Mathf.Max(max.z, mmax.z);
        }
        var center = Vector3.Lerp(min, max, 0.5f);
        var size = max - min;
        //bounds.min = min; bounds.max = max;
        boundsCollider.size = size;
        boundsCollider.offset = center;
    }

    private Bounds GetLocalBounds(BoxCollider2D boundsCollider)
    {
        var b = boundsCollider.bounds;
        return new Bounds(transform.InverseTransformPoint(b.center), b.size);
    }

    private void SpawnMushroom()
    {
        Vector2 localSpawnPoint = GetRandomSpawnPoint();
        while(!IsValidSpawnPoint(localSpawnPoint))
            localSpawnPoint = GetRandomSpawnPoint();

        GameObject inst = Instantiate(mushroomPrefab, this.transform);
        Transform nT = inst.transform;
        nT.localPosition = localSpawnPoint;

        mushrooms.Add(inst.GetComponentInChildren<Mushroom>());
        spawnDeltaTime = 0;
    }

    private Vector2 GetRandomSpawnPoint()
    {
        return new Vector2(
            Random.Range(0, spawnRectSize.x) - spawnRectExtents.x,
            Random.Range(0, spawnRectSize.y) - spawnRectExtents.y
            ) ;
    }

    private bool IsValidSpawnPoint(Vector2 pos)
    {
        if(mushrooms.Count != 0)
            foreach(var m in mushrooms)
            {
                Vector2 mp = m.transform.localPosition;
                var sqrD = Vector2.SqrMagnitude(pos - mp);
                if (sqrD < sqrSpacing)
                    return false;
            }
        return true;
    }

}
