using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mycelium : MonoBehaviour
{
    [SerializeField]
    private GameObject strandPrefab;

    [SerializeField]
    private int maxPoints = 7;
    [SerializeField]
    private float growthRate = 0.2f;
    [SerializeField]
    private float maxGrowthAngle = 20;
    [SerializeField]
    private float maxPointDistance = 0.2f;

    private protected List<MyceliumStrand> strands = new List<MyceliumStrand>();

    public class MyceliumStrand
    {
        private int index = 1;
        private Vector3 currentGrowthDirection = Vector3.down;
        List<Vector3> points;
        private LineRenderer lineRenderer;

        public MyceliumStrand(Mycelium mycelium, int maxPoints, Transform parent, Vector3 localPosition, Quaternion localRotation)
        {
            this.mycelium = mycelium;
            this.maxPoints = maxPoints;

            var go = Instantiate(mycelium.strandPrefab, parent);
            go.transform.localPosition = localPosition;
            go.transform.localRotation = localRotation;
            this.lineRenderer = go.GetComponent<LineRenderer>();

            points = new List<Vector3>(maxPoints);
            if(maxPoints > 1)
            {
                points.Add(Vector3.zero); points.Add(Vector3.zero);
                lineRenderer.positionCount = 2;
                lineRenderer.SetPositions(points.ToArray());
            }
            currentGrowthDirection = mycelium.RandomizeDown(currentGrowthDirection);
        }

        private int maxPoints;
        private Mycelium mycelium;

        public bool Grow()
        {
            if (index >= maxPoints)
                return false;
            bool grewNewStrand = false;
            var activePoint = points[index];

            float travel = mycelium.growthRate * Time.deltaTime;
            Vector3 offset = travel * currentGrowthDirection;

            activePoint = points[index] = activePoint + offset;

            var sqrDist = (activePoint - points[index - 1]).sqrMagnitude;
            if (sqrDist >= mycelium.maxPointDistance * mycelium.maxPointDistance)
            {
                index++;
                points.Add(activePoint);
                lineRenderer.positionCount = points.Count;
                lineRenderer.SetPositions(points.ToArray());

                if (Random.value < 0.2f)
                {
                    int pointCount = this.maxPoints / 2 + 1;
                    if(pointCount > 2)
                    SpawnStrand(index - 2, pointCount);
                    grewNewStrand = true;
                }
                currentGrowthDirection = mycelium.RandomizeDown(currentGrowthDirection);
            }
            else
            {
                lineRenderer.SetPosition(index, activePoint);
            }
            return grewNewStrand;
        }

        private void SpawnStrand(int inheritedPosIndex, int pointCount)
        {
            Vector3 posA = points[inheritedPosIndex];
            Vector3 posB = points[inheritedPosIndex + 1];
            Vector3 growthDirection = posB - posA;
            Vector3 inbetweenPosition = Vector3.Lerp(posA, posB, Random.value);
            Vector3 perp = Vector2.Perpendicular(growthDirection);
            perp = Random.value < 0.5f ? perp : -perp;

            Vector3 worldPosition = lineRenderer.transform.TransformPoint(inbetweenPosition);
            Vector3 localPosition = mycelium.transform.InverseTransformPoint(worldPosition);

            Quaternion localRotation = Quaternion.LookRotation(Vector3.forward, perp);

            mycelium.strands.Add(
                new MyceliumStrand(
                    mycelium,
                    pointCount,
                    mycelium.transform,
                    localPosition,
                    localRotation
                    )
                );
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        strands.Add(new MyceliumStrand(this, maxPoints, this.transform, Vector3.zero, Quaternion.identity));
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var strand in strands) if(strand.Grow()) break;
    }

    private Vector3 RandomizeDown(Vector3 current) => Quaternion.Euler(0, 0, Random.Range(-maxGrowthAngle, maxGrowthAngle)) * current;
}
