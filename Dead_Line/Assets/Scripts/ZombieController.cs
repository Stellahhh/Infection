using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class ZombieController : NetworkBehaviour
{
    // public override void OnStartLocalPlayer()
    // {
    //     base.OnStartLocalPlayer();
    //     Debug.Log("Now controlling newZombie!");

    //     // Enable the camera and movement for the newZombie
    //     Camera myCam = GetComponentInChildren<Camera>();
    //     if (myCam != null)
    //     {
    //         myCam.gameObject.SetActive(true);
    //     }
    //     else
    //     {
    //         Debug.LogError("No camera found for the new zombie!");
    //     }
    // }
    public static ZombieController lastCatcher;
    public int infectionCount = 0;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Human")) // Check if colliding object is a human
        {
            print("Zombie collided with a human!");
            HumanController human = collision.gameObject.GetComponent<HumanController>();
            if (human != null)
            {
                InfectHuman(human);
            }
        }
    }

    void InfectHuman(HumanController human)
    {
        human.BecomeZombie(); // Convert human into a zombie
        infectionCount++; // Increase this zombie's infection count
        lastCatcher = this; // Store last zombie that infected a human
    }
}

