using Mirror;
using UnityEngine;

public class PlayerResultsHandler : NetworkBehaviour
{
    [TargetRpc]
    public void TargetLoadResultsScene(NetworkConnection target, string apex, string reaper, string prey, string winnerMsg, int humanCount)
    {
        PlayerPrefs.SetString("ApexPredator", apex);
        PlayerPrefs.SetString("FinalReaper", reaper);
        PlayerPrefs.SetString("FinalPrey", prey);
        PlayerPrefs.SetString("WinnerMessage", winnerMsg);
        PlayerPrefs.SetInt("HumanCount", humanCount);

        // Optionally store winner too
        PlayerPrefs.SetString("winner", winnerMsg.Contains("Zombies") ? "Zombies" : "Humans");

        UnityEngine.SceneManagement.SceneManager.LoadScene("ResultsScene");
    }
}
