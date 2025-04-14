using UnityEngine;
using System.Collections;

public class GrowlLooper : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] growlClips;
    
    [Tooltip("Extra delay after each growl clip (seconds)")]
    public float gapBetweenGrowls = 3.0f;

    [Tooltip("Add some random variation to the delay")]
    public float randomGapRange = 1.0f; // +/- range

    void Start()
    {
        StartCoroutine(PlayGrowlLoop());
    }

    IEnumerator PlayGrowlLoop()
    {
        while (true)
        {
            AudioClip growlClip = growlClips[Random.Range(0, growlClips.Length)];
            audioSource.PlayOneShot(growlClip);

            float randomDelay = Random.Range(
                gapBetweenGrowls - randomGapRange,
                gapBetweenGrowls + randomGapRange
            );

            // Wait for the clip to finish + delay
            yield return new WaitForSeconds(growlClip.length + Mathf.Max(0, randomDelay));
        }
    }
}
