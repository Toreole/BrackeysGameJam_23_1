using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Code.Runtime
{
    public class FastForwardCheat : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            Time.timeScale = 3;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Time.timeScale = 1;
        }
    }
}