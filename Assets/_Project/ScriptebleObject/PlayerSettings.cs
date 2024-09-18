using UnityEngine;

[CreateAssetMenu(fileName = "playerSettings.asset", menuName = "Player Settings", order = 0)]
public class playerSettings : GenericScriptableSingleton<playerSettings>
{
    [Header("Generic Settings")]
    public float sens;
    public bool audio;

    [Header("Keybinds")]
    public KeyCode jump;
    public KeyCode crouch;
    public KeyCode attack;
    public KeyCode grapple;
    public KeyCode slot1;
    public KeyCode slot2;
}
