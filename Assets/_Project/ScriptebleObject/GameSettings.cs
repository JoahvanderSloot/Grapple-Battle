using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings.asset", menuName = "Game Settings", order = 0)]
public class GameSettings : ScriptableObject
{
    public int m_GameTimer;
    public int m_GameTime;
    public int m_OutCome; // 0 = draw, 1 = win, 2 = lose
    public bool m_Audio;
    public bool m_Music;
}
