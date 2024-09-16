using UnityEngine;

public class HitPoints : MonoBehaviour
{
    public int HP = 10;

    private void Update()
    {
        if(HP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
