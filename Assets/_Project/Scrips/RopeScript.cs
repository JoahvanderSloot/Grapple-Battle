using UnityEngine;

public class RopeScript : MonoBehaviour
{
    public bool isCut = false;

    void Update()
    {
        if (isCut)
        {
            Destroy(gameObject, 0.15f);
        }
    }
}
