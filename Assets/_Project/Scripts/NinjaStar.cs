using UnityEngine;

public class NinjaStar : MonoBehaviour
{
    [Header("Shooting")]
    Rigidbody rb;
    [SerializeField] float shootSpeed;
    [SerializeField] float kbStrength;

    GameObject playerCam;
    Vector3 moveDirection;
    bool canDestroy = false;

    void Start()
    {
        playerCam = GameObject.FindWithTag("PlayerCam");

        moveDirection = playerCam.transform.forward;

        rb = GetComponent<Rigidbody>();
        rb.AddForce(moveDirection.normalized * shootSpeed * 10f, ForceMode.Force);

        Destroy(gameObject, 4);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canDestroy)
        {
            Knockback knockbackScript = other.GetComponent<Knockback>();
            if (knockbackScript != null)
            {
                knockbackScript.AddKnockback(kbStrength);
                knockbackScript.kbDirection = rb.velocity.normalized;
                knockbackScript.HP--;
            }
            else
            {
                HitPoints hpScript = other.GetComponent<HitPoints>();
                if (hpScript != null)
                {
                    hpScript.HP--;
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
        canDestroy = true;
    }
}
