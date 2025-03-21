using Photon.Pun;
using UnityEngine;

public class CameraHolder : MonoBehaviourPunCallbacks
{
    public Transform m_CameraPosition;
    void Update()
    {
        transform.position = m_CameraPosition.position;
    }
}
