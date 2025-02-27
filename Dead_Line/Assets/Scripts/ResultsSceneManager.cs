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

    string apexPredator = PlayerPrefs.GetString("ApexPredator", "");
    string finalReaper = PlayerPrefs.GetString("FinalReaper", "");
    string finalPrey = PlayerPrefs.GetString("FinalPrey", "");

    // Only display zombie-related text if they exist
    apexPredatorText.gameObject.SetActive(!string.IsNullOrEmpty(apexPredator));
    finalReaperText.gameObject.SetActive(!string.IsNullOrEmpty(finalReaper));
    finalPreyText.gameObject.SetActive(!string.IsNullOrEmpty(finalPrey));

    // Set text values
    if (!string.IsNullOrEmpty(apexPredator)) apexPredatorText.text = "Apex Predator: " + apexPredator;
    if (!string.IsNullOrEmpty(finalReaper)) finalReaperText.text = "Final Reaper: " + finalReaper;
    if (!string.IsNullOrEmpty(finalPrey)) finalPreyText.text = "Final Prey: " + finalPrey;
    }
}
