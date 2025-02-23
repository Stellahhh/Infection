using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    private NavMeshAgent agent;
    public Sight sightSensor;
    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponentInParent<NavMeshAgent>();
    }

    void Update()
    {
        ChasePlayer();
    }

   void ChasePlayer()
    {
        if (sightSensor.detectedObject != null) 
        {
            agent.isStopped = false;
            agent.SetDestination(sightSensor.detectedObject.transform.position); // Continuously update position
            print("Chasing player");
        }
        else 
        {
            agent.isStopped = true; // Stop if the player is not detected
            print("Player not detected");
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        print("colliding with: " + other.gameObject.name);
        if (other.gameObject.CompareTag("Player")) // Ensure the collider belongs to the player
        {
            agent.isStopped = true; // Stop movement
        }
    }
}
