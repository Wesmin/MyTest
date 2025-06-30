using UnityEngine;
namespace TS.Tools
{
    public class MaterialFit : MonoBehaviour
    {
        private void Awake()
        {
            Material material = Resources.Load<Material>("Material/Transparent");

            foreach (MeshRenderer it in transform.GetComponentsInChildren<MeshRenderer>(true))
            {
                Material[] newMaterials = new Material[it.materials.Length];

                for (int i = 0; i < it.materials.Length; i++)
                {
                    //直接赋值目标材质，而不是创建新的材质实例
                    newMaterials[i] = material;
                }

                //将新的材质数组赋值给 MeshRenderer
                it.materials = newMaterials;

                Debug.Log($"{it.name} has been replaced with {material.name}");
            }
        }
    }
}