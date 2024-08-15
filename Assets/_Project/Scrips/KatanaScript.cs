using UnityEngine;

public class KatanaScript : MonoBehaviour
{
    Animator animator;
    [SerializeField] Transform raySpawn;
    [SerializeField] float reach;
    [SerializeField] Collider katanaCollider;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");

        //check if you hit anything with a raycast and run different command in different objects you hit
        RaycastHit hitInfo;
        bool hit = Physics.Raycast(raySpawn.position, raySpawn.forward, out hitInfo, reach);

        if (hit)
        {
            Debug.Log("Raycast hit something: " + hitInfo.collider.name);
            if (hitInfo.collider.gameObject.CompareTag("Player"))
            {
                GameObject attackedPlayer = hitInfo.collider.gameObject;
                PlayerMovement attackedPlayerScript = attackedPlayer.GetComponent<PlayerMovement>();
            }
            if (hitInfo.collider.gameObject.CompareTag("Rope"))
            {
                GameObject rope = hitInfo.collider.gameObject;
                RopeScript scriptRope = rope.GetComponent<RopeScript>();

                if (scriptRope != null)
                {
                    scriptRope.isCut = true;
                }
            }

        }
        else
        {
            Debug.Log("Raycast did not hit anything.");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(raySpawn.position, raySpawn.position + raySpawn.forward * reach);
    }
}
