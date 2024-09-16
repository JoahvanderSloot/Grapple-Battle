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
        Time.timeScale = 1f;
    }
}
