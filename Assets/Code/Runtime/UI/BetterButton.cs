using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BetterButton : Selectable, ITooltip, IPointerClickHandler
{
    [SerializeField]
    private string tooltip;
    [SerializeField]
    private UnityEvent onClick;

    public string Tooltip => tooltip;

    private bool disabled;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!disabled)
            onClick?.Invoke();
    }

    public void DisableSelfAfterDelay(float delay)
    {
        disabled = true;
        StartCoroutine(Delete(delay));
        IEnumerator Delete(float t)
        {
            this.interactable = false;
            var img = this.image;
            var col = img.color;
            var text = this.GetComponentInChildren<TextMeshProUGUI>();
            var tcol = text.color;

            for(float t2 = 0f; t2 < delay; t2+= Time.deltaTime)
            {
                col.a = 1 - t2 / delay;
                tcol.a = col.a;
                text.color = tcol;
                img.color = col;
                yield return null;
            }
            Destroy(this.gameObject);
        }
    }
}
