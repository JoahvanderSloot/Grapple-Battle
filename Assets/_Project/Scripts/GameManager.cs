using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public static GameManager Instance;

    public GameSettings m_GameSettings;

    public GameObject LocalPlayer;

    public GameObject PauseMenuObj;
    public GameObject ResultObj;
    public GameObject WaitingObj;
    public bool IsPaused;
    public bool IsRunning;
    public bool IsResult;

    private void Awake()
    {
        Instance = this;
        IsRunning = false;
    }

    private void Start()
    {
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            SceneManager.LoadScene("Title");
            return;
        }
        LocalPlayer = PhotonNetwork.Instantiate("Player", Vector3.zero + new Vector3(0, 1f, 0), Quaternion.identity);
        m_GameSettings.m_GameTimer = m_GameSettings.m_GameTime;

        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(3, null, raiseEventOptions, SendOptions.SendReliable);
        }
        WaitingObj.SetActive(true);
    }

    private void Update()
    {
        if (!PhotonNetwork.IsConnectedAndReady)
            return;

        // niet genoeg players? We wachten op players...
        if (PhotonNetwork.CurrentRoom.PlayerCount < PhotonNetwork.CurrentRoom.MaxPlayers && !IsResult && IsRunning)
        {
            if (!ResultObj.activeInHierarchy)
                ResultObj.SetActive(true);
        }
        else
        {
            // wil de local speler pauzeren, dat kan...
            if (Input.GetKeyDown(KeyCode.Escape) && !IsResult)
            {
                PauseMenuObj.SetActive(!PauseMenuObj.activeInHierarchy);
                IsPaused = PauseMenuObj.activeInHierarchy;
            }

            if (IsPaused || IsResult || WaitingObj.activeInHierarchy)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            if (IsResult)
            {
                PauseMenuObj.SetActive(false);
            }
            if (IsRunning)
            {
                WaitingObj.SetActive(false);
            }
            if (WaitingObj.activeInHierarchy)
            {
                PauseMenuObj.SetActive(false);
                IsPaused = false;
            }

            if(m_GameSettings.m_GameTime < 0)
            {
                m_GameSettings.m_GameTime = 0;
            }
        }
    }

    private IEnumerator GameTimer()
    {
        while (PhotonNetwork.CurrentRoom.PlayerCount >= PhotonNetwork.CurrentRoom.MaxPlayers && !IsResult && IsRunning)
        {
            yield return new WaitForSeconds(1);
            LocalPlayer.GetComponent<HitPoints>().Timer();
        }
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        SceneManager.LoadScene("Title");
    }

    public void QuitGame()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

        PhotonNetwork.RaiseEvent(1, null, raiseEventOptions, SendOptions.SendReliable);
    }

    public void ResumeGame()
    {
        PauseMenuObj.SetActive(false);
        IsPaused = PauseMenuObj.activeInHierarchy;
    }

    public void GiveUp()
    {
        LocalPlayer.GetComponent<HitPoints>().OnKilled();
    }

    public void NewMatch()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

        PhotonNetwork.RaiseEvent(2, null, raiseEventOptions, SendOptions.SendReliable);
    }

    // EVENTS
    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
        base.OnEnable();
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
        base.OnDisable();
    }

    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            case 0: // give up
                ResultObj.GetComponentInChildren<TextMeshProUGUI>().text = "GAME OVER";
                IsResult = true;
                ResultObj.SetActive(true);
                PauseMenuObj.SetActive(false);
                IsPaused = false;
                break;
            case 1: // quit
                PhotonNetwork.LeaveRoom();
                break;
            case 2: // restart
                IsResult = false;
                ResultObj.SetActive(false);

                PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);

                PhotonNetwork.LoadLevel("Game");
                break;
            case 3: //start
                IsRunning = true;
                if (PhotonNetwork.IsMasterClient)
                {
                    StartCoroutine(GameTimer());
                }
                break;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        if (PhotonNetwork.CurrentRoom.PlayerCount < PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            IsResult = true;
            ResultObj.SetActive(true);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        IsRunning = true;
        if (PhotonNetwork.IsMasterClient)
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(3, null, raiseEventOptions, SendOptions.SendReliable);
        }
    }
}
