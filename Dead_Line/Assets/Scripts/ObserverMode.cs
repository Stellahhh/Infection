// Linda Fan <yfan43@jhu.edu>, Stella Huo <shuo2@jhu.edu>, Hanbei Zhou <hzhou43@jhu.edu>
// Managing the observer mode for the game
using UnityEngine;
using System.Collections;

public class ObserverMode : MonoBehaviour
{
    public Camera[] tileCameras; // Set via SetCameras()
    public Canvas observerCanvas;
    public GameObject bgmManagerPrefab;
    private int currentIndex = -1;
    private bool observing = false;

    public void SetCameras(Camera[] cams)
    {
        tileCameras = cams;
    }

    public void EnableObservation()
    {
        observing = true;
        SwitchToCamera(0); // Start on camera 1 (index 0)
        if (observerCanvas != null)
            observerCanvas.enabled = true;
        if (FindObjectOfType<BGMManager>() == null)
    {
        Instantiate(bgmManagerPrefab);
    }
    }

    void Update()
    {
        if (!observing || tileCameras == null || tileCameras.Length == 0)
            return;

        for (int i = 0; i < tileCameras.Length; i++)
        {
            if (Input.GetKeyDown((KeyCode)((int)KeyCode.Alpha1 + i)))
            {
                SwitchToCamera(i);
            }
        }
    }

     void SwitchToCamera(int index)
    {
        if (index < 0 || index >= tileCameras.Length || tileCameras[index] == null)
            return;

        // Disable previous
        if (currentIndex != -1 && tileCameras[currentIndex] != null)
            tileCameras[currentIndex].gameObject.SetActive(false);

        // Enable new
        tileCameras[index].gameObject.SetActive(true);
        currentIndex = index;

        // Update the Canvas render camera
        if (observerCanvas != null)
            observerCanvas.worldCamera = tileCameras[index];
    }
}
