using UnityEngine;

public class SetKeybinds : MonoBehaviour
{
    public playerSettings m_PlayerSettings;

    private void Start()
    {
        if (AudioManager.m_Instance.IsPlaying("GameMusic"))
        {
            AudioManager.m_Instance.Stop("GameMusic");
        }
        if (!AudioManager.m_Instance.IsPlaying("MenuMusic"))
        {
            AudioManager.m_Instance.Play("MenuMusic");
        }

        if(m_PlayerSettings.jump == KeyCode.None)
        {
            m_PlayerSettings.jump = KeyCode.Space;
        }
        if(m_PlayerSettings.crouch == KeyCode.None)
        {
            m_PlayerSettings.crouch = KeyCode.LeftShift;
        }
        if(m_PlayerSettings.attack == KeyCode.None)
        {
            m_PlayerSettings.attack = KeyCode.Mouse0;
        }
        if(m_PlayerSettings.grapple == KeyCode.None)
        {
            m_PlayerSettings.grapple = KeyCode.Mouse1;
        }
        if(m_PlayerSettings.slot1 == KeyCode.None)
        {
            m_PlayerSettings.slot1 = KeyCode.Alpha1;
        }
        if(m_PlayerSettings.slot2 == KeyCode.None)
        {
            m_PlayerSettings.slot2 = KeyCode.Alpha2;
        }
    }
}
