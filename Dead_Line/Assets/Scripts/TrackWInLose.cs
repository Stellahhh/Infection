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
                    yield break; // Exit coroutine if game has ended
                }
            }
        }

        // If time runs out, determine winner
        DetermineWinnerOnTimeExpiry();
    }

    bool CheckWinConditions()
    {
        GameObject[] humans = GameObject.FindGameObjectsWithTag("Human");
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");
        
        // if both humans and zombies are eliminated before time runs out
        // possibly due to warzone
        if (humans.Length == 0 && zombies.Length == 0)
        {
            winnerMessage = "It's a draw! Both sides have been eliminated...";
            EndGame();
            return true;
        }

        // if no humans before time runs out
        if (humans.Length == 0)
        {
            winnerMessage = "Zombies Win! All humans are infected...";
            EndGame();
            return true;
        }
        // if no zombies before time runs out
        else if (zombies.Length == 0)
        {
            winnerMessage = "Humans Win! You guys are the last hope...";
            EndGame();
            return true;
        }

        return false;
    }

    void DetermineWinnerOnTimeExpiry()
    {
        if (gameEnded) return;
        gameEnded = true;

        GameObject[] humans = GameObject.FindGameObjectsWithTag("Human");

        // if there are still humans left after time runs out, humans win
        if (humans.Length > 0)
        {
            winnerMessage = "Time's up! Humans Win! You guys are the last hope...";
        }

        EndGame();
    }

    void EndGame()
    {
        if (gameEnded) return;
        gameEnded = true;

        PlayerPrefs.SetString("WinnerMessage", winnerMessage);
        SceneManager.LoadScene("ResultsScene");
    }
}
