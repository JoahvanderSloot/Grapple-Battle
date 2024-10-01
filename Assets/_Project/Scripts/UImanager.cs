using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    [Header("In Gam eObjects")]
    [SerializeField] GameObject player;
    [SerializeField] GameObject grapplingGun;

    [Header("UI Objects")]
    [SerializeField] Image crossHair;

    void Update()
    {
        CrossHair();
    }

    private void CrossHair()
    {
        GrapplingHook grapplingHookScript = grapplingGun.GetComponent<GrapplingHook>();
        if (grapplingHookScript.canGrapple)
        {
            crossHair.color = Color.green;
        }
        else
        {
            crossHair.color = Color.black;
        }
    }
}
