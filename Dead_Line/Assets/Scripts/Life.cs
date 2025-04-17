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
    void Update()
    {
        if (amount <= 0)
        {
            onDeath.Invoke();
            Destroy(gameObject);
        }
    }
}
