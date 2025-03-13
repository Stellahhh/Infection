// Linda Fan, Stella Huo, Hanbei Zhou
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrackWinLose : MonoBehaviour
{
    public float gameDuration; // 3 minutes
    public float timer; // for testing
    private bool gameEnded = false;
    private string winnerMessage = "";
    private int humanCount = 0;


    // Track zombie awards
    private static ZombieController apexPredator; // Zombie with most infections
    private static ZombieController finalReaper; // Zombie who infected last human
    private static HumanController finalPrey; // Last human to be infected

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
            DetermineZombieWinners();

            EndGame();
            return true;
        }
        // if no zombies before time runs out
        else if (zombies.Length == 0)
        {
            winnerMessage = "Humans Win! You guys are the last hope...";
            humanCount = humans.Length;
            EndGame();
            return true;
        }

        return false;
    }

     void DetermineZombieWinners()
    {
        // Find the zombie with the most infections (Apex Predator)
        apexPredator = FindApexPredator();
        
        // Store last infecting zombie as "Final Reaper"
        finalReaper = ZombieController.lastCatcher;
        
        // Store last human infected as "Final Prey"
        finalPrey = HumanController.lastInfected;

        // Store values in PlayerPrefs for ResultsScene
        PlayerPrefs.SetString("ApexPredator", apexPredator != null ? apexPredator.name : "None");
        PlayerPrefs.SetString("FinalReaper", finalReaper != null ? finalReaper.name : "None");
        PlayerPrefs.SetString("FinalPrey", finalPrey != null ? finalPrey.name : "None");
    }

    ZombieController FindApexPredator()
    {
        ZombieController[] zombies = FindObjectsOfType<ZombieController>();
        ZombieController topZombie = null;
        int maxInfections = 0;

        foreach (ZombieController zombie in zombies)
        {
            if (zombie.infectionCount > maxInfections)
            {
                maxInfections = zombie.infectionCount;
                topZombie = zombie;
            }
        }
        return topZombie;
    }

    void DetermineWinnerOnTimeExpiry()
    {
        print("Time's up! Checking win conditions...");

        GameObject[] humans = GameObject.FindGameObjectsWithTag("Human");
        print("Humans left: " + humans.Length);

        // if there are still humans left after time runs out, humans win
        if (humans.Length > 0)
        {
            winnerMessage = "Time's up! Humans Win! You guys are the last hope...";
            humanCount = humans.Length;
        }

        EndGame();
    }

    void EndGame()
    {
        if (gameEnded) return;
        gameEnded = true;

        PlayerPrefs.SetString("WinnerMessage", winnerMessage);
        PlayerPrefs.SetInt("HumanCount", humanCount);

        // Clear zombie-related values if humans win
        if (!winnerMessage.Contains("Zombies Win"))
        {
            PlayerPrefs.DeleteKey("ApexPredator");
            PlayerPrefs.DeleteKey("FinalReaper");
            PlayerPrefs.DeleteKey("FinalPrey");
        }

        SceneManager.LoadScene("ResultsScene");
    }
}
