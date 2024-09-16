using UnityEngine;

public class NinjaStar : MonoBehaviour
{
    [Header("Shooting")]
    Rigidbody rb;
    [SerializeField] float shootSpeed;

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
            HitPoints hpScript = other.GetComponent<HitPoints>();
            if (hpScript != null)
            {
                hpScript.HP--;
            }
            else
            {
                Debug.Log("Object does not have HP script");
            }

            Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canDestroy = true;
    }
}
