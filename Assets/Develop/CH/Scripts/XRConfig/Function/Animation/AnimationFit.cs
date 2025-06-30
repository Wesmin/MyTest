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
            }
        }
        public void CopyInfo()
        {
            locations.Clear();
            foreach (var item in GetComponentsInChildren<Transform>())
            {
                locations.Add(new Location(item.name, item.position, item.rotation));
            }
            Debug.Log(string.Format("<color=yellow>{0}</color>", $"Copy Success"));
        }
        public string PasteInfo()
        {
            Transform[] transforms = GetComponentsInChildren<Transform>();
            if (transforms.Length != locations.Count)
            {
                ///子物体数量不对等！停止赋值！
                return $"The number of child objects is not equal! Stop the assignment!";
            }
            for (int i = 0; i < transforms.Length; i++)
            {
                if (transforms[i].name == locations[i].name)
                {
                    transforms[i].position = locations[i].position;
                    transforms[i].rotation = locations[i].rotation;
                }
                else
                {
                    ///子物体XX发生改变！停止赋值！
                    return $"Child{locations[i].name}has changed! Stop paste! ";
                }
            }
            Debug.Log(string.Format("<color=yellow>{0}</color>", $"Paste Success"));
            return "AnimationFit Success!";
        }
        public class Location
        {
            public string name;
            public Vector3 position;
            public Quaternion rotation;

            public Location(string name, Vector3 position, Quaternion rotation)
            {
                this.name = name;
                this.position = position;
                this.rotation = rotation;
            }
        }
    }

}
