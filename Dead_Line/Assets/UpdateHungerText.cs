using UnityEngine;
using TMPro;

public class HungerUIUpdater : MonoBehaviour
{
    public TextMeshProUGUI hungerText; // Assign in Inspector
    public Hunger hungerScript; // Reference to the player's Hunger script

    void Start()
    {
        // Find the player in the scene (Make sure the player has a "Player" tag)
       
       
    }

    void Update()
    {
      
            // Update UI with the remaining hunger time
        hungerText.text = "Time remaining: " + Mathf.Ceil(hungerScript.remainingTime).ToString() + "s";
        
    }
}
