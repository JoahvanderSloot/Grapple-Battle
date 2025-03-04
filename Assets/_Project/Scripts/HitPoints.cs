using Photon.Pun.Demo.PunBasics;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            if(gameObject.tag == "Player")
            {
                GameManager _gameManager = GameManager.FindFirstObjectByType<GameManager>();
                _gameManager.m_GameSettings.m_OutCome = 2;
                SceneManager.LoadScene("GameOver");
            }
            Destroy(gameObject);
        }
    }
}
