using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Door_Interaction : MonoBehaviour
{
    public enum doorType { RotatingDoor = 0, SlidingDoor = 1 }
    public doorType doorMovementType;
    public bool open;
    public bool locked;
    public float rotateAngle = 90;
    public float speed = 1;
    public enum axis { X = 0, Y = 1, Z = 2 }
    public axis rotateAxis = axis.Y;
    public Vector3 startingPosition;
    public Vector3 endingPosition;
    public AudioClip startClip;
    public AudioClip endClip;
    private AudioSource sound;
    public Quaternion originRotation;
    public Quaternion openRotation;
    private float r;
    private bool opening;

    [Tooltip("激活测试")]
    public bool isActive = false;
    void Start()
    {
        InitRotation();
        sound = GetComponent<AudioSource>();
        r = 0;
    }
    public void InitRotation()
    {
        originRotation = transform.rotation;
        openRotation = originRotation;
        if (rotateAxis == axis.Y)
        {
            openRotation = Quaternion.Euler(new Vector3(originRotation.eulerAngles.x, originRotation.eulerAngles.y + rotateAngle, originRotation.eulerAngles.z));
        }
        else if (rotateAxis == axis.X)
        {
            openRotation = Quaternion.Euler(new Vector3(originRotation.eulerAngles.x + rotateAngle, originRotation.eulerAngles.y, originRotation.eulerAngles.z));
        }
        else if (rotateAxis == axis.Z)
        {
            openRotation = Quaternion.Euler(new Vector3(originRotation.eulerAngles.x, originRotation.eulerAngles.y, originRotation.eulerAngles.z + rotateAngle));
        }
    }

    void Update()
    {
        if (isActive)
        {
            isActive = false;
            if (opening == false && locked == false)
            {
                open = !open;
                r = 0;
                opening = true;
            }
        }
        if (opening)
        {
            ChangeState(open);
        }
    }
    void ChangeState(bool State)
    {
        Quaternion quaternion;
        Vector3 vector3;
        if (State)
        {
            quaternion = openRotation;
            vector3 = endingPosition;
            if (startClip!=null&& r==0)
            {
                sound.clip = startClip;
                sound.Play();
            }
            
        }
        else
        {
            quaternion = originRotation;
            vector3 = startingPosition;
            if (endClip != null && r == 0)
            {
                sound.clip = endClip;
                sound.Play(); 
            }
        }
        r += Time.deltaTime * speed;
        if (doorMovementType == doorType.RotatingDoor)
        {
            if (Quaternion.Angle(quaternion, transform.rotation) > 0.1F)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, r);
            }
            else
            {
                transform.rotation = quaternion;
                r = 0;
                opening = false;
            }
        }
        else if (doorMovementType == doorType.SlidingDoor)
        {
            if (Vector3.Distance(transform.localPosition, vector3) > 0.005F)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, vector3, r);
            }
            else
            {
                transform.localPosition = vector3;
                r = 0;
                opening = false;
            }
        }
    }
}
