using Photon.Pun;
using System.Collections;
using UnityEngine;

public class NinjaStar : MonoBehaviourPun
{
    [Header("Shooting")]
    Rigidbody m_rb;
    [SerializeField] float m_shootSpeed;
    [SerializeField] float m_kbStrength;

    [Header("Other")]
    GameObject m_playerCam;
    Vector3 m_moveDirection;

    public int m_OwnerID;

    void Start()
    {
        m_playerCam = GameObject.FindWithTag("PlayerCam");

        m_moveDirection = m_playerCam.transform.forward;

        m_rb = GetComponent<Rigidbody>();
        m_rb.AddForce(m_moveDirection.normalized * m_shootSpeed * 10f, ForceMode.Force);

        StartCoroutine(PhotonDestroyAfterSeconds(4));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_OwnerID > 0 && other.gameObject.GetComponentInParent<PhotonView>().OwnerActorNr != m_OwnerID)
        {
            if (other.gameObject.CompareTag("PlayerBody"))
            {
                Knockback _knockbackScript = other.GetComponentInParent<Knockback>();
                _knockbackScript.m_PhotonView.RPC("DamageOtherPlayer", RpcTarget.All, m_kbStrength, m_rb.velocity.normalized, 1);
            }

            StartCoroutine(PhotonDestroyAfterSeconds(0));
        }
    }

    private IEnumerator PhotonDestroyAfterSeconds(float _seconds)
    {
        yield return new WaitForSeconds(_seconds);
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    [PunRPC]
    public void SetOwner(int _owner)
    {
        m_OwnerID = _owner;
    }
}
