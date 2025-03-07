using Photon.Pun;
using UnityEngine;

public class Knockback : HitPoints
{
    int m_oldHitPoints;
    Rigidbody m_rigidbody;

    private void Start()
    {
        m_view = GetComponent<PhotonView>();
        m_rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        GetGameManager();
        DestroyOnKill();
       
    }

    [PunRPC]
    public void DamageOtherPlayer(float _knockBackStrength, Vector3 _direction, int _damage)
    {
        m_HP -= _damage;
        m_rigidbody.AddForce(_direction * _knockBackStrength, ForceMode.Impulse);
    }
}