using System.Collections.Generic;
using UnityEngine;

namespace TS.Tools
{
    public class AnimationFit : MonoBehaviour
    {
        public bool copy = false;
        public bool paste = false;
        public List<Location> locations = new List<Location>();

        private void OnValidate()
        {
            if (copy)
            {
                CopyInfo();
                copy = false;
            }

            if (paste)
            {
                PasteInfo();
                paste = false;
                locations.Clear();
            }
        }

        public void CopyInfo()
        {
            locations.Clear();

            foreach (var item in GetComponentsInChildren<Transform>())
            {
                SkinnedMeshRenderer smr = item.GetComponent<SkinnedMeshRenderer>();
                locations.Add(new Location(item.name, item.position, item.rotation, smr));
            }

            Debug.Log("<color=yellow>Copy Success</color>");
        }

        public string PasteInfo()
        {
            Transform[] transforms = GetComponentsInChildren<Transform>();

            if (transforms.Length != locations.Count)
            {
                return "The number of child objects is not equal! Stop the assignment!";
            }

            for (int i = 0; i < transforms.Length; i++)
            {
                if (transforms[i].name == locations[i].name)
                {
                    transforms[i].position = locations[i].position;
                    transforms[i].rotation = locations[i].rotation;

                    // Restore blendshapes if any
                    SkinnedMeshRenderer smr = transforms[i].GetComponent<SkinnedMeshRenderer>();
                    BlendShapeInfo blendInfo = locations[i].blendShapeInfo;

                    if (smr != null && blendInfo != null && smr.sharedMesh != null)
                    {
                        foreach (var kvp in blendInfo.blendShapeValues)
                        {
                            int index = smr.sharedMesh.GetBlendShapeIndex(kvp.Key);
                            if (index >= 0)
                            {
                                smr.SetBlendShapeWeight(index, kvp.Value);
                            }
                        }
                    }
                }
                else
                {
                    return $"Child {locations[i].name} has changed! Stop paste!";
                }
            }

            Debug.Log("<color=yellow>Paste Success</color>");
            return "AnimationFit Success!";
        }

        [System.Serializable]
        public class Location
        {
            public string name;
            public Vector3 position;
            public Quaternion rotation;
            public BlendShapeInfo blendShapeInfo;

            public Location(string name, Vector3 position, Quaternion rotation, SkinnedMeshRenderer smr = null)
            {
                this.name = name;
                this.position = position;
                this.rotation = rotation;

                if (smr != null && smr.sharedMesh != null && smr.sharedMesh.blendShapeCount > 0)
                {
                    blendShapeInfo = new BlendShapeInfo(smr);
                }
            }
        }

        [System.Serializable]
        public class BlendShapeInfo
        {
            public Dictionary<string, float> blendShapeValues = new Dictionary<string, float>();

            public BlendShapeInfo(SkinnedMeshRenderer smr)
            {
                Mesh mesh = smr.sharedMesh;
                for (int i = 0; i < mesh.blendShapeCount; i++)
                {
                    string shapeName = mesh.GetBlendShapeName(i);
                    float value = smr.GetBlendShapeWeight(i);
                    blendShapeValues[shapeName] = value;
                }
            }
        }
    }
}
