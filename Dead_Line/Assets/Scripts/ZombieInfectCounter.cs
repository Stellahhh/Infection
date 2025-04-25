using UnityEngine;
using Mirror;

public class ZombieInfectCounter : NetworkBehaviour
{
    private ZombieController zombieController;

    void Start()
    {
        zombieController = GetComponent<ZombieController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("isServer: " + isServer);
        if (!isServer) return; // Server handles infection count

        if (other.gameObject.layer == LayerMask.NameToLayer("Human"))
        {
            Debug.Log($"{gameObject.name} (Zombie) collided with Human {other.gameObject.name}");

            if (zombieController != null)
            {
                zombieController.IncrementInfection();
                Debug.Log($"{gameObject.name} infection count is now {zombieController.infectionCount}");
            }
        }
    }
}
