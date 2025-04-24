// Linda Fan <yfan43@jhu.edu>, Stella Huo <shuo2@jhu.edu>, Hanbei Zhou <hzhou43@jhu.edu>
// Keep track of the the last human being infected.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class HumanController : NetworkBehaviour
{
    public static HumanController lastInfected;

}