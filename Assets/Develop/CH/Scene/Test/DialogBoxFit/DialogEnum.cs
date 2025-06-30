using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogEnum : MonoBehaviour
{
    public Dialog CurrentDialogPeople = Dialog.监护人;
    public Transform LHand;
    public Transform RHand;
    public bool isSetName = false;

    private void OnValidate()
    {
        if (isSetName)
        {
            isSetName = false;
            transform.name= CurrentDialogPeople.ToString();

            
            Transform[] allTrans = this.gameObject.GetComponentsInChildren<Transform>(true);
            for (int i = 0; i < allTrans.Length; i++)
            {
                if(allTrans[i].gameObject.name== "lHand")
                {
                    LHand = allTrans[i].transform;
                }
                if(allTrans[i].gameObject.name == "rHand")
                {
                    RHand = allTrans[i].transform;
                }
            }
        }
    }
}
public enum Dialog
{
    //默认
    监护人=1,
    值班负责人 =2,
}
