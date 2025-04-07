using UnityEngine;
using Photon.Pun;

public class GrapplingHook : MonoBehaviourPun
{
    LineRenderer m_lr;
    Vector3 m_grapplePoint;
    public LayerMask m_WhatIsGround;
    public Transform m_GunTip, m_Camera, m_Player;
    [SerializeField] float m_maxDistance = 100f;
    SpringJoint m_joint;

    public bool m_CanGrapple = false;

    public Vector3 GetGrapplePoint()
    {
        return m_grapplePoint;
    }

    public float GetGrappleForce()
    {
        return 30f;
    }

    private void Awake()
    {
        m_lr = GetComponent<LineRenderer>();
        StopGrapple();
    }

    private void Update()
    {
        RaycastHit _hit;
        if (Physics.Raycast(m_Camera.position, m_Camera.forward, out _hit, m_maxDistance, m_WhatIsGround))
        {
            m_CanGrapple = true;
        }
        else
        {
            m_CanGrapple = false;
        }
    }

    private void LateUpdate()
    {
        if (photonView.IsMine)
        {
            DrawRope();

            if (m_joint != null)
            {
                photonView.RPC("DrawRopeRPC", RpcTarget.Others, m_GunTip.position, m_grapplePoint);
            }
        }
    }

    [PunRPC]
    private void StartGrappleRPC(Vector3 _grapplePoint)
    {
        m_grapplePoint = _grapplePoint;
        m_joint = m_Player.gameObject.AddComponent<SpringJoint>();
        m_joint.autoConfigureConnectedAnchor = false;
        m_joint.connectedAnchor = m_grapplePoint;

        float _distanceFromPoint = Vector3.Distance(m_Player.position, m_grapplePoint);

        m_joint.maxDistance = _distanceFromPoint * 0.8f;
        m_joint.minDistance = _distanceFromPoint * 0.25f;

        m_joint.spring = 4.5f;
        m_joint.damper = 7f;
        m_joint.massScale = 4.5f;

        m_lr.positionCount = 2;
        photonView.RPC("DrawRopeRPC", RpcTarget.AllBuffered, m_GunTip.position, m_grapplePoint);
    }

    [PunRPC]
    private void StopGrappleRPC()
    {
        if (m_lr.positionCount > 0)
        {
            m_lr.positionCount = 0;
        }
        Destroy(m_joint);
    }

    [PunRPC]
    private void DrawRopeRPC(Vector3 _gunTipPosition, Vector3 _grapplePoint)
    {
        m_grapplePoint = _grapplePoint;

        if (m_lr.positionCount != 2)
        {
            m_lr.positionCount = 2;
        }

        m_lr.SetPosition(0, _gunTipPosition);
        m_lr.SetPosition(1, _grapplePoint);
    }

    public void StartGrapple()
    {
        if (photonView.IsMine)
        {
            RaycastHit _hit;
            if (Physics.Raycast(m_Camera.position, m_Camera.forward, out _hit, m_maxDistance, m_WhatIsGround))
            {
                m_grapplePoint = _hit.point;
                photonView.RPC("StartGrappleRPC", RpcTarget.All, m_grapplePoint);
            }
        }
    }

    private void DrawRope()
    {
        if (!m_joint || m_lr.positionCount != 2) return;

        m_lr.SetPosition(0, m_GunTip.position);
        m_lr.SetPosition(1, m_grapplePoint);
    }

    public void StopGrapple()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("StopGrappleRPC", RpcTarget.All);
        }
    }
}
