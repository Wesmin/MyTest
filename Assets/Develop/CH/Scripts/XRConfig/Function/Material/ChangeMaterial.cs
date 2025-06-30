using UnityEngine;
using UnityEngine.UI;
namespace TS.Tools
{
    public class ChangeMaterial : MonoBehaviour
    {
        //public Material material;
        private void Awake()
        {
            Material material = Resources.Load<Material>("Material/Transparent");
            Image[] Image_materials;
            Text[] Text_materials;
            Image_materials = transform.GetComponentsInChildren<Image>(true);
            Text_materials = transform.GetComponentsInChildren<Text>(true);
            for (int i = 0; i < Image_materials.Length; i++)
            {
                Image_materials[i].material = material;
            }
            for (int i = 0; i < Text_materials.Length; i++)
            {
                Text_materials[i].material = material;
            }
        }
    }
}

