// Linda Fan <yfan43@jhu.edu>, Stella Huo <shuo2@jhu.edu>, Hanbei Zhou <hzhou43@jhu.edu>
// Managing the death of human when its life become negative
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror; 

public class Life : NetworkBehaviour
{
    public float amount;
    public UnityEngine.Events.UnityEvent onDeath;
    public GameObject observerPrefab;
    private bool isDead = false;

    void Update()
    {
        if (!isDead && amount <= 0)
        {
            isDead = true;
            onDeath.Invoke();

            if (isLocalPlayer)
            {
                CmdHandleDeath();
            }
        }
    }

    [Command]
    void CmdHandleDeath()
    {
        EnterObserverMode();
    }

    void EnterObserverMode()
    {
        if (observerPrefab != null)
        {
            GameObject observer = Instantiate(observerPrefab);
            NetworkServer.Spawn(observer, connectionToClient);

            var obsScript = observer.GetComponent<ObserverMode>();
            if (obsScript != null)
            {
                obsScript.SetCameras(DynamicMapGenerator.Instance.tileCameras.ToArray());
                obsScript.EnableObservation();
            }
        }

        NetworkServer.Destroy(gameObject);
    }
}
