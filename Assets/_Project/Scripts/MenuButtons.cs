using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void PointerEnter()
    {
        transform.localScale = new Vector2(1.1f, 1.1f);
    }

    public void PointerExit()
    {
        transform.localScale = new Vector2(1f, 1f);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void Settings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void Forfeit()
    {
        HitPoints _playerHP = GameObject.FindGameObjectWithTag("Player").GetComponent<HitPoints>();
        if (_playerHP != null)
        {
            _playerHP.m_HP = 0;
        }
    }

    public void QuitGame()
    {
        #if (UNITY_EDITOR || DEVELOPMENT_BUILD)
            Debug.Log(this.name + " : " + this.GetType() + " : " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        #endif
        #if (UNITY_EDITOR)
            UnityEditor.EditorApplication.isPlaying = false;
        #elif (UNITY_STANDALONE)
             Application.Quit();
        #elif (UNITY_WEBGL)
             Application.OpenURL("itch url ");
        #endif
    }
}
