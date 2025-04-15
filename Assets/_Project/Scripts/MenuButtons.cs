using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void PointerEnter()
    {
        AudioManager.m_Instance.Play("Hover");
        transform.localScale = new Vector2(1.1f, 1.1f);
    }

    public void PointerExit()
    {
        transform.localScale = new Vector2(1f, 1f);
    }

    public void MainMenu()
    {
        AudioManager.m_Instance.Play("Click");
        SceneManager.LoadScene("Title");
    }

    public void Settings()
    {
        AudioManager.m_Instance.Play("Click");
        SceneManager.LoadScene("Settings");
    }


    public void QuitGame()
    {
        AudioManager.m_Instance.Play("Click");
        #if (UNITY_EDITOR || DEVELOPMENT_BUILD)
            Debug.Log(this.name + " : " + this.GetType() + " : " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        #endif
        #if (UNITY_EDITOR)
            UnityEditor.EditorApplication.isPlaying = false;
        #elif (UNITY_STANDALONE)
             Application.Quit();
        #elif (UNITY_WEBGL)
             Application.OpenURL("https://joahvds.itch.io/grapple-battle");
        #endif
    }
}
