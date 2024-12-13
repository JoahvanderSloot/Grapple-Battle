using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class KeyCodeButtons : MonoBehaviour
{
    public playerSettings playerSettings;

    [SerializeField] string key;
    bool waitForKeyInput = false;
    [SerializeField] TextMeshProUGUI buttonText;
    List<KeyCode> keyBinds;
    [SerializeField] List<Button> allButtons;

    private void Start()
    {
        keyBinds = new List<KeyCode>();
        UpdateButtonText();

        keyBinds.AddRange(typeof(playerSettings).GetFields()
            .Where(f => f.FieldType == typeof(KeyCode))
            .Select(f => (KeyCode)f.GetValue(playerSettings)));
    }


    public void SetKeyCode()
    {
        waitForKeyInput = true;
        Image buttonColor = GetComponent<Image>();
        buttonColor.color = Color.gray;
    }

    private void Update()
    {
        if (waitForKeyInput)
        {
            DetectKeyPress();
        }
        else
        {
            UpdateButtonText();
        }
    }

    private void DetectKeyPress()
    {

        foreach (Button _b in allButtons)
        {
            _b.interactable = false;
        }

        if (Input.anyKeyDown)
        {
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    if (!keyBinds.Contains(keyCode))
                    {
                        Image buttonColor = GetComponent<Image>();
                        buttonColor.color = Color.white;
                        SetKey(keyCode);
                        waitForKeyInput = false;
                        UpdateButtonText();
                        break;
                    }
                    else
                    {
                        Debug.Log("This key is already being used");
                    }
                }
            }
        }
    }

    private void SetKey(KeyCode newKey)
    {
        switch (key)
        {
            case "jump":
                playerSettings.jump = newKey;
                break;
            case "crouch":
                playerSettings.crouch = newKey;
                break;
            case "attack":
                playerSettings.attack = newKey;
                break;
            case "grapple":
                playerSettings.grapple = newKey;
                break;
            case "slot1":
                playerSettings.slot1 = newKey;
                break;
            case "slot2":
                playerSettings.slot2 = newKey;
                break;
        }
    }

    private void UpdateButtonText()
    {
        KeyCode currentKey = GetCurrentKey();
        buttonText.text = key + "\n" + currentKey.ToString();

        foreach (Button _b in allButtons)
        {
            _b.interactable = true;
        }
    }

    private KeyCode GetCurrentKey()
    {
        switch (key)
        {
            case "jump":
                return playerSettings.jump;
            case "crouch":
                return playerSettings.crouch;
            case "attack":
                return playerSettings.attack;
            case "grapple":
                return playerSettings.grapple;
            case "slot1":
                return playerSettings.slot1;
            case "slot2":
                return playerSettings.slot2;
            default:
                return KeyCode.None;
        }
    }
}
