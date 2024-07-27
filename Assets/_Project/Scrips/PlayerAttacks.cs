using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    [Header("Keybinds")]
    public KeyCode attackKey = KeyCode.Mouse0;
    public KeyCode grappleKey = KeyCode.Mouse1;

    public KeyCode Slot1 = KeyCode.Alpha1;
    public KeyCode Slot2 = KeyCode.Alpha2;

    [Header("Items")]
    [SerializeField] GameObject grapplingHook;

    [SerializeField] enum PlayerState
    {
        Nothing,
        Katana,
        Star
    }

    private PlayerState currentState;

    private void Start()
    {
        currentState = PlayerState.Nothing;
    }

    void Update()
    {
        CurrentItem();

        if (Input.GetKeyDown(attackKey))
        {
            //atack logic
        }

        if (Input.GetKeyDown(grappleKey))
        {
            //shoot grapplinghook
        }
    }

    private void CurrentItem()
    {
        if (Input.GetKeyDown(Slot1))
        {
            currentState = PlayerState.Katana;
        }
        else if (Input.GetKeyDown(Slot2))
        {
            currentState = PlayerState.Star;
        }
    }
}
