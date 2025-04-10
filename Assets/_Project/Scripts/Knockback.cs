using Photon.Pun;
using UnityEngine;

public class Knockback : HitPoints
{
    private Rigidbody m_rigidbody;

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    [PunRPC]
    public void DamageOtherPlayer(float _knockBackStrength, Vector3 _direction, int _damage)
    {
        m_HP -= _damage;
        if(m_hitCoroutine == null)
        {
            m_hitCoroutine = StartCoroutine(HitTick());
        }
        if(m_flashCoroutine == null)
        {
            m_flashCoroutine = StartCoroutine(DamageFlash());
        }
        StartCoroutine(DamageFlash());
        ApplyKnockback(_knockBackStrength, _direction);
    }

    void ApplyKnockback(float _knockBackStrength, Vector3 _direction)
    {
        if (m_rigidbody != null)
        {
            m_rigidbody.AddForce(_direction * _knockBackStrength, ForceMode.Impulse);
        }
    }
}