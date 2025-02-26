using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunger : MonoBehaviour
{
    public float maxTime;
    public float remainingTime;
    
    public UnityEngine.Events.UnityEvent onDeath;

    private void Start()
    {
        remainingTime = maxTime;
    }
    void Update()
    {
        remainingTime -= Time.deltaTime; // Decrease time gradually

        if (remainingTime < 0)
        {
            onDeath.Invoke();
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Human")) // Check if collided with a human
        {
            remainingTime = maxTime; // clear hungriness if eat a human
        }
    }
}
