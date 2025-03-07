using Photon.Pun;
using UnityEngine;

public class NinjaStar : MonoBehaviour
{
    [Header("Shooting")]
    Rigidbody m_rb;
    [SerializeField] float m_shootSpeed;
    [SerializeField] float m_kbStrength;

    [Header("Other")]
    GameObject m_playerCam;
    Vector3 m_moveDirection;
    bool m_canDestroy = false;
    public GameObject m_player;

    void Start()
    {
        m_playerCam = GameObject.FindWithTag("PlayerCam");

        m_moveDirection = m_playerCam.transform.forward;

        m_rb = GetComponent<Rigidbody>();
        m_rb.AddForce(m_moveDirection.normalized * m_shootSpeed * 10f, ForceMode.Force);

        Destroy(gameObject, 4);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_canDestroy)
        {
            if (other.gameObject.CompareTag("PlayerBody"))
            {
                Knockback _knockbackScript = m_player.GetComponent<Knockback>();
                 PhotonView _targetView = other.GetComponentInParent<PhotonView>();
                if (_targetView != null)
                {
                    _targetView.RPC("DamageOtherPlayer", RpcTarget.Others, m_kbStrength, m_moveDirection, 2);
                }
            }
            else
            {
                HitPoints _hpScript = other.GetComponent<HitPoints>();
                if (_hpScript != null)
                {
                    _hpScript.m_HP--;
                }
                else
                {
                    Debug.Log("Object does not have HP script");
                }
            }

            Destroy(gameObject);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        m_canDestroy = true;
    }
}
