using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {


    AudioSource audioSrc;
    AudioClip diceClip;
    [HideInInspector]
    public float diceClipTime;
    public string selectedVoice;
    public string[] voices;
    public string[] voiceLang;
    int voiceIndex = 0;
    int voiceSelectionIndex = 0;
    List<AudioClip> audioQeue;
    bool playing = false;

	void Awake () 
    {
        audioSrc = GetComponent<AudioSource>();
        diceClip = Resources.Load<AudioClip>("Audio/Dice");
        diceClipTime = diceClip.length;

        if(PlayerPrefs.GetFloat("rato666", 0) > 0)
        {
            var list = new List<string>(voices);
            list.Add("Edu");
            voices = list.ToArray();


            var list2 = new List<string>(voiceLang);
            list2.Add("pt");
            voiceLang = list2.ToArray();
        }

        selectedVoice = PlayerPrefs.GetString("selectedVoice", Application.systemLanguage == SystemLanguage.Portuguese? "Camilla": "Christopher");
      //  selectedVoice = "Camilla";
        for (int i = 0; i < voices.Length; i++)
        {
            if(voices[i] == selectedVoice)
            {
                voiceIndex = i;
                voiceSelectionIndex = i;
                LanguageManager.Instance.ChangeLanguage(voiceLang[voiceIndex]);
                break;
            }
        }
    }

    public float PlayAudio(string audioName)
    {
      
        audioSrc.Stop();

        AudioClip clip = Resources.Load<AudioClip>("Audio/" + selectedVoice + "/" + audioName);
        if(clip!=null)
        {
            audioSrc.PlayOneShot(clip);
            return clip.length;
        }
        else
        {
            Debug.Log("audio não encontrado - " + selectedVoice+"/"+ audioName);
            return 0;
        }

    }

    public float PlayAudio(string[] audioNames)
    {
        audioSrc.Stop();
        playing = true;
        audioQeue = new List<AudioClip>();

        float totalClipLenght = 0.0f;

        foreach (var item in audioNames)
        {
            AudioClip clip = Resources.Load<AudioClip>("Audio/" + selectedVoice + "/" + item);
            if(clip == null)
            {
                Debug.Log(item);
            }
            audioQeue.Add(clip);
            totalClipLenght += clip.length;
        }

      
        if (audioQeue.Count > 0)
        {
            var clip = audioQeue[0];
            audioSrc.PlayOneShot(clip);
            audioQeue.Remove(clip);
            StartCoroutine(PlayNextAudio(clip.length));
            return totalClipLenght;
        }
        else
        {
            Debug.Log("audios não encontrados!" + selectedVoice);
            return 0;
        }

    }

    IEnumerator PlayNextAudio(float time)
    {
        yield return new WaitForSeconds(time);
        if (audioQeue.Count > 0)
        {
            var clip = audioQeue[0];
            audioSrc.PlayOneShot(clip);
            audioQeue.Remove(clip);
            StartCoroutine(PlayNextAudio(clip.length));
        }
        else
        {
            playing = false;
        }
    }

    public void PlayDice()
    {
        audioSrc.PlayOneShot(diceClip);
    }

    public void SelectVoice()
    {
        voiceIndex = voiceSelectionIndex;
        PlayerPrefs.SetString("selectedVoice", voices[voiceIndex]);
        selectedVoice = voices[voiceIndex];
        LanguageManager.Instance.ChangeLanguage(voiceLang[voiceIndex]);
    }

    public float PlayNextVoice()
    {
        voiceSelectionIndex = (voiceSelectionIndex + 1) % voices.Length;
        return PlaySelectionVoice();
    }

    public float PlayPreviousVoice()
    {
        voiceSelectionIndex--;
        if(voiceSelectionIndex < 0)
        {
            voiceSelectionIndex = voices.Length - 1;
        }
        return PlaySelectionVoice();
    }

    private float PlaySelectionVoice()
    {
        audioSrc.Stop();
        audioSrc.clip = null;
        AudioClip clip = Resources.Load<AudioClip>("Audio/" + voices[voiceSelectionIndex] + "/" + voices[voiceSelectionIndex]);
        audioSrc.PlayOneShot(clip);
        return clip.length;
    }

    public bool IsPlaying()
    {
        return audioSrc.isPlaying && playing;
    }

    public void PlayMouse(int audioIndex)
    {

    }
}
