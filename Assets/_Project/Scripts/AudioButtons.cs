using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioButtons : MonoBehaviour
{
    [SerializeField] private Button audioButton;
    [SerializeField] private Button musicButton;

    [SerializeField] private List<Sprite> m_images;

    [SerializeField] bool m_isAudioButton;

    [SerializeField] GameSettings m_gameSettings;

    private void Start()
    {
        if (m_isAudioButton)
        {
            UpdateButtonSprite(audioButton, m_gameSettings.m_Audio);
        }
        else
        {
            UpdateButtonSprite(musicButton, m_gameSettings.m_Music);
        }
    }

    public void PointerEnter()
    {
        transform.localScale = new Vector2(1.05f, 1.05f);
        AudioManager.m_Instance.Play("Hover");
    }

    public void PointerExit()
    {
        transform.localScale = new Vector2(1f, 1f);
    }

    public void AudioButton()
    {
        if (audioButton != null)
        {
            AudioManager.m_Instance.Play("Hover");
            m_gameSettings.m_Audio = !m_gameSettings.m_Audio;
            UpdateButtonSprite(audioButton, m_gameSettings.m_Audio);
        }
    }

    public void MusicButton()
    {
        if (musicButton != null)
        {
            m_gameSettings.m_Music = !m_gameSettings.m_Music;
            UpdateButtonSprite(musicButton, m_gameSettings.m_Music);

            if (m_gameSettings.m_Music)
            {
                AudioManager.m_Instance.Play("MenuMusic");
            }
            else
            {
                AudioManager.m_Instance.StopAllSounds();
            }

            AudioManager.m_Instance.Play("Click");
        }
    }

    private void UpdateButtonSprite(Button button, bool isActive)
    {
        if (button != null && m_images != null && m_images.Count >= 2)
        {
            button.image.sprite = isActive ? m_images[1] : m_images[0];
        }
    }
}
