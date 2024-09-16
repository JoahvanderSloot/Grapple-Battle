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
            HitPoints hpScript = hitInfo.collider.GetComponent<HitPoints>();
            if (hpScript != null)
            {
                hpScript.HP -= 2;
            }
            else
            {
                Debug.Log("Object does not have HP script");
            }


        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(raySpawn.position, raySpawn.position + raySpawn.forward * reach);
    }
}
