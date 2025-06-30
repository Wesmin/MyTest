
using System;
using UnityEngine;
public class XREventArgs : EventArgs
{
    public float ValueFloat { get; set; }
    public bool ValueBool { get; set; }
    public Vector2 ValueVec2 { get; set; }
    public Vector3 ValueVec3 { get; set; }
    public Quaternion ValueQuaternion { get; set; }
}
