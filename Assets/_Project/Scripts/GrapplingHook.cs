using UnityEngine;

public class GrapplingHook : MonoBehaviour
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
            m_CanGrapple= false;
        }
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    public void StartGrapple()
    {
        RaycastHit _hit;
        if(Physics.Raycast(m_Camera.position, m_Camera.forward, out _hit, m_maxDistance, m_WhatIsGround))
        {
            m_grapplePoint = _hit.point;
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
        }
    }

    void DrawRope()
    {
        if(!m_joint) return;

        m_lr.SetPosition(0, m_GunTip.position);
        m_lr.SetPosition(1, m_grapplePoint);
    }

    public void StopGrapple()
    {
        m_lr.positionCount = 0;
        Destroy(m_joint);
    }
}
