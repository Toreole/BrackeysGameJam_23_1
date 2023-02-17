using UnityEngine;
using UnityEngine.Events;

public class OnDestroyAction : MonoBehaviour
{
    [SerializeField]
    private UnityEvent action;
    private void OnDestroy()
    {
        action?.Invoke();
    }
}