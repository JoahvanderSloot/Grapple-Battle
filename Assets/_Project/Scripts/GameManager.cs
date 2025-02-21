using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameSettings m_GameSettings;
    Coroutine m_gameTimeCoroutine;
    public bool m_Paused;
    [SerializeField] Canvas m_canvas;

    private void Awake()
    {
        m_canvas.gameObject.SetActive(false);
    }

    private void Start()
    {
        m_GameSettings.m_GameTimer = m_GameSettings.m_GameTime;
        m_Paused = false;

        if (m_gameTimeCoroutine == null)
        {
            m_gameTimeCoroutine = StartCoroutine(GameTimer());
        }
    }

    private void Update()
    {
        if(m_GameSettings.m_GameTimer <= 0)
        {
            m_GameSettings.m_OutCome = 0;
            SceneManager.LoadScene("GameOver");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            m_Paused = !m_Paused;
        }
    }

    private IEnumerator GameTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            m_GameSettings.m_GameTimer--;
        }
    }
}
