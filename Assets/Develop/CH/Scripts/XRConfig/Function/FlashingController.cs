using UnityEngine;
using System.Collections;
namespace TS.Tools 
{
    public class FlashingController : MonoBehaviour
    {
        public Color flashingStartColor = Color.cyan;
        public Color flashingEndColor = Color.red;
        public float flashingDelay = 0f;
        public float flashingFrequency = 1.5f;

        //private HighlightableObject ho;

        //private void Start()
        //{
        //    ho = gameObject.AddComponent<HighlightableObject>();
        //}
        //protected IEnumerator DelayFlashing()
        //{
        //    yield return new WaitForSeconds(flashingDelay);
        //    ho.FlashingOn(flashingStartColor, flashingEndColor, flashingFrequency);
        //}

        //public void ShowHight()
        //{
        //    StartCoroutine(DelayFlashing());
        //}
        //public void CloseHight()
        //{
        //    ho.FlashingOff();
        //}

        public void ShowHight(){}
        public void CloseHight(){}
    }
}