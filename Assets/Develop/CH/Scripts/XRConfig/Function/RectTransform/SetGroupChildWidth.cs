using UnityEngine;
using UnityEngine.UI;

namespace TS.Tools 
{
    public class SetGroupChildWidth : MonoBehaviour
    {
        /// <summary>
        /// 两边留白缩进宽度
        /// </summary>
        int sideswidth = 80;
        /// <summary>
        /// 父物体宽度
        /// </summary>
        float parentwidth;
        /// <summary>
        /// 子物体宽度
        /// </summary>
        float childwidth;
        /// <summary>
        /// 子物体数量
        /// </summary>
        int childcount;
        
        void Start()
        {
            //激活的子物体数量
            foreach (RectTransform item in transform)
            {
                if (item.IsActive())
                {
                    childcount++;
                }
            }

            parentwidth = transform.GetComponent<RectTransform>().sizeDelta.x - sideswidth;
            switch (childcount)
            {
                case 1:
                    childwidth = parentwidth / 2;
                    break;
                case 2:
                    transform.GetComponent<HorizontalLayoutGroup>().padding.left = sideswidth;
                    transform.GetComponent<HorizontalLayoutGroup>().padding.right = sideswidth;
                    childwidth = (parentwidth - sideswidth * 2) / childcount;
                    break;
                default:
                    childwidth = parentwidth / childcount;
                    break;
            }

            foreach (RectTransform item in transform)
            {
                item.sizeDelta = new Vector2(childwidth, item.sizeDelta.y);
            }
        }
    }
}