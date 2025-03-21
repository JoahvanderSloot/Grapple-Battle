using Photon.Pun;
using System.Collections;
using UnityEngine;

public class PlayerTurn : MonoBehaviourPunCallbacks
{
    public Transform m_PlayerPos;

    private void Update()
    {
        m_PlayerPos.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }
}