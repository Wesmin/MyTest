using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogBoxFit : MonoBehaviour
{
    public int Words = 25;
    private TMP_Text info;
    private Button SkipBtn;
    private string currentInfo = "一二三四五上山打老虎老虎没打到打到小松鼠松鼠有几只";

    public string CurrentInfo { get => currentInfo; set => currentInfo = value; }

    private void Awake()
    {
        info = transform.Find("BG/Info").GetComponent<TMP_Text>();
        SkipBtn = transform.Find("Image/SkipBtn").GetComponent<Button>();
        SkipBtn.onClick.AddListener(() => 
        {
            SkipBtn.gameObject.SetActive(false);
            //AudioManager.Instance.StopAliText();
            //AliManager.Instance.StopCor();
            //OperationFlowManager.Instance.MoveToNextStep();
        });
    }

    private void Update()
    {
        if (info.text.Length < Words)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(info.transform.parent.GetComponent<RectTransform>());
            info.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            info.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.Unconstrained;
            info.transform.parent.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.Unconstrained;
        }
        else
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(info.transform.parent.GetComponent<RectTransform>());
            info.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            info.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            info.transform.parent.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        }

        

        //if (info.text.Length >= 100)
        //{
        //    transform.Find("占位1").SetActive(true);
        //    transform.Find("占位2").SetActive(true);
        //    SkipBtn.gameObject.SetActive(true);
        //}
        //else if( info.text.Length >= 50 && info.text.Length < 100)
        //{
        //    transform.Find("占位1").SetActive(true);
        //    transform.Find("占位2").SetActive(false);
        //    SkipBtn.gameObject.SetActive(true);
        //}
        //else
        //{
        //    SkipBtn.gameObject.SetActive(false);
        //    transform.Find("占位1").SetActive(false);
        //}
    }

    private void OnEnable()
    {
        StartCoroutine(ShowText());
    }
    IEnumerator ShowText()
    {
        for (int i = 0; i <= CurrentInfo.Length; i++)
        {
            info.text = CurrentInfo.Substring(0, i);
            if (CurrentInfo.Length >= 50)
            {
                yield return new WaitForSeconds(0.01f);
            }
            else
            {
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}
