using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMouseInteract : MonoBehaviour
{
    Collider2D[] buffer = new Collider2D[10];
    [SerializeField]
    private new Camera camera;

    // Update is called once per frame
    void Update()
    {
        HandleClickInteract();
    }

    private void HandleClickInteract()
    {
        if (!Input.GetMouseButtonDown(0))
            return;
        var pos = camera.ScreenToWorldPoint(Input.mousePosition);
        int n = Physics2D.OverlapPointNonAlloc(pos, buffer);
        for (int i = 0; i < n; i++)
        {
            IInteractable interactable = buffer[i].GetComponent<IInteractable>();
            if (interactable == null)
                continue;
            interactable.Interact();
        }
    }
}
