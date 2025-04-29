// Linda Fan <yfan43@jhu.edu>, Stella Huo <shuo2@jhu.edu>, Hanbei Zhou <hzhou43@jhu.edu>
// AI zombies that can automatically chasing humans
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    public NavMeshAgent agent;
    public Sight sightSensor;
    public Animator animator;
    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponentInParent<NavMeshAgent>();
    }

    void Update()
    {
        ChasePlayer();
    }

    void ChasePlayer() {

        if (sightSensor.detectedObject != null) 
            {
                agent.isStopped = false;
                agent.SetDestination(sightSensor.detectedObject.transform.position); 
                
                print("Chasing Player: " + sightSensor.detectedObject.transform.position);
                animator.SetBool("isWalking", true);  // Trigger walk animation
                //print("Agent Velocity: " + agent.speed);
            }
        else {
            agent.isStopped = true; 
            animator.SetBool("isWalking", false);  // Trigger walk animation
            //print("Player not detected");
        }
    }

    
    // private void OnTriggerEnter(Collider other)
    // {
    //     print("colliding with: " + other.gameObject.name);
    //     if (other.gameObject.CompareTag("Player")) // Ensure the collider belongs to the player
    //     {
    //         agent.isStopped = true; // Stop movement
    //     }
    // }
}
