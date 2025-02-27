using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsSceneManager : MonoBehaviour
{
    public Text winnerText;
    public Text apexPredatorText;
    public Text finalReaperText;
    public Text finalPreyText;

    void Start()
    {
        // Display main winner message
        winnerText.text = PlayerPrefs.GetString("WinnerMessage", "No results available.");

        // Display special zombie titles
        apexPredatorText.text = "Apex Predator: " + PlayerPrefs.GetString("ApexPredator", "None");
        finalReaperText.text = "Final Reaper: " + PlayerPrefs.GetString("FinalReaper", "None");
        finalPreyText.text = "Final Prey: " + PlayerPrefs.GetString("FinalPrey", "None");
    }
}
