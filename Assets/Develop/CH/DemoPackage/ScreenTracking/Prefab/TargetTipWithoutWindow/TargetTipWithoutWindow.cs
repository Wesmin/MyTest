using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetTipWithoutWindow : MonoBehaviour
{
    public Transform TargetTransform;
    public Image img;
    public float offsetRight, offsetLeft, offsetUp, offsetDown;
    private void LateUpdate()
    {
        Method2();
    }

    private void Method2()
    {
        Transform camTransform = Camera.main.transform;

        var vFov = Camera.main.fieldOfView;
        var radHFov = 2 * Mathf.Atan(Mathf.Tan(vFov * Mathf.Deg2Rad / 2) * Camera.main.aspect);
        var hFov = Mathf.Rad2Deg * radHFov;
        if (TargetTransform==null)
        {
            return;
        }
        Vector3 deltaUnitVec = (TargetTransform.position - camTransform.position).normalized;

        float vdegobj = Vector3.Angle(Vector3.up, deltaUnitVec) - 90f;
        float vdegcam = Vector3.SignedAngle(Vector3.up, camTransform.forward, camTransform.right) - 90f;

        float vdeg = vdegobj - vdegcam;

        float hdeg = Vector3.SignedAngle(Vector3.ProjectOnPlane(camTransform.forward, Vector3.up), Vector3.ProjectOnPlane(deltaUnitVec, Vector3.up), Vector3.up);

        vdeg = Mathf.Clamp(vdeg, -89f, 89f);
        hdeg = Mathf.Clamp(hdeg, hFov * -0.5f, hFov * 0.5f);

        Vector3 projectedPos = Quaternion.AngleAxis(vdeg, camTransform.right) * Quaternion.AngleAxis(hdeg, camTransform.up) * camTransform.forward;
        Debug.DrawLine(camTransform.position, camTransform.position + projectedPos, Color.red);

        Vector3 newPos = Camera.main.WorldToScreenPoint(camTransform.position + projectedPos);
        if (newPos.x > Screen.width - offsetRight || newPos.x < offsetLeft || newPos.y > Screen.height - offsetUp || newPos.y < offsetDown)
            newPos = KClamp(newPos);
        else
        {
            img.gameObject.SetActive(false);
        }
        
        img.transform.position = newPos;
    }
    private Vector3 KClamp(Vector3 newPos)
    {
        img.gameObject.SetActive(true);
        Vector2 center = new Vector2(Screen.width / 2, Screen.height / 2);
        float k = (newPos.y - center.y) / (newPos.x - center.x);

        if (newPos.y - center.y > 0)
        {
            newPos.y = Screen.height - offsetUp;
            newPos.x = center.x + (newPos.y - center.y) / k;
        }
        else
        {
            newPos.y = offsetDown;
            newPos.x = center.x + (newPos.y - center.y) / k;
        }

        if (newPos.x > Screen.width - offsetRight)
        {
            newPos.x = Screen.width - offsetRight;
            newPos.y = center.y + (newPos.x - center.x) * k;
        }
        else if (newPos.x < offsetLeft)
        {
            newPos.x = offsetLeft;
            newPos.y = center.y + (newPos.x - center.x) * k;
        }

        return newPos;
    }
}
