using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceAnimationManager : MonoBehaviour
{
    public DiceRotationManager[] dices;
    public DiceBundle[] dice;
    public Animator anim;

   

    public void RollDice(int sides, int[] values)
    {

        foreach (var item in dice)
        {
            item.gameObject.SetActive(false);
        }

        switch (sides)
        {
            case 4:
                dice[0].gameObject.SetActive(true);
                dice[0].SetRotation(values);

                break;

            case 6:
                dice[1].gameObject.SetActive(true);
                dice[1].SetRotation(values);
                break;

            case 8:
                dice[2].gameObject.SetActive(true);
                dice[2].SetRotation(values);
                break;

            case 10:
                dice[3].gameObject.SetActive(true);
                dice[3].SetRotation(values);
                break;

            case 12:
                dice[4].gameObject.SetActive(true);
                dice[4].SetRotation(values);
                break;

            case 20:
                dice[5].gameObject.SetActive(true);
                dice[5].SetRotation(values);
                break;

            case 100:
                dice[6].gameObject.SetActive(true);
                dice[6].SetRotation(new int[] { Mathf.FloorToInt(values[0] / 10) , values[0] % 10 == 0 ? 10 : values[0] % 10 });
                break;

            case 3:
                dice[7].gameObject.SetActive(true);
                break;
        }

        anim.SetTrigger("Roll");
    }

}
