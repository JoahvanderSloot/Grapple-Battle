using UnityEngine;

public class Knockback : HitPoints
{
    private int oldHitPoints;
    public Vector3 kbDirection;

    private void Update()
    {
        DestroyOnKill();
    }

    public void AddKnockback(float knockBackStrength)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(kbDirection * knockBackStrength, ForceMode.Impulse);
    }
}
