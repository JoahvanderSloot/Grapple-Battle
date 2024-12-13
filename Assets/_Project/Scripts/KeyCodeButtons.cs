using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class KeyCodeButtons : MonoBehaviour
{
    public playerSettings playerSettings;

    [SerializeField] string m_key;
    bool m_waitForKeyInput = false;
    [SerializeField] TextMeshProUGUI m_buttonText;
    List<KeyCode> m_keyBinds;

    private void Start()
    {
        m_keyBinds = new List<KeyCode>();
        UpdateButtonText();

        m_keyBinds.AddRange(typeof(playerSettings).GetFields()
            .Where(f => f.FieldType == typeof(KeyCode))
            .Select(f => (KeyCode)f.GetValue(playerSettings)));
    }


    public void SetKeyCode()
    {
        m_waitForKeyInput = true;
        Image _buttonColor = GetComponent<Image>();
        _buttonColor.color = Color.gray;
    }

    private void Update()
    {
        if (m_waitForKeyInput)
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
        if (Input.anyKeyDown)
        {
            foreach (KeyCode _keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(_keyCode))
                {
                    if (!m_keyBinds.Contains(_keyCode))
                    {
                        Image buttonColor = GetComponent<Image>();
                        buttonColor.color = Color.white;
                        SetKey(_keyCode);
                        m_waitForKeyInput = false;
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

    private void SetKey(KeyCode _newKey)
    {
        switch (m_key)
        {
            case "jump":
                playerSettings.m_Jump = _newKey;
                break;
            case "crouch":
                playerSettings.m_Crouch = _newKey;
                break;
            case "attack":
                playerSettings.m_Attack = _newKey;
                break;
            case "grapple":
                playerSettings.m_Grapple = _newKey;
                break;
            case "slot1":
                playerSettings.m_Slot1 = _newKey;
                break;
            case "slot2":
                playerSettings.m_Slot2 = _newKey;
                break;
        }
    }

    private void UpdateButtonText()
    {
        KeyCode _currentKey = GetCurrentKey();
        m_buttonText.text = m_key + "\n" + _currentKey.ToString();
    }

    private KeyCode GetCurrentKey()
    {
        switch (m_key)
        {
            case "jump":
                return playerSettings.m_Jump;
            case "crouch":
                return playerSettings.m_Crouch;
            case "attack":
                return playerSettings.m_Attack;
            case "grapple":
                return playerSettings.m_Grapple;
            case "slot1":
                return playerSettings.m_Slot1;
            case "slot2":
                return playerSettings.m_Slot2;
            default:
                return KeyCode.None;
        }
    }
}
