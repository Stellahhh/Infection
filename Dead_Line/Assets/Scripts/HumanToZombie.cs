// Linda Fan, Stella Huo, Hanbei Zhou
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

    IEnumerator DelayedSwitch()
    {
        if (isLocalPlayer)
        {
            Debug.Log("Starting red screen fade effect");
            
            // Ensure the screen overlay is active
            screenOverlay.gameObject.SetActive(true);
            
            // Fade to red
            yield return StartCoroutine(FadeScreen(Color.clear, new Color(1, 0, 0, 0.6f), 1.0f));

            // Play the sound effect
            audioSource.PlayOneShot(soundEffect);
            yield return new WaitForSeconds(soundEffect.length);

            // Fade back to normal
            yield return StartCoroutine(FadeScreen(new Color(1, 0, 0, 0.6f), Color.clear, 1.0f));
            
            Debug.Log("Finished red screen fade effect");
        }

        // Switch the player prefab
        CmdSwitchPrefab();
    }

    IEnumerator FadeScreen(Color startColor, Color endColor, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            if (screenOverlay != null)
            {
                screenOverlay.color = Color.Lerp(startColor, endColor, elapsed / duration);
            }
            yield return null;
        }
        if (screenOverlay != null)
        {
            screenOverlay.color = endColor; // Ensure final color is set
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isOwned) return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Zombie") || gameObject.layer == LayerMask.NameToLayer("Zombie"))
        {
            Debug.Log("Collided with Zombie - Starting Coroutine");
            StartCoroutine(DelayedSwitch());
        }
    }

    [Command]
    void CmdSwitchPrefab()
    {
        GameObject newPrefab = (gameObject.name.Contains("puppet_kid")) ? prefabB : prefabA;

        Vector3 lastPosition = transform.position;
        Quaternion lastRotation = transform.rotation;

        GameObject newPlayer = Instantiate(newPrefab, lastPosition, lastRotation);

        NetworkServer.ReplacePlayerForConnection(connectionToClient, newPlayer, true);

        NetworkServer.Destroy(gameObject);
    }
}
