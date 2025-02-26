using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TrackWinLose : MonoBehaviour
{
    public float gameDuration; // 3 minutes
    private float timer;
    private bool gameEnded = false;
    private string winnerMessage = "";

    // Start is called before the first frame update
    void Start()
    {
        timer = gameDuration;
        StartCoroutine(GameTimer());
    }

    IEnumerator GameTimer()
    {
        while (timer > 0)
        {
            timer -= Time.deltaTime;

            // Update UI Timer
            // if (timerText != null)
            //     timerText.text = "Time Left: " + Mathf.Ceil(timer) + "s";
            yield return null;
        }
        EndGame();
    }

    void EndGame()
    {
        if (gameEnded) return;
        gameEnded = true;

        GameObject[] humans = GameObject.FindGameObjectsWithTag("Human");
        if (humans.Length > 0)
        {
            winnerMessage = "Humans Win! You guys are the last hope...";
        }
        else
        {
            winnerMessage = "Zombies Win! All humans are infected...\n";
        }
        PlayerPrefs.SetString("WinnerMessage", winnerMessage);
        SceneManager.LoadScene("ResultsScene");

        // Call methods to display winner details
        // DetermineWinners();
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
}
