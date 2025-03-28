using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings.asset", menuName = "Game Settings", order = 0)]
public class GameSettings : ScriptableObject
{
    public int m_GameTimer;
    public int m_GameTime;
    public bool m_Audio;
    public bool m_Music;
}
