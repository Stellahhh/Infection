// Linda Fan, Stella Huo, Hanbei Zhou

using UnityEngine;
using TMPro;

// Controls the warning UI displayed on the player's screen when they enter a danger zone
public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI warningText; // Reference to the warning message text (assigned in the prefab)

    // Toggle visibility of the warning text based on the 'show' flag
    public void ShowWarning(bool show)
    {
        // Activate or deactivate the warning UI GameObject
        warningText.gameObject.SetActive(show);

        // Update the displayed text only if showing
        warningText.text = show ? "Danger Zone! Get Out!" : "";
    }

 

}
