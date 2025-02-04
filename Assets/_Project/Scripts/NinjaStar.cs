using UnityEngine;

public class NinjaStar : MonoBehaviour
{
    [Header("Shooting")]
    Rigidbody m_rb;
    [SerializeField] float m_shootSpeed;
    [SerializeField] float m_kbStrength;

    GameObject m_playerCam;
    Vector3 m_moveDirection;
    bool m_canDestroy = false;

    void Start()
    {
        m_playerCam = GameObject.FindWithTag("PlayerCam");

        m_moveDirection = m_playerCam.transform.forward;

        m_rb = GetComponent<Rigidbody>();
        m_rb.AddForce(m_moveDirection.normalized * m_shootSpeed * 10f, ForceMode.Force);

        Destroy(gameObject, 4);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_canDestroy)
        {
            Knockback _knockbackScript = other.GetComponent<Knockback>();
            if (_knockbackScript != null)
            {
                _knockbackScript.AddKnockback(m_kbStrength);
                _knockbackScript.m_KbDirection = m_rb.velocity.normalized;
                _knockbackScript.m_HP--;
            }
            else
            {
                HitPoints _hpScript = other.GetComponent<HitPoints>();
                if (_hpScript != null)
                {
                    _hpScript.m_HP--;
                }
                else
                {
                    Debug.Log("Object does not have HP script");
                }
            }

            Destroy(gameObject);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        m_canDestroy = true;
    }
}
