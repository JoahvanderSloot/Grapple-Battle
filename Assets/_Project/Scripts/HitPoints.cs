using UnityEngine;

public class HitPoints : MonoBehaviour
{
    public int m_HP = 10;

    private void Update()
    {
        DestroyOnKill();
    }

    protected void DestroyOnKill()
    {
        if (m_HP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
