using UnityEngine;

public class RopeScript : MonoBehaviour
{
    public bool m_IsCut = false;

    void Update()
    {
        if (m_IsCut)
        {
            Destroy(gameObject, 0.15f);
        }
    }
}
