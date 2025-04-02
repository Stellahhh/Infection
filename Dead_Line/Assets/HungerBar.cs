using UnityEngine;
using UnityEngine.UI;

public class HungerBar : MonoBehaviour
{
    public RectTransform hpBarTransform; // The RectTransform of the HP bar fill
    //public Text hpText; // The Text UI element displaying HP value
    private Hunger hunger;

    private float originalWidth;

    void Start()
    {
        hunger = FindObjectOfType<Hunger>(); // Get player reference
        originalWidth = hpBarTransform.sizeDelta.x; // Store the initial bar width
    }

    void Update()
{
    if (hunger != null)
    {
        float hpRatio = hunger.remainingTime / 999f;
        hpBarTransform.sizeDelta = new Vector2(originalWidth * hpRatio, hpBarTransform.sizeDelta.y);
    }
}
}
