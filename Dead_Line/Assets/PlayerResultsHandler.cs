using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerResultsHandler : NetworkBehaviour
{
    [TargetRpc]
    public void TargetLoadResultsScene(NetworkConnection target)
    {
        Debug.Log("Client received command to load ResultsScene.");
        SceneManager.LoadScene("ResultsScene");
    }
}
