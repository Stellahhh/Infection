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
    public AudioSource catchAudioSource;
    public AudioClip catchAudioEffect;
    
    
    IEnumerator DelayedSwitch()
    {
        if (isLocalPlayer)
        {
            
            // update the player role for future reference
            PlayerPrefs.SetString("PlayerRole", "Zombie");
            
            // Play the sound effect
            catchAudioSource.PlayOneShot(catchAudioEffect);
            // to make the sound fully played
            yield return new WaitForSeconds(catchAudioEffect.length);

            
            
        }

        // Switch the player prefab
        CmdSwitchPrefab();
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

        //print("becoming zombie at" + transform.position);
        Vector3 lastPosition = transform.position;
        Quaternion lastRotation = transform.rotation;

        GameObject newPlayer = Instantiate(zombie_prefab, lastPosition, lastRotation);
        GameObject oldPlayer = gameObject;  
        NetworkServer.ReplacePlayerForConnection(connectionToClient, newPlayer, true);
        string name = oldPlayer.name;
        newPlayer.name = $"Zombie ({name})";
        newPlayer.transform.position = lastPosition;
        newPlayer.transform.rotation = lastRotation;
        
        NetworkServer.Destroy(oldPlayer);

    }
    
    
}
