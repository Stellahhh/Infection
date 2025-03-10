using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class MapUI : MonoBehaviour
{
    public GameObject mapUI; // The local minimap for this player

    
    private bool isMapVisible = false;

    public InputActionReference toggleMapAction; // Input Action for "M"

    void Start()
    {
        
        mapUI.SetActive(false); // Hide the map at start
        toggleMapAction.action.performed += OnToggleMap;
        toggleMapAction.action.Enable();
    }
    
    private void OnDestroy()
    {
        toggleMapAction.action.Disable();
    }

    private void OnToggleMap(InputAction.CallbackContext context)
    {
        print("press m");
        isMapVisible = !isMapVisible;
        mapUI.SetActive(isMapVisible);
    }

    
}
