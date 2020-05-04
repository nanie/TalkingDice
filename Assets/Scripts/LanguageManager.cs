using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class LanguageManager : MonoBehaviour
{

    public static LanguageManager Instance;
    string current;
    public Dictionary<string, localizedWord> dictionary = new Dictionary<string, localizedWord>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            Init();
        }
        else if(Instance!=this)
        {
            Destroy(gameObject);
        }
    }

    public void ChangeLanguage(string lang)
    {
        current = lang;

        var txts = FindObjectsOfType<LocalizatedUIText>();

        foreach (var item in txts)
        {
            item.SetText(GetText(item.localizatedString));
        }
    }

    private void Init()
    {
        string text = System.IO.File.ReadAllText("assets/resources/localization.csv");
        string[] lines = Regex.Split(text, "\r\n");

        for (int i = 0; i < lines.Length; i++)
        {
            string[] stringsOfLine = Regex.Split(lines[i], ";");
            if(stringsOfLine.Length >0 && stringsOfLine[0] != "" && !dictionary.ContainsKey(stringsOfLine[0]))
            {
                localizedWord wrd = new localizedWord();
                int wordIndex = 1;

                while (wordIndex < stringsOfLine.Length - 1)
                {
                    wrd.words.Add(stringsOfLine[wordIndex], stringsOfLine[wordIndex + 1]);
                    wordIndex++;
                }

                dictionary.Add(stringsOfLine[0], wrd);
            }

        }


    }

    public string GetText(string txt)
    {
        if(dictionary.ContainsKey(txt))
        {
            if(dictionary[txt].words.ContainsKey(current))
            {
                return dictionary[txt].words[current];
            }
            else
            {
               return dictionary[txt].words.Values.First();
            }
        }

        return "";
    }

}

public class localizedWord
{
    public Dictionary<string, string> words  = new Dictionary<string, string>();
}