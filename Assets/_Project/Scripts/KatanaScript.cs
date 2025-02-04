using UnityEngine;

public class KatanaScript : MonoBehaviour
{
    Animator m_animator;
    [SerializeField] Transform m_raySpawn;
    [SerializeField] float m_reach;
    [SerializeField] Collider m_katanaCollider;
    [SerializeField] float m_kbStrength;

    void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    public void Attack()
    {
        m_animator.SetTrigger("Attack");

        //check if you hit anything with a raycast and run different command in different objects you hit
        RaycastHit _hitInfo;
        bool hit = Physics.Raycast(m_raySpawn.position, m_raySpawn.forward, out _hitInfo, m_reach);

        if (hit)
        {
            Knockback _knockbackScript = _hitInfo.collider.GetComponent<Knockback>();
            if (_knockbackScript != null)
            {
                _knockbackScript.AddKnockback(m_kbStrength);
                _knockbackScript.m_KbDirection = m_raySpawn.forward;
                _knockbackScript.m_HP--;
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
