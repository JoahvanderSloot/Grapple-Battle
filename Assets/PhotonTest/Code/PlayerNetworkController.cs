using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class PlayerNetworkController : MonoBehaviourPunCallbacks
{
    public float MovementSpeed = 1f;

    private void Start()
    {
        if (photonView.IsMine) // ben ik dit? kleur mezelf blauw
            GetComponent<Renderer>().materials[0].color = Color.blue;
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            // geen UI zichtbaar? we kunnen bewegen
            if (!GameManagerTest.Instance.IsPaused && !GameManagerTest.Instance.IsResult)
                transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * MovementSpeed, 0, Input.GetAxis("Vertical") * Time.deltaTime * MovementSpeed);
        }
    }

    public void OnKilled()
    {
        // doe dingen
        photonView.RPC("SetWinner", RpcTarget.Others, true);
        SetWinner(false);

        // reset positie
        transform.position = Vector3.zero + new Vector3(0, 1, 0); // met offset
    }


    [PunRPC]
    public void SetWinner(bool _isWinner)
    {
        GameManagerTest.Instance.IsResult = true;
        GameManagerTest.Instance.ResultObj.GetComponentInChildren<TextMeshProUGUI>().text = _isWinner ? "YOU WIN!" : "YOU LOSE!";
        GameManagerTest.Instance.ResultObj.SetActive(true);
    }
}
