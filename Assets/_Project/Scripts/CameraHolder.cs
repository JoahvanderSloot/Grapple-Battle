using Photon.Pun;
using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    public Transform m_CameraPosition;
    void Update()
    {
        transform.position = m_CameraPosition.position;
    }
}
