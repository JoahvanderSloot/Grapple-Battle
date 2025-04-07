using UnityEngine;

public class StartRefill : MonoBehaviour
{
    bool m_onCooldown;
    float m_timer;
    [SerializeField] float m_cooldown;

    [SerializeField] Material m_normalColor;
    [SerializeField] Material m_cooldownColor;

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
                _playerAttack.m_StarCount = 10;
                m_onCooldown = true;
            }
        }
    }

    private void Update()
    {
        if (m_onCooldown)
        {
            MeshRenderer _meshRenderer = GetComponent<MeshRenderer>();
            _meshRenderer.material = m_cooldownColor;
            m_timer += Time.deltaTime * 2;
            if(m_timer >= m_cooldown)
            {
                m_onCooldown = false;
                m_timer = 0;
                _meshRenderer.material = m_normalColor;
            }
        }
    }
}
