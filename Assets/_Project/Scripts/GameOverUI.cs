using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] GameSettings m_gameSettings;
    [SerializeField] TextMeshProUGUI m_title;

    private void Start()
    {
        switch (m_gameSettings.m_OutCome)
        {
            case 0:
                m_title.text = "Draw";
                break;
            case 1:
                m_title.text = "Victory";
                break;
            case 2:
                m_title.text = "Defeat";
                break;
        }

    }
}
