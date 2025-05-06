using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterController), typeof(NavMeshAgent))]
public class NavMeshDropper : MonoBehaviour
{
    public float gravity = -9.81f;
    public float groundCheckDistance = 0.1f;

    private CharacterController characterController;
    private NavMeshAgent agent;
    private Vector3 velocity;
    private bool isFalling = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();

        agent.enabled = false; // disable NavMeshAgent until grounded
    }

    void Update()
    {
        if (isFalling)
        {
            velocity.y += gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);

            if (IsGrounded() && OnNavMesh())
            {
                isFalling = false;
                velocity = Vector3.zero;

                // Snap to NavMesh and switch to NavMeshAgent
                agent.enabled = true;
                characterController.enabled = false;
                agent.Warp(transform.position);
            }
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance + 0.1f);
    }

    bool OnNavMesh()
    {
        NavMeshHit hit;
        return NavMesh.SamplePosition(transform.position, out hit, 1.0f, NavMesh.AllAreas);
    }
}
