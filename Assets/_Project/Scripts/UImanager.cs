using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    [Header("In Game Objects")]
    [SerializeField] GameObject m_player;
    [SerializeField] GameObject m_grapplingGun;
    [SerializeField] GameManager m_gameManager;

    [Header("UI Objects")]
    [SerializeField] Image m_crossHair;
    [SerializeField] TextMeshProUGUI m_gameTimerText;
    [SerializeField] TextMeshProUGUI m_playerHP;
    [SerializeField] TextMeshProUGUI m_starCount;

    void Update()
    {
        CrossHair();
        m_gameTimerText.text = m_gameManager.m_GameSettings.m_GameTimer.ToString();

        m_playerHP.text = "HP " + m_player.GetComponent<Knockback>().m_HP.ToString();
        m_starCount.text = "Star Count " + m_player.GetComponent<PlayerAttacks>().m_StarCount.ToString();
    }

    private void CrossHair()
    {
        GrapplingHook _grapplingHookScript = m_grapplingGun.GetComponent<GrapplingHook>();
        if (_grapplingHookScript.m_CanGrapple)
        {
            m_crossHair.color = Color.green;
        }
        else
        {
            m_crossHair.color = Color.black;
        }
    }
}
