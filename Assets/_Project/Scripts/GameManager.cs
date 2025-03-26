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
                PauseMenuObj.SetActive(!PauseMenuObj.activeInHierarchy); // toggle
                IsPaused = PauseMenuObj.activeInHierarchy;
            }

            if (IsPaused || IsResult)
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
        // we zijn de kamer uit, dus we gaan weg naar title
        SceneManager.LoadScene("Title");
    }

    public void QuitGame()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

        PhotonNetwork.RaiseEvent(1, null, raiseEventOptions, SendOptions.SendReliable);
    }

    public void ResumeGame()
    {
        // de speler wilt gewoon verder spelen, dus zet pauze scherm uit
        PauseMenuObj.SetActive(false); // toggle
        IsPaused = PauseMenuObj.activeInHierarchy;
    }

    public void GiveUp()
    {
        LocalPlayer.GetComponent<HitPoints>().OnKilled();
    }

    public void NewMatch()
    {
        // Stuur een signaal naar andere spelers dat een nieuw spel gaat beginnen
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // iedereen

        // de "2" is gewoon een random code nummer, die vangen we op in de switch-case van "OnEvent" functie (zie onder)
        PhotonNetwork.RaiseEvent(2, null, raiseEventOptions, SendOptions.SendReliable);
    }

    // EVENTS
    // we voegen event functionaliteit toe
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

    // hier vangen we globale events op, zoals als spelers iets doen wat invloed op de hele game heeft
    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code) // we checken welke code dit event heeft (zie PhotonNetwork.RaiseEvent boven)
        {
            case 0: // give up
                ResultObj.GetComponentInChildren<TextMeshProUGUI>().text = "GAME OVER";
                IsResult = true;
                ResultObj.SetActive(true);
                PauseMenuObj.SetActive(false);
                IsPaused = false;
                break;
            case 1: // quit!
                PhotonNetwork.LeaveRoom();
                break;
            case 2: // restart!
                IsResult = false;
                ResultObj.SetActive(false);

                PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);

                // reload! 
                PhotonNetwork.LoadLevel("Game");
                break;
            case 3:
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

        // iemand anders is weggegaan
        // check of er nog genoeg spelers zijn
        if (PhotonNetwork.CurrentRoom.PlayerCount < PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            // niet genoeg spelers, laat resultaat zien
            IsResult = true;
            ResultObj.SetActive(true); // ziet alleen de overgebleven speler
        }
        // als er wel genoeg spelers over zijn, gaat het spel gewoon door...
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
