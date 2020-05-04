using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceBundle : MonoBehaviour
{

    public DiceRotationManager[] dice;

    public void SetRotation(int[] rotations)
    {
        foreach (var item in dice)
        {
            item.gameObject.SetActive(false);
        }

        for (int i = 0; i < rotations.Length; i++)
        {
            dice[i].gameObject.SetActive(true);
            dice[i].SetFaceRotation(rotations[i]);
        }
    }
}
