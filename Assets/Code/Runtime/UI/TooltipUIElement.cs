using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TooltipUIElement : MonoBehaviour
{
    [SerializeField]
    private RectTransform self = null;
    [SerializeField]
    private new Camera camera;
    [SerializeField]
    private TextMeshProUGUI textGUI;
    [SerializeField]
    private CanvasGroup group;
    [SerializeField]
    private Canvas canvas;
    
    private GraphicRaycaster graphicRaycaster;
    private RectTransform canvasTransform;

    void Start()
    {
        canvasTransform = canvas.transform as RectTransform;
        self ??= transform as RectTransform;
        graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
    }

    Collider2D[] buffer = new Collider2D[8];

    void LateUpdate()
    {
        var mousePosition = Input.mousePosition;
        Vector2 worldPosition = camera.ScreenToWorldPoint(mousePosition);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, mousePosition, canvas.worldCamera, out Vector2 localPoint);
        
        Vector2 tempPos = canvas.transform.TransformPoint(localPoint);

        //at this point, the position is exactly on top of the mouse (the center of the element)
        self.position = tempPos;
        //after this assignment, the self.anchoredPosition is correct on the canvas rect.

        Vector2 size = self.rect.size;
        Vector2 halfSize = size * 0.5f;
        Vector2 limit = canvasTransform.rect.size;
        limit.x -= halfSize.x;
        limit.y -= halfSize.y;

        Vector2 anchoredMax = self.anchoredPosition + size * 0.5f;
        Vector2 anchoredMin = anchoredMax - size;

        //offset from mouse
        Vector2 offset = new Vector2(halfSize.x, -halfSize.y);
        Vector2 targetPosition = self.anchoredPosition + offset;
        //limit position to its always fully onscreen
        targetPosition.x = Mathf.Clamp(targetPosition.x, halfSize.x, limit.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, halfSize.y, limit.y);
        self.anchoredPosition = targetPosition;

        group.alpha = 0;
        if(!UpdateTooltipFromGUI(mousePosition))
            UpdateTooltipFromWorld(worldPosition);
    }

    private List<RaycastResult> resultList = new List<RaycastResult>(20);

    private bool UpdateTooltipFromGUI(Vector2 mousePosition)
    {
        resultList.Clear();
        var pointerEvent = new PointerEventData(EventSystem.current);
        pointerEvent.position = mousePosition;

        graphicRaycaster.Raycast(pointerEvent, resultList);
        for(int i = 0; i < resultList.Count; i++)
        {
            var result = resultList[i];
            var target = result.gameObject.GetComponent<ITooltip>();
            if (target == null)
                continue;
            textGUI.text = target.Tooltip;
            group.alpha = 1;
            return true;
        }
        //return false when no ui element is hit. (anything that blocks raycasts)
        return resultList.Count != 0;
    }

    private void UpdateTooltipFromWorld(Vector2 worldPosition)
    {
        int n = Physics2D.OverlapPointNonAlloc(worldPosition, buffer);
        for (int i = 0; i < n; i++)
        {
            var target = buffer[i].GetComponent<ITooltip>();
            if (target == null)
                continue;
            textGUI.text = target.Tooltip;
            group.alpha = 1;
        }
    }
}
