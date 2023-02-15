using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMouseInteract : MonoBehaviour
{
    Collider2D[] buffer = new Collider2D[10];
    [SerializeField]
    private new Camera camera;
    [SerializeField]
    private float minX, maxX;
    [SerializeField]
    private float panSpeed = 4f;
    [SerializeField]
    private float cornerPadding = 0.1f;
    [SerializeField]
    private float edgeThickness = 0.07f;

    // Update is called once per frame
    void Update()
    {
        HandleClickInteract();
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        float cornerOffset = screenSize.y * cornerPadding;
        float edgeWidth = edgeThickness * screenSize.x;

        Vector2 mousePosition = Input.mousePosition;

        if (mousePosition.y > cornerOffset && mousePosition.y < screenSize.y - cornerOffset)
            if (mousePosition.x < edgeWidth)
                Move(-panSpeed);
            else if (mousePosition.x > screenSize.x - edgeWidth)
                Move(panSpeed);
        void Move(float speed)
        {
            Vector3 pos = camera.transform.position;
            pos.x = Mathf.Clamp(pos.x + speed * Time.deltaTime, minX, maxX);
            camera.transform.position = pos;
        }
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
