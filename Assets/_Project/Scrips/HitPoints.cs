using UnityEngine;

public class HitPoints : MonoBehaviour
{
    public int HP = 10;

    private void Update()
    {
        DestroyOnKill();
    }

    protected void DestroyOnKill()
    {
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
