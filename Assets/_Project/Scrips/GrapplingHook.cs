using JetBrains.Annotations;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatIsGround;
    public Transform gunTip, camera, player;
    private float maxDistance = 100f;
    private SpringJoint joint;

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
    public float GetGrappleForce()
    {
        return 30f;
    }

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        StopGrapple();
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    public void StartGrapple()
    {
        RaycastHit hit;
        if(Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, whatIsGround))
        {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
        }
    }

    void DrawRope()
    {
        if(!joint) return;

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, grapplePoint);
    }

    public void StopGrapple()
    {
        lr.positionCount = 0;
        Destroy(joint);
    }
}
