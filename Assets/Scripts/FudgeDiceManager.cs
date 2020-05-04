using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FudgeDiceManager : MonoBehaviour
{
    public DiceRotationManager[] dices;

    public void SetDiceRotation(int[] values)
    {
        for (int i = 0; i < 4; i++)
        {
            dices[i].SetFaceRotation(values[i]);
        }
    }
}
