using TMPro;
using UnityEngine;

public class KeyCodeButtons : MonoBehaviour
{
    public PlayerSettings playerSettings;

    [SerializeField] string key;
    bool waitForKeyInput = false;
    [SerializeField] TextMeshProUGUI buttonText;

    public void SetKeyCode()
    {
        waitForKeyInput = true;
        buttonText.text = key;
    }

    private void Update()
    {
        if (waitForKeyInput)
        {
            SetKey(key);
        }
    }

    private void SetKey(string key)
    {
        KeyCode newKey = KeyCode.G;
        switch (key)
        {
            case "jump":
                playerSettings.jump = newKey;
                break;
            case "crouch":
                playerSettings.jump = newKey;
                break;
            case "attack":
                playerSettings.jump = newKey;
                break;
            case "grapple":
                playerSettings.jump = newKey;
                break;
            case "slot1":
                playerSettings.jump = newKey;
                break;
            case "slot2":
                playerSettings.jump = newKey;
                break;
        }
    }
}
