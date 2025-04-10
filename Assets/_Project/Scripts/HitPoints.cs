using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HitPoints : MonoBehaviourPun
{
    public int m_HP = 10;
    protected Coroutine m_hitCoroutine;
    protected Coroutine m_flashCoroutine;
    public ParticleSystem m_Blood;
    [SerializeField] Color m_bodyDamageColor;
    [SerializeField] Material m_bodyMaterial;
    public PhotonView m_PhotonView;
    bool m_drawSet;

    private void Awake()
    {
        m_PhotonView = photonView;
        m_drawSet = false;
    }

    private void Update()
    {
        DestroyOnKill();

        if (GameManager.Instance.m_GameSettings.m_GameTimer <= 0 && PhotonNetwork.IsMasterClient && !m_drawSet)
        {
            OnDraw();
        }
    }

    public void Timer()
    {
        GameManager.Instance.m_GameSettings.m_GameTimer--;
        photonView.RPC("SetTimer", RpcTarget.Others, GameManager.Instance.m_GameSettings.m_GameTimer);
    }

    [PunRPC]
    public void SetTimer(int _gameTime)
    {
        GameManager.Instance.m_GameSettings.m_GameTimer = _gameTime;
    }

    protected void DestroyOnKill()
    {
        if (m_HP <= 0 && !GameManager.Instance.IsResult)
        {
            if (gameObject.CompareTag("Player") && photonView.IsMine)
            {
                OnKilled();
            }
        }
    }

    public void OnKilled()
    {
        photonView.RPC("SetWinner", RpcTarget.Others, true);
        SetWinner(false);
    }

    public void OnDraw()
    {
        photonView.RPC("SetDraw", RpcTarget.Others);
        SetDraw();
        m_drawSet = true;
    }


    [PunRPC]
    public void SetWinner(bool _isWinner)
    {
        GameManager.Instance.IsResult = true;
        GameManager.Instance.ResultObj.GetComponentInChildren<TextMeshProUGUI>().text = _isWinner ? "YOU WIN!" : "YOU LOSE!";
        GameManager.Instance.ResultObj.SetActive(true);
        AudioManager.m_Instance.Play("Game");
        AudioManager.m_Instance.Stop("Footsteps");
    }

    [PunRPC]
    public void SetDraw()
    {
        GameManager.Instance.IsResult = true;
        GameManager.Instance.ResultObj.GetComponentInChildren<TextMeshProUGUI>().text = "DRAW!";
        GameManager.Instance.ResultObj.SetActive(true);
        AudioManager.m_Instance.Play("Game");
        AudioManager.m_Instance.Stop("Footsteps");
    }

    protected IEnumerator HitTick()
    {
        m_bodyMaterial.color = m_bodyDamageColor;
        m_Blood.Play();
        AudioManager.m_Instance.Play("Damage");

        yield return new WaitForSeconds(0.2f);

        m_bodyMaterial.color = Color.black;
        m_Blood.Stop();

        StopCoroutine(m_hitCoroutine);
        m_hitCoroutine = null;
    }

    protected IEnumerator DamageFlash()
    {
        if (!photonView.IsMine) yield break;

        for (float t = 0; t < 0.2f; t += Time.deltaTime)
        {
            Color _color = GameManager.Instance.DamageFlash.color;
            _color.a = Mathf.Lerp(0, 0.5f, t / 0.2f);
            GameManager.Instance.DamageFlash.color = _color;
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        for (float t = 0; t < 0.2f; t += Time.deltaTime)
        {
            Color _color = GameManager.Instance.DamageFlash.color;
            _color.a = Mathf.Lerp(0.5f, 0, t / 0.2f);
            GameManager.Instance.DamageFlash.color = _color;
            yield return null;
        }

        StopCoroutine(m_flashCoroutine);
    }
}
