// Linda Fan <yfan43@jhu.edu>, Stella Huo <shuo2@jhu.edu>, Hanbei Zhou <hzhou43@jhu.edu>
// Create the result scene depending on the game ending result (zombie win or human win)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ResultsSceneManager : MonoBehaviour
{
    public Text winMessageText;
    public Text humanWinMessageText; // The message shown when humans win
    public Text apexPredatorText;
    public Text finalReaperText;
    public Text finalPreyText;
    
    public GameObject apexPredator;
    public GameObject finalReaper;
    public GameObject finalPrey;
    public GameObject background;
    public GameObject whiteBackground;
    public Button backButton;
    public CanvasGroup fadeCanvas; 

    public AudioSource audioSource;
    public AudioClip winClip;
    public AudioClip loseClip;

    void Start()
    {

        // enable cursor 
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        // disable background first 
        background.SetActive(false);
        whiteBackground.SetActive(true);
        if (backButton != null)
        {
            // change the text of the button to "Restart Game"
            backButton.GetComponentInChildren<Text>().text = "Restart Game";
            backButton.gameObject.SetActive(true); // Show back button
            backButton.onClick.AddListener(RestartGame);
        }
        // Fade in effect
        StartCoroutine(FadeInScene());

        // Display main winner message
        string winnerText = PlayerPrefs.GetString("WinnerMessage", "No results available.");
        string winner = PlayerPrefs.GetString("winner", "Draw");
        Debug.Log("Winner in start: " + winner);
        winMessageText.text = winnerText;
        humanWinMessageText.text = winnerText;

        // Get zombie-related names
        string apexPredatorName = PlayerPrefs.GetString("ApexPredator", "");
        string finalReaperName = PlayerPrefs.GetString("FinalReaper", "");
        string finalPreyName = PlayerPrefs.GetString("FinalPrey", "");

        // Display zombie-related names
        apexPredatorText.text = "Apex Predator: " + apexPredatorName;
        finalReaperText.text = "Final Reaper: " + finalReaperName;
        finalPreyText.text = "Final Prey: " + finalPreyName;

        // If Zombies win, show zombie characters
        if (winner.ToLower().Contains("zombie"))
        {
            DisplayWinMessage(winMessageText.text, Color.red);
            ShowZombieCharacters(true);
        }
        // If Humans win, disable everything except humanWinMessageText
        else if (winner.ToLower().Contains("human"))
        {
            ShowOnlyHumanWinMessage();
        }

        PlayResultAudio();
    }

    void DisplayWinMessage(string message, Color color)
    {
        winMessageText.text = message;
        winMessageText.color = color;
        StartCoroutine(AnimateWinMessage());
    }

    IEnumerator FadeInScene()
    {
        fadeCanvas.alpha = 0;
        while (fadeCanvas.alpha < 1)
        {
            fadeCanvas.alpha += Time.deltaTime * 0.5f;
            yield return null;
        }
    }

    IEnumerator AnimateWinMessage()
    {
        winMessageText.transform.localScale = Vector3.zero;
        float time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime;
            winMessageText.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, time);
            yield return null;
        }
    }

    void ShowZombieCharacters(bool isActive) {
    Debug.Log("Setting zombie character visibility: " + isActive);

    if (apexPredator != null)
    {
        apexPredator.SetActive(isActive);
        apexPredator.transform.SetParent(GameObject.Find("PrefabContainer").transform); // Attach to Canvas
        // apexPredator.transform.localPosition = new Vector3(-2, -1, 0); // Adjust X, Y, Z
        // apexPredator.transform.localScale = Vector3.one * 0.5f; // Scale it down
    }

    if (finalReaper != null)
    {
        finalReaper.SetActive(isActive);
        finalReaper.transform.SetParent(GameObject.Find("PrefabContainer").transform);
        // finalReaper.transform.localPosition = new Vector3(0, -1, 0); // Centered
        // finalReaper.transform.localScale = Vector3.one * 0.5f;
    }

    if (finalPrey != null)
    {
        finalPrey.SetActive(isActive);
        finalPrey.transform.SetParent(GameObject.Find("PrefabContainer").transform);
        // finalPrey.transform.localPosition = new Vector3(2, -1, 0); // Right side
        // finalPrey.transform.localScale = Vector3.one * 0.5f;
    }

    // Show/hide the corresponding text
    apexPredatorText?.gameObject.SetActive(isActive);
    finalReaperText?.gameObject.SetActive(isActive);
    finalPreyText?.gameObject.SetActive(isActive);
    humanWinMessageText?.gameObject.SetActive(!isActive);
}


    void ShowOnlyHumanWinMessage()
    {
        // Debug.Log("Hiding everything except humanWinMessageText");

        // Disable all other UI elements
        winMessageText.gameObject.SetActive(false);
        apexPredatorText.gameObject.SetActive(false);
        finalReaperText.gameObject.SetActive(false);
        finalPreyText.gameObject.SetActive(false);

        // Disable all zombie-related game objects
        apexPredator?.SetActive(false);
        finalReaper?.SetActive(false);
        finalPrey?.SetActive(false);

        // Show only humanWinMessageText
        humanWinMessageText.color = new Color(0.0f, 0.5f, 0.0f); // Dark Green (RGB: 0, 128, 0)
        humanWinMessageText.gameObject.SetActive(true);

        // int humanCount = PlayerPrefs.GetInt("HumanCount", 0);

    }

    void PlayResultAudio()
    {
        string winner = PlayerPrefs.GetString("winner", "");
        Debug.Log("Winner: " + winner);
        string playerRole = PlayerPrefs.GetString("PlayerRole", "");
        string playerName = PlayerPrefs.GetString("PlayerName", "");
        string finalPrey = PlayerPrefs.GetString("FinalPrey", "");

        bool isZombie = playerRole == "Zombie";
        bool isHuman = playerRole == "Human";

        if (playerName == finalPrey && winClip != null)
        {
            audioSource.clip = winClip; // Play special clip for final prey
        }
        else if (winner.ToLower().Contains("zombies"))
        {
            audioSource.clip = isZombie ? winClip : loseClip;
            if (isZombie)
            {
                Debug.Log("zombies win and you are a zombie, play win clip");
            }
            else
            {
                Debug.Log("zombies win and you are a human, play lose clip");
            }
        }
        else if (winner.ToLower().Contains("humans"))
        {
            audioSource.clip = isHuman ? winClip : loseClip;
            if (isHuman)
            {
                Debug.Log("humans win and you are a human, play win clip");
            }
            else
            {
                Debug.Log("humans win and you are a zombie, play lose clip");
            }
        }
        else
        {
            audioSource.clip = loseClip; 
        }

        audioSource.Play();
    }
    public void RestartGame() {
        Debug.Log("Restarting game...");
        Time.timeScale = 1;
        whiteBackground.SetActive(false);
        background.SetActive(true);
        SceneManager.LoadScene("SampleScene");
    }
}
