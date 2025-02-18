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

    void Update()
    {
        CrossHair();
        m_gameTimerText.text = m_gameManager.m_GameSettings.m_GameTimer.ToString();
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
