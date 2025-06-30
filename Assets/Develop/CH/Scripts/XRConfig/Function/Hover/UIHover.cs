using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TS.Tools
{
    public class UIHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        private void ScaleUI(bool isHovered)
        {
            if (transform.GetComponent<Button>().interactable)
            {
                Vector3 targetScale = isHovered ? new Vector3(1.1f, 1.1f, 1.1f) : new Vector3(1f, 1f, 1f);
                float duration = 0.1f;
                transform.DOScale(targetScale, duration);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            ScaleUI(true);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            ScaleUI(false);
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            ScaleUI(true);
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            ScaleUI(false);
        }
    }
}