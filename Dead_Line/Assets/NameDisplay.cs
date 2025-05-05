using UnityEngine;
using TMPro;
public class NameDisplay : MonoBehaviour
{
    private Camera targetCam;
    public PlayerRole role;
    private TextMeshProUGUI nameText;
    void Start(){
        nameText = GetComponent<TextMeshProUGUI>();
        
    }
    void Update()
    {
        nameText.text = role.playerName;
        if (targetCam == null)
        {
            // Find the only active camera named "Human Camera"
            Camera[] allCams = Camera.allCameras;
            foreach (Camera cam in allCams)
            {
                if (cam.gameObject.activeInHierarchy)
                {
                    targetCam = cam;
                    break;
                }
            }

            if (targetCam == null) return;}
    }
}
