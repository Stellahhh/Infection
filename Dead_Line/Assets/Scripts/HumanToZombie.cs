// Linda Fan, Stella Huo, Hanbei Zhou
using UnityEngine;
using Mirror;
using UnityEngine.InputSystem; // Import New Input System
using System.Collections;
using UnityEngine.UI; // For Image reference

public class PlayerSwitch : NetworkBehaviour
{
    public GameObject human_prefab;
    public GameObject zombie_prefab;
    public AudioSource audioSource;
    public AudioClip soundEffect;
    //public Image screenOverlay; // Reference to UI Image for red screen effect

    IEnumerator DelayedSwitch()
    {
        if (isLocalPlayer)
        {
            Debug.Log("Starting red screen fade effect");
            
            // Ensure the screen overlay is active
            //screenOverlay.gameObject.SetActive(true);
            
            // Fade to red
        //yield return StartCoroutine(FadeScreen(Color.clear, new Color(1, 0, 0, 0.3f), 1.0f));
            
            // update the player role for future reference
            PlayerPrefs.SetString("PlayerRole", "Zombie");
            
            // Play the sound effect
            audioSource.PlayOneShot(soundEffect);
            yield return new WaitForSeconds(soundEffect.length);

            
            Debug.Log("Finished red screen fade effect");
        }

        // Switch the player prefab
        CmdSwitchPrefab();
    }

    // IEnumerator FadeScreen(Color startColor, Color endColor, float duration)
    // {
    //     float elapsed = 0f;
    //     while (elapsed < duration)
    //     {
    //         elapsed += Time.deltaTime;
    //         if (screenOverlay != null)
    //         {
    //             screenOverlay.color = Color.Lerp(startColor, endColor, elapsed / duration);
    //         }
    //         yield return null;
    //     }
    //     // if (screenOverlay != null)
    //     // {
    //     //     screenOverlay.color = endColor; // Ensure final color is set
    //     // }
    // }

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

        print("becoming zombie... at" + transform.position);
        Vector3 lastPosition = transform.position;
        Quaternion lastRotation = transform.rotation;

        GameObject newPlayer = Instantiate(zombie_prefab, lastPosition, lastRotation);
        GameObject oldPlayer = gameObject;  
        NetworkServer.ReplacePlayerForConnection(connectionToClient, newPlayer, true);
        newPlayer.name = $"Zombie [connId={connectionToClient.connectionId}]";
        newPlayer.transform.position = lastPosition;
        newPlayer.transform.rotation = lastRotation;
        
        print(newPlayer.transform.position);
        NetworkServer.Destroy(oldPlayer);

    }
    
    
}
