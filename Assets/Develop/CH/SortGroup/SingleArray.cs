using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleArray : MonoBehaviour
{
    public int value;
    public bool isSet = false;
    public Text Content => transform.GetComponentInChildren<Text>();
    public RectTransform rect => transform.GetComponent<RectTransform>();
    private void OnValidate()
    {
        if (isSet)
        {
            isSet = false;
            Content.text = value.ToString();
            transform.name= value.ToString();

            Vector2 sizeDelta = rect.sizeDelta;
            sizeDelta.y = value * 50;
            rect.sizeDelta = sizeDelta;

        }
    }
}
