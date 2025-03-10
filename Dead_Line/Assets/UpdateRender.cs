using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateRender : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Camera minimapCam = GetComponent<Camera>();
        minimapCam.targetTexture.Release(); // Clear previous frames
        minimapCam.Render(); // Force a redraw
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Camera>().Render();
    }
}
