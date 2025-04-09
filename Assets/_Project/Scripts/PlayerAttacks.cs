using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAttacks : MonoBehaviourPunCallbacks
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
    public int m_StarCount = 10;

    [SerializeField] GameObject m_playerCam;

    [SerializeField] GrapplingHook m_grapplingHookScript;
    [SerializeField] GameObject m_hook;

    bool m_isGrappling = false;

    [Header("Cooldown")]
    [SerializeField] float m_attackCooldown = 0.25f;
    float m_lastAttackTime = 0f;

    PlayerMovement m_movement;

    private void Start()
    {
        m_movement = gameObject.GetComponent<PlayerMovement>();

        m_AttackKey = playerSettings.Instance.attack;
        m_GrappleKey = playerSettings.Instance.grapple;

        m_Slot1Key = playerSettings.Instance.slot1;
        m_Slot2Key = playerSettings.Instance.slot2;
    }

    void Update()
    {
        if (!m_movement.m_inFocus || !photonView.IsMine || GameManager.Instance.IsPaused || GameManager.Instance.IsResult || !GameManager.Instance.IsRunning) return;
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
                GameObject _start = PhotonNetwork.Instantiate("Star", m_playerCam.transform.position, m_playerCam.transform.rotation);
                NinjaStar _starScript = _start.GetComponent<NinjaStar>();
                _starScript.m_OwnerID = photonView.Owner.ActorNumber;
                _starScript.photonView.RPC("SetOwner", RpcTarget.Others, photonView.Owner.ActorNumber);
                
                m_StarCount--;
            }
        }

        if (m_isGrappling && Input.GetKeyDown(m_AttackKey))
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
            photonView.RPC("SetHookActive", RpcTarget.All, false);
        }
        if (Input.GetKeyUp(m_GrappleKey))
        {
            m_grapplingHookScript.StopGrapple();
            m_isGrappling = false;
            photonView.RPC("SetHookActive", RpcTarget.All, true);
        }

        if (photonView.IsMine)
        {
            photonView.RPC("SetCurrentItem", RpcTarget.All, m_itemSlot);
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

        if (m_StarCount <= 0)
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

        m_hook.SetActive(!m_isGrappling);
    }

    [PunRPC]
    private void SetCurrentItem(int _itemSlot)
    {
        if (_itemSlot == 1)
        {
            m_katana.SetActive(true);
            m_star.SetActive(false);
        }
        else if (_itemSlot == 2)
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

    [PunRPC]
    private void SetHookActive(bool _active)
    {
        m_hook.SetActive(_active);
    }


    [PunRPC]
    public void StopGrappleRPC()
    {
        if (m_grapplingHookScript.m_lr.positionCount > 0)
        {
            m_grapplingHookScript.m_lr.positionCount = 0;
        }
        m_grapplingHookScript.m_grapplePoint = Vector3.zero;
        Destroy(m_grapplingHookScript.m_joint);
    }

    [PunRPC]
    private void StartGrappleRPC(Vector3 _grapplePoint)
    {
        m_grapplingHookScript.m_grapplePoint = _grapplePoint;
        m_grapplingHookScript.m_joint = gameObject.AddComponent<SpringJoint>();
        m_grapplingHookScript.m_joint.autoConfigureConnectedAnchor = false;
        m_grapplingHookScript.m_joint.connectedAnchor = m_grapplingHookScript.m_grapplePoint;

        float _distanceFromPoint = Vector3.Distance(transform.position, m_grapplingHookScript.m_grapplePoint);

        m_grapplingHookScript. m_joint.maxDistance = _distanceFromPoint * 0.8f;
        m_grapplingHookScript.m_joint.minDistance = _distanceFromPoint * 0.25f;

        m_grapplingHookScript.m_joint.spring = 4.5f;
        m_grapplingHookScript.m_joint.damper = 7f;
        m_grapplingHookScript.m_joint.massScale = 4.5f;

        m_grapplingHookScript.m_lr.positionCount = 2;

        m_grapplingHookScript.DrawRopeRPC(transform.position, m_grapplingHookScript.m_grapplePoint);
    }
}