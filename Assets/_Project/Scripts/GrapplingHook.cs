using UnityEngine;
using Photon.Pun;

public class GrapplingHook : MonoBehaviourPun
{
    public LineRenderer m_lr;
    public Vector3 m_grapplePoint;
    public LayerMask m_WhatIsGround;
    public Transform m_GunTip, m_Camera;
    [SerializeField] float m_maxDistance = 100f;
    public SpringJoint m_joint;

    public bool m_CanGrapple = false;

    [SerializeField] PhotonView m_photonView;

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
        if (m_joint != null && m_grapplePoint != Vector3.zero)
        {
            DrawRope();
            DrawRopeRPC(m_GunTip.position, m_grapplePoint);
        }

    }


    public void DrawRopeRPC(Vector3 _gunTipPosition, Vector3 _grapplePoint)
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
        if (m_photonView.IsMine)
        {
            RaycastHit _hit;
            if (Physics.Raycast(m_Camera.position, m_Camera.forward, out _hit, m_maxDistance, m_WhatIsGround))
            {
                m_grapplePoint = _hit.point;
                m_photonView.RPC("StartGrappleRPC", RpcTarget.All, m_grapplePoint);
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
        if (m_photonView.IsMine)
        {
            m_photonView.RPC("StopGrappleRPC", RpcTarget.All);
        }
    }
}
