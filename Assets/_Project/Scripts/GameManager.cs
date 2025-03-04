using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameSettings m_GameSettings;
    Coroutine m_gameTimeCoroutine;
    public bool m_Paused;
    [SerializeField] Canvas m_canvas;

    [SerializeField] List<GameObject> m_players;
    public bool m_GameStart;

    private void Awake()
    {
        m_GameStart = false;
        m_canvas.gameObject.SetActive(false);
        m_players = new List<GameObject>();
        StartCoroutine(CheckForPlayers());
    }

    private void Start()
    {
        m_GameSettings.m_GameTimer = m_GameSettings.m_GameTime;
        m_Paused = false;
    }

    private void Update()
    {
        if (m_GameSettings.m_GameTimer <= 0)
        {
            m_GameSettings.m_OutCome = 0;
            SceneManager.LoadScene("GameOver");
        }

        if(m_players.Count == 1 && m_GameStart)
        {
            m_GameSettings.m_OutCome = 1;
            SceneManager.LoadScene("GameOver");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            m_Paused = !m_Paused;
        }
    }

    private IEnumerator CheckForPlayers()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);

            m_players.RemoveAll(player => player == null);

            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            m_players.Clear();
            m_players.AddRange(players);

            if (m_players.Count >= 2 && !m_GameStart)
            {
                m_GameStart = true;
                if (m_gameTimeCoroutine == null)
                {
                    m_gameTimeCoroutine = StartCoroutine(GameTimer());
                }
            }

            if (m_players.Count == 0)
            {
                m_GameStart = false;
            }
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
