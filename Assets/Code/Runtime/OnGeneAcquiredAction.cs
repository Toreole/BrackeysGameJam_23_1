using UnityEngine.Events;
using UnityEngine;


public class OnGeneAcquiredAction : MonoBehaviour
{
    [SerializeField]
    private UnityEvent action;

    void Start()
    {
        var gene = GetComponent<MushroomGene>();
        if (gene)
            gene.OnGeneAcquired += action.Invoke;
    }
}