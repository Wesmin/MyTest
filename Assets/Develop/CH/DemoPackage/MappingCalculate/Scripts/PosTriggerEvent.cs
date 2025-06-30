using UnityEngine;
using UnityEngine.Events;

public class PosTriggerEvent : MonoBehaviour
{
    public UnityEvent PosB;
    public UnityEvent PosA;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Pos_Right")
        {           
            PosB.Invoke();
        }
        if (other.name == "Pos_Left")
        {
            PosA.Invoke();
        }
    }
}
