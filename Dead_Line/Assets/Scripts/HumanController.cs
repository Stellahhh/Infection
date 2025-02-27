// Linda Fan, Stella Huo, Hanbei Zhou
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
