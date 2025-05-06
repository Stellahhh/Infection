// Linda Fan <yfan43@jhu.edu>, Stella Huo <shuo2@jhu.edu>, Hanbei Zhou <hzhou43@jhu.edu>
// Managing hunger level of zombie
using UnityEngine;
using Mirror;

public class Hunger : NetworkBehaviour
{
    public float maxTime;
    [SyncVar] public float remainingTime;
    
    public UnityEngine.Events.UnityEvent onDeath;

    private bool isDead = false;

    private void Start()
    {
        if (isServer)
        {
            remainingTime = maxTime;
        }
    }

    void Update()
    {
        if (!isServer || isDead) return;

        remainingTime -= Time.deltaTime;

        if (remainingTime < 0)
        {
            isDead = true;
            onDeath.Invoke();
            NetworkServer.Destroy(gameObject); // only server can do this
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isServer) return;

        if (other.CompareTag("Human")) // Check if collided with a human
        {
            remainingTime = maxTime;
        }
    }
}
