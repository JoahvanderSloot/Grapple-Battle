using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAttacks : MonoBehaviour
{
    [Header("Keybinds")]
    public KeyCode m_AttackKey;
    public KeyCode m_GrappleKey;

    public KeyCode m_Slot1Key;
    public KeyCode m_Slot2Key;

    [Header("Items")]
    [SerializeField] GameObject m_katana;
    [SerializeField] GameObject m_star;
    [SerializeField] int m_itemSlot = 1;

    [Header("Shooting")]
    [SerializeField] Transform m_orientation;

    [SerializeField] GameObject m_starPref;
    public int m_StarCount = 10;

    [SerializeField] GameObject m_playerCam;

    [SerializeField] GrapplingHook m_grapplingHookScript;

    bool m_isGrappling = false;

    [Header("Cooldown")]
    [SerializeField] float m_attackCooldown = 0.25f;
    float m_lastAttackTime = 0f;

    private void Start()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            UImanager _UI = FindObjectOfType<UImanager>();
            if (_UI != null)
            {
                _UI.SetPlayer(gameObject);
            }
        }

        m_AttackKey = playerSettings.Instance.attack;
        m_GrappleKey = playerSettings.Instance.grapple;

        m_Slot1Key = playerSettings.Instance.slot1;
        m_Slot2Key = playerSettings.Instance.slot2;
    }

    void Update()
    {
        CurrentItem();

        if (Input.GetKeyDown(m_AttackKey) && Time.time >= m_lastAttackTime + m_attackCooldown && !m_isGrappling)
        {
            m_lastAttackTime = Time.time;

            if (m_itemSlot == 1)
            {
                KatanaScript _katanaScript = m_katana.GetComponent<KatanaScript>();
                _katanaScript.Attack();
            }
            else if (m_itemSlot == 2 && m_StarCount >= 1)
            {
                GameObject _star = Instantiate(m_starPref, m_playerCam.transform.position, m_playerCam.transform.rotation);
                NinjaStar _starScript = _star.GetComponent<NinjaStar>();
                _starScript.m_player = gameObject;
                m_StarCount--;
            }
        }

        if(m_isGrappling && Input.GetKeyDown(m_AttackKey))
        {
            Vector3 _playerToGrapple = m_grapplingHookScript.GetGrapplePoint() - m_playerCam.transform.position;
            _playerToGrapple.Normalize();

            GetComponent<Rigidbody>().AddForce(_playerToGrapple * m_grapplingHookScript.GetGrappleForce(), ForceMode.Impulse);

            m_grapplingHookScript.StopGrapple();
            m_isGrappling = false;
        }

        if (Input.GetKeyDown(m_GrappleKey) && m_grapplingHookScript.m_CanGrapple)
        {
            m_grapplingHookScript.StartGrapple();
            m_isGrappling = true;

        }
        if (Input.GetKeyUp(m_GrappleKey))
        {
            m_grapplingHookScript.StopGrapple();
            m_isGrappling = false;
        }
    }

    private void CurrentItem()
    {
        if (Input.mouseScrollDelta.y < 0)
        {
            m_itemSlot--;
        }
        else if (Input.mouseScrollDelta.y > 0)
        {
            m_itemSlot++;
        }

        if (Input.GetKeyDown(m_Slot1Key))
        {
            m_itemSlot = 1;
        }
        else if (Input.GetKeyDown(m_Slot2Key))
        {
            m_itemSlot = 2;
        }

        if(m_StarCount <= 0)
        {
            m_itemSlot = 1;
        }

        if (m_itemSlot > 2)
        {
            m_itemSlot = 1;
        }
        else if (m_itemSlot < 1)
        {
            m_itemSlot = 2;
        }

        if (m_itemSlot == 1)
        {
            m_katana.SetActive(true);
            m_star.SetActive(false);
        }
        else if (m_itemSlot == 2)
        {
            m_katana.SetActive(false);
            if (m_StarCount > 0)
            {
                m_star.SetActive(true);
            }
            else
            {
                m_star.SetActive(false);
            }
        }
    }
}
