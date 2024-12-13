using UnityEngine;

[CreateAssetMenu(fileName = "playerSettings.asset", menuName = "Player Settings", order = 0)]
public class playerSettings : GenericScriptableSingleton<playerSettings>
{
    [Header("Generic Settings")]
    public float m_Sens;
    public bool m_Audio;

    [Header("Keybinds")]
    public KeyCode m_Jump;
    public KeyCode m_Crouch;
    public KeyCode m_Attack;
    public KeyCode m_Grapple;
    public KeyCode m_Slot1;
    public KeyCode m_Slot2;
}
