using UnityEngine;

public class StartRefill : MonoBehaviour
{
    bool m_onCooldown;
    float m_timer;
    [SerializeField] float m_cooldown;

    [SerializeField] GameObject m_orb;

    private void Start()
    {
        m_onCooldown = false;
        m_timer = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerAttacks _playerAttack = other.gameObject.GetComponentInParent<PlayerAttacks>();
        if (_playerAttack != null && !m_onCooldown)
        {
            if(_playerAttack.m_StarCount < 10)
            {
                AudioManager.m_Instance.Play("Pickup");
                _playerAttack.m_StarCount = 10;
                m_onCooldown = true;
            }
        }
    }

    private void Update()
    {
        if (m_onCooldown)
        {
            m_orb.SetActive(false);
            m_timer += Time.deltaTime * 2;
            if(m_timer >= m_cooldown)
            {
                AudioManager.m_Instance.Play("Recharge");
                m_onCooldown = false;
                m_timer = 0;
                m_orb.SetActive(true);
            }
        }
    }
}
