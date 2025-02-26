using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrackWinLose : MonoBehaviour
{
    public float gameDuration = 180f; // 3 minutes
    private float timer;
    private bool gameEnded = false;
    private string winnerMessage = "";

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
            yield return null;

            // check win conditions every 2 seconds 
            if (timer % 2 < Time.deltaTime)
            {
                if (CheckWinConditions())
                {
                    yield break; // Exit if game has ended
                }
            }
        }
        EndGame(); // If timer reaches 0, determine winner
    }

    bool CheckWinConditions()
    {
        GameObject[] humans = GameObject.FindGameObjectsWithTag("Human");
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");

        if (humans.Length == 0)
        {
            winnerMessage = "Zombies Win! All humans are infected...";
            EndGame();
            return true;
        }
        else if (zombies.Length == 0)
        {
            winnerMessage = "Humans Win! You guys are the last hope...";
            EndGame();
            return true;
        }

        return false;
    }

    void EndGame()
    {
        if (gameEnded) return;
        gameEnded = true;

        PlayerPrefs.SetString("WinnerMessage", winnerMessage);
        SceneManager.LoadScene("ResultsScene");
    }
}

