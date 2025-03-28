using System.Collections;
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
    [SerializeField] TextMeshProUGUI m_waitingText;

    private void Start()
    {
        StartCoroutine(AnimateDots());
    }

    void Update()
    {
        if (!GameManager.Instance.LocalPlayer) return;

        m_playerHP.text = GameManager.Instance.LocalPlayer.GetComponent<Knockback>().m_HP.ToString();
        m_starCount.text = GameManager.Instance.LocalPlayer.GetComponent<PlayerAttacks>().m_StarCount.ToString();

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

    private IEnumerator AnimateDots()
    {
        string _baseText = "Waiting for opponent";
        int _dotCount = 0;

        while (GameManager.Instance.WaitingObj.activeInHierarchy)
        {
            m_waitingText.text = _baseText + new string('.', _dotCount);
            _dotCount = (_dotCount + 1) % 4;
            yield return new WaitForSeconds(0.5f);
        }

        m_waitingText.text = _baseText;
    }

}
