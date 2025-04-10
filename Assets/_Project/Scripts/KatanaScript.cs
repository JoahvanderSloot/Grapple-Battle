using UnityEngine;
using Photon.Pun;

public class KatanaScript : MonoBehaviourPunCallbacks
{
    Animator m_animator;
    [SerializeField] Transform m_raySpawn;
    [SerializeField] float m_reach;
    [SerializeField] float m_kbStrength;
    public bool m_CanHitPlayer;
    [SerializeField] Collider m_ownBody;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    private void Update()
    {
        RaycastHit _hitInfo;
        if(PerformRaycast(out _hitInfo))
        {
            if(_hitInfo.collider.CompareTag("PlayerBody") && _hitInfo.collider != m_ownBody)
            {
                m_CanHitPlayer = true;
            }
        }
        else
        {
            m_CanHitPlayer = false;
        }
    }

    public void Attack()
    {
        m_animator.SetTrigger("Attack");
        AudioManager.m_Instance.Play("Swoosh");

        RaycastHit _hitInfo;
        if (PerformRaycast(out _hitInfo))
        {
            if (_hitInfo.collider.CompareTag("PlayerBody"))
            {
                PhotonView _targetView = _hitInfo.collider.GetComponentInParent<PhotonView>();
                if (_targetView != null)
                {
                    AudioManager.m_Instance.Play("Hit");
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
            }
        }
    }

    private bool PerformRaycast(out RaycastHit hitInfo)
    {
        return Physics.Raycast(m_raySpawn.position, m_raySpawn.forward, out hitInfo, m_reach);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(m_raySpawn.position, m_raySpawn.position + m_raySpawn.forward * m_reach);
    }
}
