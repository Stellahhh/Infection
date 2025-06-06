// Linda Fan <yfan43@jhu.edu>, Stella Huo <shuo2@jhu.edu>, Hanbei Zhou <hzhou43@jhu.edu>
// creating Lobby UI
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Mirror;


public class LobbyUIManager : MonoBehaviour
{
    public TMP_InputField nameInput;

    public void OnStartGamePressed()
    {
        string enteredName = nameInput.text.Trim();
        if (!string.IsNullOrEmpty(enteredName))
        {
            PlayerPrefs.SetString("PlayerName", enteredName);
            SceneManager.LoadScene("SampleScene"); // Replace with your actual scene name
        }
    }
}

