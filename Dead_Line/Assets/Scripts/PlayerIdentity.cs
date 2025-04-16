using Mirror;
using UnityEngine;

public class PlayerIdentity : NetworkBehaviour
{
    [SyncVar] public string playerName;

    public override void OnStartLocalPlayer()
    {
        // Send PlayerPrefs name to the server once the local player is spawned
        string storedName = PlayerPrefs.GetString("PlayerName", "Player");
        CmdSetName(storedName);
    }

    [Command]
    void CmdSetName(string name)
    {
        playerName = name;
        if (gameObject.tag == "Zombie") {
            gameObject.name = $"Zombie ({name})";
        } else if (gameObject.tag == "Human") {
            gameObject.name = $"Human ({name})";
        }
    }
}
