// Linda Fan <yfan43@jhu.edu>, Stella Huo <shuo2@jhu.edu>, Hanbei Zhou <hzhou43@jhu.edu>
// Counting the pray number for each zombie
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class ZombieController : NetworkBehaviour
{
    [SyncVar]
    public int infectionCount = 0;

    public static ZombieController lastCatcher;

    public void IncrementInfection()
    {
        infectionCount++;
        lastCatcher = this;
    }
}

