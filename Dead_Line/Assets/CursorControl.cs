using UnityEngine;
using UnityEngine.InputSystem; // New Input System

public class CursorControl : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput; // Reference to PlayerInput component
    private bool cursorVisible = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnEnable()
    {
        // Subscribe to the toggle action from Input System
        playerInput.actions["ToggleCursor"].performed += ToggleCursor;
    }

    void OnDisable()
    {
        // Unsubscribe when disabled
        playerInput.actions["ToggleCursor"].performed -= ToggleCursor;
    }

    private void ToggleCursor(InputAction.CallbackContext context)
    {
        cursorVisible = !cursorVisible;

        if (cursorVisible)
        {
            Cursor.lockState = CursorLockMode.None; // Free the cursor
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked; // Lock to center
            Cursor.visible = false;
        }
    }
}
