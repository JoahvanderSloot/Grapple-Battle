using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    [Header("In Game Objects")]
    public GameObject m_player;
    [SerializeField] GrapplingHook m_grapplingGun;
    [SerializeField] GameManager m_gameManager;

    [Header("UI Objects")]
    [SerializeField] Image m_crossHair;
    [SerializeField] TextMeshProUGUI m_gameTimerText;
    [SerializeField] TextMeshProUGUI m_playerHP;
    [SerializeField] TextMeshProUGUI m_starCount;
    [SerializeField] GameObject m_escMenu;

    void Update()
    {
        if(m_player != null)
        {
            m_playerHP.text = "HP " + m_player.GetComponent<Knockback>().m_HP.ToString();
            m_starCount.text = "Star Count " + m_player.GetComponent<PlayerAttacks>().m_StarCount.ToString();

            if (m_grapplingGun == null)
            {
                m_grapplingGun = m_player.GetComponentInChildren<GrapplingHook>();
            }

            CrossHair();
        }

        m_gameTimerText.text = m_gameManager.m_GameSettings.m_GameTimer.ToString();

        m_escMenu.SetActive(m_gameManager.m_Paused);
        if (m_gameManager.m_Paused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
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

    public void SetPlayer(GameObject _player)
    {
        m_player = _player;
    }
}
