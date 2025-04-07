using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

public class NinjaStar : MonoBehaviour
{
    [Header("Shooting")]
    Rigidbody m_rb;
    [SerializeField] float m_shootSpeed;
    [SerializeField] float m_kbStrength;

    [Header("Other")]
    GameObject m_playerCam;
    Vector3 m_moveDirection;

    public PhotonView m_OwnerView;

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
        if (m_OwnerView != null && !m_OwnerView.IsMine)
        {
            if (other.gameObject.CompareTag("PlayerBody"))
            {
                Knockback _knockbackScript = other.GetComponentInParent<Knockback>();
                PhotonView _targetView = other.GetComponentInParent<PhotonView>();
                if (_targetView != null)
                {
                    // Apply knockback and damage on all clients (including attacker)
                    _targetView.RPC("DamageOtherPlayer", RpcTarget.Others, m_kbStrength, m_rb.velocity.normalized, 1);
                }
            }
            else
            {
                HitPoints _hpScript = other.GetComponent<HitPoints>();
                if (_hpScript != null)
                {
                    _hpScript.m_HP--;
                }
            }

            Destroy(gameObject);
        }
    }
}
