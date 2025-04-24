// Linda Fan <yfan43@jhu.edu>, Stella Huo <shuo2@jhu.edu>, Hanbei Zhou <hzhou43@jhu.edu>
// Tracking the status of the game in terms of winner and loser.
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
        string playerRole = PlayerPrefs.GetString("PlayerRole", "");

        // if both humans and zombies are eliminated before time runs out
        // possibly due to warzone
        if (humans.Length == 0 && zombies.Length == 0)
        {
            PlayerPrefs.SetString("winner", "Draw");
            winnerMessage = "It's a draw! Both sides have been eliminated...";
            EndGame();
            return true;
        }

        // if no humans before time runs out
        if (humans.Length == 0)
        {
            PlayerPrefs.SetString("winner", "Zombies");
            winnerMessage = "Zombies Win! All humans are infected...";
            DetermineZombieWinners();
            EndGame();
            return true;
        }
        // if no zombies before time runs out
        else if (zombies.Length == 0)
        {
            PlayerPrefs.SetString("winner", "Humans");
            if (playerRole == "Zombie")
            {
                // if player is a zombie, they lose
                winnerMessage = "Zombies Lose! All zombies are eliminated...";
            }
            else
            {
                // if player is a human, they win
                winnerMessage = "Humans Win! You guys are the last hope...";
            }
            
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
        Debug.Log("Apex Predator: " + (apexPredator != null ? apexPredator.name : "None"));
        
        // Store last infecting zombie as "Final Reaper"
        finalReaper = ZombieController.lastCatcher;
        
        // Store last human infected as "Final Prey"
        // finalPrey = HumanController.lastInfected;
        string finalPreyName = HumanController.lastInfectedName ?? "None";

        // Store values in PlayerPrefs for ResultsScene
        PlayerPrefs.SetString("ApexPredator", apexPredator != null ? apexPredator.name : "None");
        PlayerPrefs.SetString("FinalReaper", finalReaper != null ? finalReaper.name : "None");
        // PlayerPrefs.SetString("FinalPrey", finalPrey != null ? finalPrey.name : "None");
        PlayerPrefs.SetString("FinalPrey", finalPreyName);
        Debug.Log("Final Prey: " + finalPreyName);

    }

    ZombieController FindApexPredator()
    {
        ZombieController[] zombies = FindObjectsOfType<ZombieController>();
        ZombieController topZombie = null;
        int maxInfections = 0;
        Debug.Log("all zombies: " + zombies.Length);

        foreach (ZombieController zombie in zombies)
        {
            Debug.Log("zombie: " + zombie.name + " infection count: " + zombie.infectionCount);
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
        string playerRole = PlayerPrefs.GetString("PlayerRole", "");

        // if there are still humans left after time runs out, humans win
        if (humans.Length > 0)
        {
            PlayerPrefs.SetString("winner", "Humans");
            if (playerRole == "Zombie")
            {
                // if player is a zombie, they lose
                winnerMessage = "Time's up! Zombies Lose! Some humans survived...";
            }
            else
            {
                // if player is a human, they win
                winnerMessage = "Time's up! Humans Win! You guys are the last hope...";
            }
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
        if (!PlayerPrefs.GetString("winner").ToLower().Contains("zombies"))
        {
            PlayerPrefs.DeleteKey("ApexPredator");
            PlayerPrefs.DeleteKey("FinalReaper");
            PlayerPrefs.DeleteKey("FinalPrey");
        }
        SceneManager.LoadScene("ResultsScene");
    }
}
