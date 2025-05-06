// Linda Fan <yfan43@jhu.edu>, Stella Huo <shuo2@jhu.edu>, Hanbei Zhou <hzhou43@jhu.edu>
// Managing the death of human when its life become negative
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Life : MonoBehaviour
{
    public float amount;
    public UnityEvent onDeath;
    public GameObject observerPrefab; // Assign in Inspector
    private bool isDead = false;
    void Update()
    {
        if (!isDead && amount <= 0)
        {
            isDead = true;
            onDeath.Invoke();
            EnterObserverMode();
        }
    }

     void EnterObserverMode() {
        // Example: disable player controls
        var movement = GetComponent<PlayerMovement>();
        if (movement != null) movement.enabled = false;
        var hpBar = GetComponent<HPBar>(); // lowercase 'h'
        if (hpBar != null && hpBar.hpBar != null)
            hpBar.hpBar.gameObject.SetActive(false);
        // Hide player model (optional)
        // var renderers = GetComponentsInChildren<Renderer>();
        // foreach (var r in renderers)
        //     r.enabled = false;

        // var observer = GetComponent<ObserverMode>();
        // if (observer != null)
        // {
        //     // Optionally set cameras from the map generator
        //     observer.SetCameras(DynamicMapGenerator.Instance.tileCameras.ToArray());
        //     observer.EnableObservation();
        // }
        if (observerPrefab != null)
        {
            GameObject observer = Instantiate(observerPrefab);
            var obsScript = observer.GetComponent<ObserverMode>();
            if (obsScript != null)
            {
                obsScript.SetCameras(DynamicMapGenerator.Instance.tileCameras.ToArray());
                obsScript.EnableObservation();
            }
        }

        // Destroy the player for game state logic
        Destroy(gameObject, 0.1f);
    }
}
