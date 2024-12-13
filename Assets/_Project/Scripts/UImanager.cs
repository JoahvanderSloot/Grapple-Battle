using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    [Header("In Gam eObjects")]
    [SerializeField] GameObject _player;
    [SerializeField] GameObject _grapplingGun;

    [Header("UI Objects")]
    [SerializeField] Image _crossHair;

    void Update()
    {
        CrossHair();
    }

    private void CrossHair()
    {
        GrapplingHook _grapplingHookScript = _grapplingGun.GetComponent<GrapplingHook>();
        if (_grapplingHookScript.m_CanGrapple)
        {
            _crossHair.color = Color.green;
        }
        else
        {
            _crossHair.color = Color.black;
        }
    }
}
