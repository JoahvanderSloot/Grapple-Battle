using UnityEngine;
using Photon.Pun;

public class KatanaScript : MonoBehaviour
{
    Animator m_animator;
    [SerializeField] Transform m_raySpawn;
    [SerializeField] float m_reach;
    [SerializeField] float m_kbStrength;
    [SerializeField] Knockback m_knockbackScript;

    void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    public void Attack()
    {
        m_animator.SetTrigger("Attack");

        //check if you hit anything with a raycast and run different command in different objects you hit
        RaycastHit _hitInfo;
        bool _hit = Physics.Raycast(m_raySpawn.position, m_raySpawn.forward, out _hitInfo, m_reach);

        if (_hit)
        {
            if (_hitInfo.collider.gameObject.CompareTag("PlayerBody"))
            {
                m_knockbackScript.m_view.RPC("DamageOtherPlayer", RpcTarget.Others, m_kbStrength, m_raySpawn.forward, 2);
            }
            else
            {
                HitPoints _hpScript = _hitInfo.collider.GetComponent<HitPoints>();
                if (_hpScript != null)
                {
                    _hpScript.m_HP--;
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
