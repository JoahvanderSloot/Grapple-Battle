using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings.asset", menuName = "Game Settings", order = 0)]
public class GameSettings : ScriptableObject
{
    public int m_GameTimer;
    public int m_GameTime;
    //0 = draw, 1 = win, 2 = lose
    public int m_OutCome;
}
