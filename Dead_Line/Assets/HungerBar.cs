using UnityEngine;
using UnityEngine.UI;

public class HungerBar : MonoBehaviour
{
    public Image hpBar; // The RectTransform of the HP bar fill
    //public Text hpText; // The Text UI element displaying HP value
    private Hunger hunger;

    private float originalWidth;

    void Start()
    {
        hunger = FindObjectOfType<Hunger>(); // Get player reference
        
    }

    void Update()
{
    if (hunger != null)
    {
        float hpRatio = hunger.remainingTime / hunger.maxTime;
        hpBar.fillAmount = hpRatio;
    }
}
}
