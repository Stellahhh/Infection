using UnityEngine;

public class FootStepController : MonoBehaviour
{
    public CharacterController controller;
    public AudioClip[] footstepClips;
    public float stepDistance = 2.0f;  // How far to walk before playing next footstep
    public float volume = 0.8f;

    public AudioSource audioSource;
    private Vector3 lastStepPosition;

    void Start()
    {
        lastStepPosition = transform.position;
    }

    void Update()
    {
        
        float moved = Vector3.Distance(transform.position, lastStepPosition);

        if (moved >= stepDistance && !audioSource.isPlaying && controller.isGrounded)
        {
            PlayFootstep();
            lastStepPosition = transform.position;
        }
    }

    void PlayFootstep()
    {
        print("footstep played");
        if (footstepClips.Length == 0) return;

        AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
        audioSource.PlayOneShot(clip, volume);
    }
}
