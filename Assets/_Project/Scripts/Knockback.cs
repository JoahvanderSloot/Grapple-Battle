using UnityEngine;

public class Knockback : HitPoints
{
    int m_oldHitPoints;
    public Vector3 m_KbDirection;

    private void Update()
    {
        DestroyOnKill();
    }

    public void AddKnockback(float _knockBackStrength)
    {
        Rigidbody _rb = GetComponent<Rigidbody>();
        _rb.AddForce(m_KbDirection * _knockBackStrength, ForceMode.Impulse);
    }
}
