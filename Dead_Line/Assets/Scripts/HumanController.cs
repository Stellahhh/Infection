// Linda Fan <yfan43@jhu.edu>, Stella Huo <shuo2@jhu.edu>, Hanbei Zhou <hzhou43@jhu.edu>
// Keep track of the the last human being infected.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : MonoBehaviour
{
    public static HumanController lastInfected;

    public void BecomeZombie()
    {
        lastInfected = this; // Track last human to be infected
        gameObject.tag = "Zombie"; // Change to zombie
    }
}
