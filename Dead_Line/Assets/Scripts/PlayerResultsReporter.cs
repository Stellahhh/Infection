using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerResultsReporter : NetworkBehaviour
{
    private void Start()
    {
        if (isLocalPlayer)
        {
            // Save current player info locally
            PlayerPrefs.SetString("PlayerRole", gameObject.tag); // "Human" or "Zombie"
            PlayerPrefs.SetString("PlayerName", gameObject.name);
        }
    }
}
