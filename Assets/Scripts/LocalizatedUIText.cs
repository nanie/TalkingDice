using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalizatedUIText : MonoBehaviour
{

    public string localizatedString;
    public Text txt;

 

    void Start()
    {
        txt = GetComponent<Text>();

        txt.text = LanguageManager.Instance.GetText(localizatedString);
    }

    public void SetText(string text)
    {
        if(txt!=null)
        {
            txt.text = text;
        }
    }

}
