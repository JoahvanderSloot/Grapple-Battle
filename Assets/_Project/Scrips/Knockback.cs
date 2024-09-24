using UnityEngine;

public class Knockback : HitPoints
{
    private int oldHitPoints;
    public Vector3 kbDirection;

    private void Start()
    {
        oldHitPoints = HP;
    }

    private void Update()
    {
        DestroyOnKill();

        if(HP < oldHitPoints)
        {
            oldHitPoints = HP;
        }
    }

    public void AddKnockback(float knockBackStrength)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(kbDirection * knockBackStrength, ForceMode.Impulse);
    }
}
