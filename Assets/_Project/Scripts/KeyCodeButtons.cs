using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyCodeButtons : MonoBehaviour
{
    public playerSettings m_PlayerSettings;

    bool m_waitForKeyInput = false;
    List<KeyCode> m_keyBinds;
    [SerializeField] private List<Button> m_allButtons;
    int m_selectedButtonIndex = -1;
    [SerializeField] GameObject m_keyIsBeingUsed;
    float m_textTimer = 0;

    private void Start()
    {
        m_keyBinds = new List<KeyCode>();

        m_keyIsBeingUsed.SetActive(false);

        m_keyBinds.AddRange(typeof(playerSettings).GetFields()
            .Where(f => f.FieldType == typeof(KeyCode))
            .Select(f => (KeyCode)f.GetValue(m_PlayerSettings)));

        UpdateButtonText();
    }

    public void SetKeyCode(int _buttonIndex)
    {
        m_waitForKeyInput = true;
        m_selectedButtonIndex = _buttonIndex;

        foreach (Button _button in m_allButtons)
        {
            _button.interactable = false;
        }

        m_allButtons[_buttonIndex].GetComponent<Image>().color = Color.gray;
    }

    private void Update()
    {
        if (m_waitForKeyInput && !m_keyIsBeingUsed.activeSelf)
        {
            DetectKeyPress();
        }

        if (m_keyIsBeingUsed.activeSelf)
        {
            m_textTimer += Time.deltaTime * 2;
            if (m_textTimer >= 2)
            {
                m_keyIsBeingUsed.SetActive(false);
                m_textTimer = 0;
            }
        }
    }

    private void DetectKeyPress()
    {
        if (Input.anyKeyDown)
        {
            foreach (KeyCode _key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(_key))
                {
                    if (!m_keyBinds.Contains(_key))
                    {
                        SetKey(m_selectedButtonIndex, _key);
                        m_waitForKeyInput = false;
                        m_selectedButtonIndex = -1;
                        break;
                    }
                    else
                    {
                        m_keyIsBeingUsed.SetActive(true);

                        m_waitForKeyInput = false;
                        m_selectedButtonIndex = -1;

                        foreach (Button _button in m_allButtons)
                        {
                            _button.interactable = true;
                            _button.GetComponent<Image>().color = Color.white;
                        }
                    }

                }
            }
        }
    }

    private void SetKey(int _buttonIndex, KeyCode _newKey)
    {
        string _keyName = GetKeyName(_buttonIndex);

        switch (_keyName)
        {
            case "jump": m_PlayerSettings.jump = _newKey; break;
            case "crouch": m_PlayerSettings.crouch = _newKey; break;
            case "attack": m_PlayerSettings.attack = _newKey; break;
            case "grapple": m_PlayerSettings.grapple = _newKey; break;
            case "slot1": m_PlayerSettings.slot1 = _newKey; break;
            case "slot2": m_PlayerSettings.slot2 = _newKey; break;
        }

        RefreshKeyBinds();
        UpdateButtonText();
    }

    private void RefreshKeyBinds()
    {
        m_keyBinds.Clear();
        m_keyBinds.AddRange(typeof(playerSettings).GetFields()
            .Where(f => f.FieldType == typeof(KeyCode))
            .Select(f => (KeyCode)f.GetValue(m_PlayerSettings)));
    }

    private void UpdateButtonText()
    {
        for (int i = 0; i < m_allButtons.Count; i++)
        {
            Button _button = m_allButtons[i];
            string _keyName = GetKeyName(i);
            KeyCode _currentKey = GetCurrentKey(_keyName);

            _button.GetComponentInChildren<TextMeshProUGUI>().text = _keyName + "\n" + _currentKey.ToString();
            _button.interactable = true;
            _button.GetComponent<Image>().color = Color.white;
        }
    }

    private KeyCode GetCurrentKey(string _keyName)
    {
        return _keyName switch
        {
            "jump" => m_PlayerSettings.jump,
            "crouch" => m_PlayerSettings.crouch,
            "attack" => m_PlayerSettings.attack,
            "grapple" => m_PlayerSettings.grapple,
            "slot1" => m_PlayerSettings.slot1,
            "slot2" => m_PlayerSettings.slot2,
            _ => KeyCode.None
        };
    }

    private string GetKeyName(int _index)
    {
        return m_allButtons[_index].name;
    }
}