using UnityEngine;

public class PlayerTurn : MonoBehaviour
{
    public Transform playerPos;

    private void Update()
    {
        playerPos.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

}