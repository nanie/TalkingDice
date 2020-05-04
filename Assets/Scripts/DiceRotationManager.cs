using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRotationManager : MonoBehaviour
{
    public Vector3[] rotations;
    public int value;

    public void SetFaceRotation(int value)
    {
        this.value = value;
        if(value -1 < 0)
        {
            value = rotations.Length;
        }
        transform.rotation = Quaternion.Euler(rotations[value -1]);
    }
}
