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
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(m_KbDirection * _knockBackStrength, ForceMode.Impulse);
    }
}
