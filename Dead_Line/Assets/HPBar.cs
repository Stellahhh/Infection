using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public RectTransform hpBarTransform; // The RectTransform of the HP bar fill
    //public Text hpText; // The Text UI element displaying HP value
    private Life playerLife;

    private float originalWidth;

    void Start()
    {
        playerLife = FindObjectOfType<Life>(); // Get player reference
        originalWidth = hpBarTransform.sizeDelta.x; // Store the initial bar width
    }

    void Update()
{
    if (playerLife != null)
    {
        float hpRatio = playerLife.amount / 10000f;
        hpBarTransform.sizeDelta = new Vector2(originalWidth * hpRatio, hpBarTransform.sizeDelta.y);
    }
}
}
