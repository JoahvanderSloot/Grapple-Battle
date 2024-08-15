using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    [Header("Keybinds")]
    public KeyCode attackKey = KeyCode.Mouse0;
    public KeyCode grappleKey = KeyCode.Mouse1;

    public KeyCode Slot1 = KeyCode.Alpha1;
    public KeyCode Slot2 = KeyCode.Alpha2;

    [Header("Items")]
    [SerializeField] GameObject katana;
    [SerializeField] GameObject star;
    [SerializeField] int itemSlot = 1;

    [Header("Shooting")]
    [SerializeField] Transform orientation;

    [SerializeField] GameObject grapplingHook;

    [SerializeField] GameObject starPref;
    [SerializeField] int starCount = 10;

    [SerializeField] GameObject playerCam;

    [SerializeField] GrapplingHook grapplingHookScript;

    bool isGrappling = false;

    [Header("Cooldown")]
    [SerializeField] float attackCooldown = 0.5f;
    private float lastAttackTime = 0f;

    void Update()
    {
        CurrentItem();

        if (Input.GetKeyDown(attackKey) && Time.time >= lastAttackTime + attackCooldown && !isGrappling)
        {
            lastAttackTime = Time.time;

            if (itemSlot == 1)
            {
                KatanaScript katanaScript = katana.GetComponent<KatanaScript>();
                katanaScript.Attack();
            }
            else if (itemSlot == 2 && starCount >= 1)
            {
                Instantiate(starPref, playerCam.transform.position, playerCam.transform.rotation);
                starCount--;
            }
        }

        if(isGrappling && Input.GetKeyDown(attackKey))
        {
            Vector3 playerToGrapple = grapplingHookScript.GetGrapplePoint() - playerCam.transform.position;
            playerToGrapple.Normalize();

            GetComponent<Rigidbody>().AddForce(playerToGrapple * grapplingHookScript.GetGrappleForce(), ForceMode.Impulse);

            grapplingHookScript.StopGrapple();
            isGrappling = false;
        }

        if (Input.GetKeyDown(grappleKey))
        {
            grapplingHookScript.StartGrapple();
            isGrappling = true;
            
        }
        if (Input.GetKeyUp(grappleKey))
        {
            grapplingHookScript.StopGrapple();
            isGrappling = false;
        }
    }

    private void CurrentItem()
    {
        //scroll and hotbar logic
        if (Input.mouseScrollDelta.y < 0)
        {
            itemSlot--;
        }
        else if (Input.mouseScrollDelta.y > 0)
        {
            itemSlot++;
        }

        if (Input.GetKeyDown(Slot1))
        {
            itemSlot = 1;
        }
        else if (Input.GetKeyDown(Slot2))
        {
            itemSlot = 2;
        }

        if(starCount <= 0)
        {
            itemSlot = 1;
        }

        if (itemSlot > 2)
        {
            itemSlot = 1;
        }
        else if (itemSlot < 1)
        {
            itemSlot = 2;
        }

        //item visualization logic
        if (itemSlot == 1)
        {
            katana.SetActive(true);
            star.SetActive(false);
        }
        else if (itemSlot == 2)
        {
            katana.SetActive(false);
            if (starCount > 0)
            {
                star.SetActive(true);
            }
            else
            {
                star.SetActive(false);
            }
        }
    }
}
