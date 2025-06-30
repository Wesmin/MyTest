using System;
using UnityEngine;

namespace TS.Tools
{
    public class HandRayHit : MonoBehaviour
    {
        Ray ray;
        RaycastHit hit;
        bool isRay = false;
        public Transform Hand;
        public Action OnRayClick;
        private Transform hitTemp;
        public Transform HitTemp { get => hitTemp; set => hitTemp = value; }

        public static HandRayHit Instance;
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            XRCenterDeviceInput.Instance.handRight.EventHandler_Trigger_Click += RightTrigger_Click;
            XRCenterDeviceInput.Instance.handRight.EventHandler_TouchPad2DAxis_Changed += delegate { isRay = true; };
            XRCenterDeviceInput.Instance.handRight.EventHandler_TouchPad2DAxis_Release += delegate { isRay = false; };
        }
        private void RightTrigger_Click(object sender, XREventArgs e)
        {
            if (isRay)
            {
                Transform FaSheDian = XRCenterDeviceInput.Instance.handRight.transform.Find("Ray Interactor/[Ray Interactor] Ray Origin");
                ray = new Ray(FaSheDian.position, FaSheDian.forward);

                if (Physics.Raycast(ray, out hit, int.MaxValue))
                {
                    //Debug.Log(hit.transform.name);
                    if (hit.transform == HitTemp)
                    {
                        HitTemp.gameObject.TSHighlightHide();
                        Destroy(HitTemp.GetComponent<XRSimpleInteractable>());
                        HitTemp.Hide();
                        OnRayClick?.Invoke();
                        BaseConfig.NextStep();
                    }
                }
            }
        }
    }
    public static class RayHitExtend
    {
        public static void HandRay(this Transform temp, Action action = null)
        {
            DirectionDetection.Instance.target = temp;
            HandRayHit.Instance.HitTemp = temp;
            HandRayHit.Instance.HitTemp.Show();
            HandRayHit.Instance.HitTemp.GetAddComponent<XRSimpleInteractable>();
            HandRayHit.Instance.HitTemp.gameObject.TSHighlightShow();
            HandRayHit.Instance.OnRayClick = action;
        }
    }
}


