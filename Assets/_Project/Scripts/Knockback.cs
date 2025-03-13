using Photon.Pun;
using UnityEngine;

public class Knockback : HitPoints
{
    private Rigidbody m_rigidbody;

    private void Start()
    {
        m_view = GetComponent<PhotonView>();
        m_rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        DestroyOnKill();
        GetGameManager();
    }

    [PunRPC]
    public void DamageOtherPlayer(float _knockBackStrength, Vector3 _direction, int _damage)
    {
        m_HP -= _damage;
        m_view.RPC("ApplyKnockbackRPC", RpcTarget.All, _knockBackStrength, _direction);
        
    }

    [PunRPC]
    void ApplyKnockbackRPC(float _knockBackStrength, Vector3 _direction)
    {
        if (m_rigidbody != null)
        {
            m_rigidbody.AddForce(_direction * _knockBackStrength, ForceMode.Impulse);
        }
    }
}
