using UnityEngine;
using Photon.Pun;

public class KatanaScript : MonoBehaviour
{
    Animator m_animator;
    [SerializeField] Transform m_raySpawn;
    [SerializeField] float m_reach;
    [SerializeField] float m_kbStrength;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    public void Attack()
    {
        m_animator.SetTrigger("Attack");

        RaycastHit _hitInfo;
        bool _hit = Physics.Raycast(m_raySpawn.position, m_raySpawn.forward, out _hitInfo, m_reach);

        if (_hit)
        {
            if (_hitInfo.collider.gameObject.CompareTag("PlayerBody"))
            {
                PhotonView _targetView = _hitInfo.collider.GetComponentInParent<PhotonView>();
                if (_targetView != null)
                {
                    _targetView.RPC("DamageOtherPlayer", RpcTarget.AllBuffered, m_kbStrength, m_raySpawn.forward, 2);
                }
            }
            else
            {
                HitPoints _hpScript = _hitInfo.collider.GetComponent<HitPoints>();
                if (_hpScript != null)
                {
                    PhotonView objectView = _hitInfo.collider.GetComponent<PhotonView>();
                    if (objectView != null && objectView.IsMine)
                    {
                        objectView.RPC("TakeDamage", RpcTarget.AllBuffered, 1);
                    }
                }
                else
                {
                    Debug.Log("Object does not have HP script");
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(m_raySpawn.position, m_raySpawn.position + m_raySpawn.forward * m_reach);
    }
}
