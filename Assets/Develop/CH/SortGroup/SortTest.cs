using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SortTest : MonoBehaviour
{
    public Transform ArrayParent => transform.GetComponentInChildren<HorizontalLayoutGroup>().transform;

    public bool isSet = false;

    private void OnValidate()
    {
        if (isSet)
        {
            isSet = false;

        }
    }


    public void SetValue(int child)
    {
        RectTransform rect = ArrayParent.GetChild(child).GetComponent<RectTransform>();
        Text Content = rect.GetComponentInChildren<Text>();

        int value = Random.Range(1, 21);
        Content.text = value.ToString();
        rect.transform.name = value.ToString();

        Vector2 sizeDelta = rect.sizeDelta;
        sizeDelta.y = value * 50;
        rect.sizeDelta = sizeDelta;
    }
}
