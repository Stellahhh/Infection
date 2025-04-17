// Linda Fan <yfan43@jhu.edu>, Stella Huo <shuo2@jhu.edu>, Hanbei Zhou <hzhou43@jhu.edu>
// Collecting the current player status
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
