using UnityEngine;

[CreateAssetMenu(fileName = "playerSettings.asset", menuName = "Player Settings", order = 0)]
public class PlayerSettings : GenericScriptableSingleton<PlayerSettings>
{
    [Header("Generic Settings")]
    public float sens;
    public bool audio;

    [Header("Keybinds")]
    public KeyCode jump = KeyCode.Space;
    public KeyCode crouch = KeyCode.LeftShift;
    public KeyCode attack = KeyCode.Mouse0;
    public KeyCode grapple = KeyCode.Mouse1;
    public KeyCode slot1 = KeyCode.Alpha1;
    public KeyCode slot2 = KeyCode.Alpha2;
}
