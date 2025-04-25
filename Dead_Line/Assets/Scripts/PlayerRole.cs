using UnityEngine;
using Mirror;

public class PlayerRole : NetworkBehaviour
{
    [SyncVar]
    public string role; // "Human" or "Zombie"

    [SyncVar]
    public string playerName; // Custom player name

    public override void OnStartLocalPlayer()
    {
        // Send name from PlayerPrefs to the server
        string nameFromPrefs = PlayerPrefs.GetString("PlayerName", $"Player_{netId}");
        CmdSetPlayerName(nameFromPrefs);

        // Set PlayerPrefs for local access
        PlayerPrefs.SetString("PlayerRole", role);
    }

    [Command]
    void CmdSetPlayerName(string name)
    {
        playerName = name;
    }
}
