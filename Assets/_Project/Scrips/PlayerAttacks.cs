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

    [Header("Cooldown")]
    [SerializeField] float attackCooldown = 0.5f;
    private float lastAttackTime = 0f;

    void Update()
    {
        CurrentItem();

        if (Input.GetKeyDown(attackKey) && Time.time >= lastAttackTime + attackCooldown)
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

        if (Input.GetKeyDown(grappleKey))
        {
            // Shoot grappling hook
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
