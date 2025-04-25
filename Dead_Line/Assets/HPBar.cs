using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Image hpBar; 
    //public Text hpText; // The Text UI element displaying HP value
    private Life playerLife;

    private float originalWidth;

    void Start()
    {
        playerLife = FindObjectOfType<Life>(); // Get player reference
        
    }

    void Update()
{
    if (playerLife != null)
    {
        hpBar.fillAmount =  playerLife.amount / 10000f;
    }
}
}
