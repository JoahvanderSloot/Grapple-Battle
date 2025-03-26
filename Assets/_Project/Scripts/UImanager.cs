using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    [Header("In Game Objects")]
    [SerializeField] GrapplingHook m_grapplingGun;

    [Header("UI Objects")]
    [SerializeField] Image m_crossHair;
    [SerializeField] TextMeshProUGUI m_gameTimerText;
    [SerializeField] TextMeshProUGUI m_playerHP;
    [SerializeField] TextMeshProUGUI m_starCount;

    void Update()
    {
        if (!GameManager.Instance.LocalPlayer) return;

        m_playerHP.text = "HP " + GameManager.Instance.LocalPlayer.GetComponent<Knockback>().m_HP.ToString();
        m_starCount.text = "Star Count " + GameManager.Instance.LocalPlayer.GetComponent<PlayerAttacks>().m_StarCount.ToString();

        if (m_grapplingGun == null)
        {
            m_grapplingGun = GameManager.Instance.LocalPlayer.GetComponentInChildren<GrapplingHook>();
        }

        CrossHair();


        m_gameTimerText.text = GameManager.Instance.m_GameSettings.m_GameTimer.ToString();
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
