using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    [Header("In Gam eObjects")]
    [SerializeField] GameObject m_player;
    [SerializeField] GameObject m_grapplingGun;

    [Header("UI Objects")]
    [SerializeField] Image m_crossHair;

    void Update()
    {
        CrossHair();
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
