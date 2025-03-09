using UnityEngine;
using Mirror;
using UnityEngine.InputSystem; // Import New Input System
using System.Collections;
using UnityEngine.UI; // For Image reference

public class PlayerSwitch : NetworkBehaviour
{
    public GameObject prefabA;
    public GameObject prefabB;
    public AudioSource audioSource;
    public AudioClip soundEffect;
    public Image screenOverlay; // Reference to UI Image for red screen effect

    // Coroutine to handle delayed switch and sound effect
    IEnumerator DelayedSwitch()
    {
        // If this is the local player, start the fade effect
        if (isLocalPlayer)
        {
            // Fade to red
            yield return StartCoroutine(FadeScreen(Color.clear, new Color(1, 0, 0, 0.6f), 1.0f));

            // Play the sound effect
            audioSource.PlayOneShot(soundEffect);
            yield return new WaitForSeconds(soundEffect.length); // Wait for sound to finish


        }

        // Switch the player prefab
        CmdSwitchPrefab();
        // Fade back to normal
        yield return StartCoroutine(FadeScreen(new Color(1, 0, 0, 0.6f), Color.clear, 1.0f));
    }

    // Handle the fade effect of the screen
    IEnumerator FadeScreen(Color startColor, Color endColor, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            screenOverlay.color = Color.Lerp(startColor, endColor, elapsed / duration);
            yield return null;
        }
        screenOverlay.color = endColor; // Ensure final color is set
    }

    // Triggered when colliding with an object
    private void OnTriggerEnter(Collider other)
    {
        if (!isOwned) return;

        // Check if the collision is with the "Zombie" layer
        if (other.gameObject.layer == LayerMask.NameToLayer("Zombie"))
        {
            StartCoroutine(DelayedSwitch());
        }
    }

    [Command]
    void CmdSwitchPrefab()
    {
        GameObject newPrefab = (gameObject.name.Contains("puppet_kid")) ? prefabB : prefabA;

        // ✅ Fix: Store the actual position & rotation before destroying
        Vector3 lastPosition = transform.position;
        Quaternion lastRotation = transform.rotation;

        // ✅ Instantiate the new player prefab at the stored position/rotation
        GameObject newPlayer = Instantiate(newPrefab, lastPosition, lastRotation);

        // ✅ Ensure the new object is properly set up for networking
        NetworkServer.ReplacePlayerForConnection(connectionToClient, newPlayer, true);

        // ✅ Destroy the old object only after replacement
        NetworkServer.Destroy(gameObject);
    }

}
