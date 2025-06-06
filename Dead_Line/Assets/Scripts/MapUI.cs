// Linda Fan <yfan43@jhu.edu>, Stella Huo <shuo2@jhu.edu>, Hanbei Zhou <hzhou43@jhu.edu>
// Manage user input system associated with map function
using UnityEngine;
using UnityEngine.InputSystem;

public class MapUI : MonoBehaviour
{
    public GameObject mapUI; // The local minimap for this player
    public GameObject mapFrame;
    public PlayerInput playerInput; // Reference to PlayerInput component

    private bool isMapVisible = false;
    private InputAction toggleMapAction;

    void Start()
    {
        mapUI.SetActive(false); // Hide the map at start
        mapFrame.SetActive(false);
        // Find the toggle action from PlayerInput
        toggleMapAction = playerInput.actions["ToggleMap"]; // Ensure "ToggleMap" matches the Input Action name in your Input Actions asset

        if (toggleMapAction != null)
        {
            toggleMapAction.performed += OnToggleMap;
            toggleMapAction.Enable();
        }
        else
        {
            Debug.LogError("ToggleMap action not found in PlayerInput!");
        }
    }

    private void OnDestroy()
    {
        if (toggleMapAction != null)
        {
            toggleMapAction.Disable();
        }
    }

    private void OnToggleMap(InputAction.CallbackContext context)
    {
        print("Press M to toggle the map");
        isMapVisible = !isMapVisible;
        mapUI.SetActive(isMapVisible);
        mapFrame.SetActive(isMapVisible);
    }
}
