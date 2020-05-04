using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceManager : MonoBehaviour {

    public int[] diceSizes;
    public string[] options;
    public string[] voices;
    public Text txtDiceSize;
    public Text txtDiceRoll;
    public AudioManager audioManager;
    public DiceAnimationManager anim;
    public  FudgeDiceManager fudgeDice;
    public float animationDelay = 2.0f;
    public GameObject tutorial;
    int diceIndex;
    int optionIndex;
    bool VoiceSelectionOpen;
    bool ConfigOpen = false;
    bool started;
    int sixCount;
    int diceCount = 1;
    bool rolling;

    private void Awake()
    {
        SwipeDetector.OnSwipe += SwipeDetector_OnSwipe;
        SwipeDetector.OnClick += SwipeDetector_OnClick;
 
    }

    private void Start()
    {
        var time = audioManager.PlayAudio("Tutorial");
        Destroy(tutorial, time);
        Invoke("SetDiceSize", time);
    }

    private void Update()
    {
        if (started && !audioManager.IsPlaying())
        {

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SwipeLeft();
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                SwipeRight();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Clicked();
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                SwipeUp();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                SwipeDown();
            }
        }

     
    }

    public void Clicked()
    {
        if (started && !audioManager.IsPlaying() && !rolling)
            SwipeDetector_OnClick();
    }

    void SwipeDetector_OnClick()
    {
        if (!started)
            return;

        if (VoiceSelectionOpen)
        {
            audioManager.SelectVoice();
            VoiceSelectionOpen = false;
            CloseAfterSeconds();
        }
        else if (ConfigOpen)
        {
            switch(options[optionIndex])
            {
                case "close":
                    CloseAfterSeconds();
                    break;

                case "credits":
                    var time = audioManager.PlayAudio("creditsFull");
                    Invoke("CloseAfterSeconds", time);
                    break;

                case "change":
                    VoiceSelectionOpen = true;
                    audioManager.PlayNextVoice();
                    break;
            }
        }
        else
        {
            if (diceSizes[diceIndex] == 0)
            {
                optionIndex = 0;
                ConfigOpen = true;
                PlayOption();
            }
            else if(diceSizes[diceIndex] == 3)
            {
                RollFudgeDice();
            }
            else
            {
                RollDice();
            }
        }

    }

    private void CloseAfterSeconds()
    {
        VoiceSelectionOpen = false;
        ConfigOpen = false;
        SwipeRight();
    }

    private void PlayOption()
    {
        txtDiceSize.text = options[optionIndex] == "close" ? LanguageManager.Instance.GetText("close") : 
                           options[optionIndex] == "credits" ? LanguageManager.Instance.GetText("credits")
                                                 : LanguageManager.Instance.GetText("changeVoice");
        audioManager.PlayAudio(options[optionIndex]);
    }

    private void SwipeDetector_OnSwipe(SwipeData data)
    {
        if (!started)
            return;
        if (data.Direction == SwipeDirection.Left)
        {
            SwipeLeft();
        }
        else if (data.Direction == SwipeDirection.Right)
        {
            SwipeRight();
        }
        else if (data.Direction == SwipeDirection.Up)
        {
            SwipeUp();
        }
        else
        {
            SwipeDown();
        }
    }

    private void SwipeDown()
    {
        if (diceSizes[diceIndex] == 0 || diceSizes[diceIndex] == 3 || diceSizes[diceIndex] == 100)
            return;
        diceCount = diceCount - 1;
        diceCount = diceCount < 0 ? 5 : diceCount;
        SetDiceSize();
    }

    private void SwipeUp()
    {
        if (diceSizes[diceIndex] == 0 || diceSizes[diceIndex] == 3 || diceSizes[diceIndex] == 100)
            return;
        diceCount = diceCount + 1;
        diceCount = diceCount > 5 ? 1 : diceCount;
        
        SetDiceSize();
    }

    private void SwipeLeft()
    {
        diceCount = 1;
        if (VoiceSelectionOpen)
        {
            audioManager.PlayNextVoice();
        }
        else if (ConfigOpen)
        {
            optionIndex = (optionIndex + 1) % options.Length;
            PlayOption();
        }
        else
        {   
            diceIndex = (diceIndex + 1) % diceSizes.Length;
            SetDiceSize();
        }
    }

    private void SwipeRight()
    {
        diceCount = 1;
        if (VoiceSelectionOpen)
        {
            audioManager.PlayPreviousVoice();
        }
        else if (ConfigOpen)
        {
            optionIndex = optionIndex == 0 ? options.Length - 1 : optionIndex - 1;
            PlayOption();
        }
        else
        {
            diceIndex = diceIndex == 0 ? diceSizes.Length - 1 : diceIndex - 1;
            SetDiceSize();
        }
    }

    void RollDice()
    {
        rolling = true;
        fudgeDice.gameObject.SetActive(false);
        audioManager.PlayDice();


        var diceValues = new int[diceCount];
        for (int i = 0; i < diceCount; i++)
        {
            diceValues[i] = UnityEngine.Random.Range(1, diceSizes[diceIndex] + 1);
        }


        anim.RollDice(diceSizes[diceIndex], diceValues);
        StartCoroutine(SetDiceRoll(animationDelay, diceValues));
    }

    void RollFudgeDice()
    {
        rolling = true;
        audioManager.PlayDice();
        var values = new int[4];
        var val = 0;
        for (int i = 0; i < 4; i++)
        {
            values[i] = UnityEngine.Random.Range(1, 7);
            val += values[i] == 5 || values[i] == 6 ? 1 :
                   values[i] == 2 || values[i] == 4 ? -1 : 0;
        }

        fudgeDice.gameObject.SetActive(true);
        fudgeDice.SetDiceRotation(values);

        anim.RollDice(3, new int[]{val});
        StartCoroutine(SetFudgeDiceRoll(animationDelay, val));
    }

    IEnumerator SetDiceRoll(float timer, int[] val)
    {
        yield return new WaitForSeconds(timer);

       // txtDiceRoll.text = val.ToString();
        if(val.Length >1)
        {
            string[] values = new string[val.Length + 2];
            int totalValue = 0;

            for (int i = 0; i < val.Length; i++)
            {
                totalValue += val[i];
                values[i] = val[i].ToString();
            }

            values[val.Length] = "total";
            values[val.Length + 1] = totalValue.ToString();

            audioManager.PlayAudio(values);
        }
        else
        {
            audioManager.PlayAudio(val[0].ToString());
        }
        rolling = false;
    }

    int CheckMouse(int val)
    {
        return 0;

        //TODO ativar o mouse quando o audio estiver pronto
        if (PlayerPrefs.GetFloat("rato666", 0) < 1)
        {
            int rollCount = PlayerPrefs.GetInt("rollCount", 0);
            rollCount++;
            PlayerPrefs.SetInt("rollCount", rollCount);

            if(rollCount == 666)
            {
                PlayerPrefs.SetFloat("rato666", 1);
                return 1;
            }

            sixCount = val == 6 ? sixCount + 1 : 0;
            if (sixCount == 3)
            {
                PlayerPrefs.SetFloat("rato666", 1);
                return 2;
            }
        }
        return 0;
    }

    IEnumerator SetFudgeDiceRoll(float timer, int val)
    {
        yield return new WaitForSeconds(timer);
        txtDiceRoll.text = val.ToString();

        var audioname = val > 0 ? "pls" : val < 0 ? "min" : "zero";
        if(val!=0)
        {
            audioname = audioname + Mathf.Abs(val).ToString();
        }

        audioManager.PlayAudio(audioname);

        rolling = false;
    }

    void SetDiceSize()
    {
        started = true;

        if (diceSizes[diceIndex] == 0)
        {
            txtDiceSize.text = LanguageManager.Instance.GetText("config");
            audioManager.PlayAudio("Settings");
        }
        else if (diceSizes[diceIndex] == 3)
        {
            txtDiceSize.text = "Fate";
            audioManager.PlayAudio("fate");
        }
        else
        {
            if(diceCount > 1)
            {
                txtDiceSize.text = diceCount + "D" + diceSizes[diceIndex];
                audioManager.PlayAudio(new string[] {diceCount.ToString(), "D" + diceSizes[diceIndex] });
            }
            else
            {
                txtDiceSize.text = "D" + diceSizes[diceIndex];
                audioManager.PlayAudio("D" + diceSizes[diceIndex]);
            }

        }
     
    }
}
