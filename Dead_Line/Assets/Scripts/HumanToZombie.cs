// Linda Fan <yfan43@jhu.edu>, Stella Huo <shuo2@jhu.edu>, Hanbei Zhou <hzhou43@jhu.edu>
// Handle the switch from human to zombie when collide
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
    public GameObject canvas;
    
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

            ZombieController zombieController = other.GetComponent<ZombieController>();
            Debug.Log("ZombieController: " + zombieController);
            if (zombieController != null)
            {
                Debug.Log("Incrementing infection count");
                zombieController.IncrementInfection(); // Track infection count
            }

            // You can track last prey too if needed
            HumanController humanController = GetComponent<HumanController>();
            PlayerRole roleComponent = GetComponent<PlayerRole>();
            if (humanController != null && roleComponent != null)
            {
                HumanController.lastInfectedName = roleComponent.playerName; // Use the custom player name from the lobby
                Debug.Log($"Last infected human (playerName): {HumanController.lastInfectedName}");
            }

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
        int start = gameObject.name.IndexOf('(') + 1;
        int end = gameObject.name.IndexOf(')');
        string name = gameObject.name.Substring(start, end - start);
        PlayerRole roleComponent = newPlayer.GetComponent<PlayerRole>();
        if (roleComponent != null)
        {
            roleComponent.role = "Zombie";
            
            roleComponent.playerName = name;
        }

        GameObject oldPlayer = gameObject;  
        NetworkServer.ReplacePlayerForConnection(connectionToClient, newPlayer, true);
        
        newPlayer.transform.position = lastPosition;
        newPlayer.transform.rotation = lastRotation;
        canvas.SetActive(false);
        NetworkServer.Destroy(oldPlayer);

    }
    
    
}
