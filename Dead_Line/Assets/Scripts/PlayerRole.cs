using UnityEngine;
using Mirror;

public class PlayerRole : NetworkBehaviour
{
    [SyncVar]
    public string role; // "Human" or "Zombie"

    public override void OnStartLocalPlayer()
    {
        // Set PlayerPrefs locally so UI can access role
        PlayerPrefs.SetString("PlayerRole", role);
    }
}
