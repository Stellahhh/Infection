using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsSceneManager : MonoBehaviour
{
    public Text winnerText;

    void Start()
    {
        if (winnerText != null)
        {
            winnerText.text = PlayerPrefs.GetString("WinnerMessage", "No results available.");
        }
    }  
}
