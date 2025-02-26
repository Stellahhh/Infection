using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class ZombieController : NetworkBehaviour
{
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        Debug.Log("Now controlling newZombie!");

        // Enable the camera and movement for the newZombie
        Camera myCam = GetComponentInChildren<Camera>();
        if (myCam != null)
        {
            myCam.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("No camera found for the new zombie!");
        }
    }
}

