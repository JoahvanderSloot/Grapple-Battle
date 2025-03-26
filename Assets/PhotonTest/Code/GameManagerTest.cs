using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerTest : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public static GameManagerTest Instance;

    public GameObject LocalPlayer;

    public GameSettings m_GameSettings;
    Coroutine m_gameTimeCoroutine;
    [SerializeField] Canvas m_canvas;

    [SerializeField] List<GameObject> m_players;
    public bool m_GameStart;

    private void Awake()
    {
        Instance = this;
        m_GameStart = false;
        m_canvas.gameObject.SetActive(false);
        m_players = new List<GameObject>();
        StartCoroutine(CheckForPlayers());
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

    public void GiveUp()
    {
        //LocalPlayer.GetComponent<PlayerNetworkController>().OnKilled();
        //// Sluit Pauze scherm...
        //PauseMenuObj.SetActive(false); // toggle
        //IsPaused = PauseMenuObj.activeInHierarchy;

        //// Stuur een signaal naar andere spelers dat het spel afgelopen is...
        //RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // iedereen gaat dit bericht ontvangen

        //// de "0" is gewoon een random code nummer, die vangen we op in de switch-case van "OnEvent" functie (zie onder)
        //PhotonNetwork.RaiseEvent(0, null, raiseEventOptions, SendOptions.SendReliable);
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
        //switch (photonEvent.Code) // we checken welke code dit event heeft (zie PhotonNetwork.RaiseEvent boven)
        //{
        //    case 0: // give up
        //        Instance.ResultObj.GetComponentInChildren<TextMeshProUGUI>().text = "GAME OVER";
        //        IsResult = true;
        //        ResultObj.SetActive(true);
        //        break;
        //    case 1: // quit!
        //        Instance.ResultObj.GetComponentInChildren<TextMeshProUGUI>().text = "GAME OVER";
        //        IsResult = true;
        //        ResultObj.SetActive(true); // ziet alleen de overgebleven speler
        //        break;
        //    case 2: // restart!
        //        IsResult = false;
        //        ResultObj.SetActive(false);

        //        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);

        //        // reload! 
        //        PhotonNetwork.LoadLevel("Game");
        //        break;
        //}
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        // we zijn de kamer uit, dus we gaan weg naar title
        SceneManager.LoadScene("Title");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
    //    base.OnPlayerLeftRoom(otherPlayer);

    //    // iemand anders is weggegaan
    //    // check of er nog genoeg spelers zijn
    //    if (PhotonNetwork.CurrentRoom.PlayerCount < PhotonNetwork.CurrentRoom.MaxPlayers)
    //    {
    //        // niet genoeg spelers, laat resultaat zien
    //        IsResult = true;
    //        ResultObj.SetActive(true); // ziet alleen de overgebleven speler
    //    }
    //    // als er wel genoeg spelers over zijn, gaat het spel gewoon door...
    }
}
