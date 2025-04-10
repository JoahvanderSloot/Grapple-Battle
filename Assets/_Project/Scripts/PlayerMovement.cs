using Photon.Pun;
using UnityEngine;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    [Header("Movement")]
    public float m_BaseSpeed;
    public float m_GroundDrag;
    public float m_JumpForce;
    public float m_JumpCooldown;
    public float m_AirMultiplier;
    public float m_WallJumpForce;
    float m_moveSpeed;
    bool m_readyToJump;

    [HideInInspector] public float m_WalkSpeed;
    [HideInInspector] public float m_SprintSpeed;

    [Header("Keybinds")]
    public KeyCode m_JumpKey;
    public KeyCode m_CrouchKey;

    [Header("Ground Check")]
    public LayerMask m_WhatIsGround;
    bool m_grounded;

    public Transform m_Orientation;

    float m_horizontalInput;
    float m_verticalInput;

    Vector3 m_moveDirection;

    Rigidbody m_rb;
    PhotonRigidbodyView m_photonRigidbodyView;

    [SerializeField] CapsuleCollider m_playerCollider;

    [Header("Camera")]
    [SerializeField] PlayerCam m_playerCam;
    bool m_isCrouching;

    [Header("Other")]
    HitPoints m_hitPoints;
    public bool m_inFocus;
    Animator m_anim;

    private void Start()
    {
        m_anim = GetComponent<Animator>();
        m_inFocus = true;
        m_hitPoints = GetComponent<HitPoints>();
        m_JumpKey = playerSettings.Instance.jump;
        m_CrouchKey = playerSettings.Instance.crouch;

        m_rb = GetComponent<Rigidbody>();
        m_rb.freezeRotation = true;
        m_readyToJump = true;

        // Photon Rigidbody synchronisatie
        m_photonRigidbodyView = GetComponent<PhotonRigidbodyView>();
        if (m_photonRigidbodyView != null)
        {
            m_photonRigidbodyView.m_TeleportEnabled = true;
            m_photonRigidbodyView.m_SynchronizeVelocity = true;
            m_photonRigidbodyView.m_SynchronizeAngularVelocity = true;
        }
    }

    private void Update()
    {
        // Kill player in void
        if (transform.position.y <= -20)
        {
            float _decrementRate = 200;
            if (m_hitPoints.m_HP > 0)
            {
                m_hitPoints.m_HP -= Mathf.FloorToInt(_decrementRate * Time.deltaTime);
            }
            else
            {
                m_hitPoints.m_HP = 0;
            }

        }

        if (!m_inFocus || !photonView.IsMine || GameManager.Instance.IsPaused || GameManager.Instance.IsResult || !GameManager.Instance.IsRunning) return;

        float _playerHeight = m_playerCollider.height * m_playerCollider.transform.localScale.y;
        m_grounded = Physics.Raycast(m_playerCollider.transform.position, Vector3.down, _playerHeight * 0.5f + 0.1f, m_WhatIsGround);

        MyInput();
        SpeedControl();

        m_rb.drag = m_grounded ? m_GroundDrag : 0;

        // Verhoog massa bij crouchen in de lucht
        m_rb.mass = (!m_grounded && Input.GetKey(m_CrouchKey)) ? 5 : 2;

        m_playerCam.FieldOfView(m_rb.velocity.magnitude, m_isCrouching);
        m_anim.SetBool("Crouch", m_isCrouching);
    }

    private void OnApplicationFocus(bool focus)
    {
        m_inFocus = focus;
    }

    private void FixedUpdate()
    {
        if (!m_inFocus || !photonView.IsMine || GameManager.Instance.IsPaused || GameManager.Instance.IsResult || !GameManager.Instance.IsRunning) return;

        MovePlayer();
        WallJump();

        m_anim.SetBool("Grounded", m_grounded);

        if (m_grounded)
        {
            m_anim.SetFloat("Velo", m_rb.velocity.magnitude);

            if (m_rb.velocity.magnitude > 0.5f && !AudioManager.m_Instance.IsPlaying("Footsteps"))
            {
                AudioManager.m_Instance.Play("Footsteps");
            }
            else if (m_rb.velocity.magnitude <= 0.5f && AudioManager.m_Instance.IsPlaying("Footsteps"))
            {
                AudioManager.m_Instance.Stop("Footsteps");
            }
        }
        else
        {
            if (AudioManager.m_Instance.IsPlaying("Footsteps"))
            {
                AudioManager.m_Instance.Stop("Footsteps");
            }

            Vector3 _vel = m_rb.velocity;
            if (GetComponent<PlayerAttacks>().m_IsGrappling)
            {
                m_moveSpeed = m_BaseSpeed + 5;
            }
            else
            {
                m_moveSpeed = m_BaseSpeed;
            }
            _vel.y -= 15f * Time.fixedDeltaTime;
            m_rb.velocity = _vel;

            m_anim.SetFloat("Velo", 0);
        }
    }

    private void MyInput()
    {
        m_horizontalInput = Input.GetAxisRaw("Horizontal");
        m_verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(m_JumpKey) && m_readyToJump && m_grounded)
        {
            m_readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), m_JumpCooldown);
        }

        if (Input.GetKeyDown(m_CrouchKey))
        {
            Crouch();
        }
        else if (Input.GetKeyUp(m_CrouchKey))
        {
            m_moveSpeed = 7;
            m_GroundDrag = 5;
            m_readyToJump = true;
            m_isCrouching = false;
        }
    }

    private void MovePlayer()
    {
        if (m_rb == null) return;

        m_moveDirection = m_Orientation.forward * m_verticalInput + m_Orientation.right * m_horizontalInput;

        if (m_grounded)
            m_rb.AddForce(m_moveDirection.normalized * m_moveSpeed * 10f, ForceMode.Force);
        else
            m_rb.AddForce(m_moveDirection.normalized * m_moveSpeed * 10f * m_AirMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 _flatVel = new Vector3(m_rb.velocity.x, 0f, m_rb.velocity.z);

        if (_flatVel.magnitude > m_moveSpeed)
        {
            Vector3 limitedVel = _flatVel.normalized * m_moveSpeed;
            m_rb.velocity = new Vector3(limitedVel.x, m_rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        AudioManager.m_Instance.Play("Jump");
        m_rb.velocity = new Vector3(m_rb.velocity.x, 0f, m_rb.velocity.z);
        m_rb.AddForce(transform.up * m_JumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        m_readyToJump = true;
    }

    private void Crouch()
    {
        AudioManager.m_Instance.Play("Crouch");
        m_moveSpeed = 3;
        m_GroundDrag = 8;
        m_readyToJump = false;
        m_isCrouching = true;
    }

    private void WallJump()
    {
        if (m_grounded || !Input.GetKey(m_JumpKey) || !m_readyToJump) return;

        float _playerHeight = m_playerCollider.height * m_playerCollider.transform.localScale.y;
        float _detectionRadius = _playerHeight * 0.5f - 0.3f;
        Vector3 _wallJumpSpherePos = new Vector3(m_playerCollider.transform.position.x, m_playerCollider.transform.position.y - 0.5f, m_playerCollider.transform.position.z);

        Collider[] _hitColliders = Physics.OverlapSphere(_wallJumpSpherePos, _detectionRadius, m_WhatIsGround);
        bool _playerCanWalljump = false;
        Vector3 _wallPosition = Vector3.zero;

        foreach (Collider _hitCollider in _hitColliders)
        {
            _wallPosition = _hitCollider.ClosestPoint(transform.position);
            _playerCanWalljump = true;
            break;
        }

        if (_playerCanWalljump)
        {
            m_readyToJump = false;
            Vector3 _wallDirection = (transform.position - _wallPosition).normalized;
            Vector3 _jumpDirection = Vector3.up + _wallDirection;

            AudioManager.m_Instance.Play("Jump");

            m_rb.velocity = new Vector3(m_rb.velocity.x, 0f, m_rb.velocity.z);
            m_rb.AddForce(_jumpDirection * m_WallJumpForce, ForceMode.Impulse);

            Invoke(nameof(ResetJump), m_JumpCooldown);
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 _endPosition = m_playerCollider.transform.position;
        float _playerHeight = m_playerCollider.height * m_playerCollider.transform.localScale.y;
        _endPosition.y -= _playerHeight * 0.5f + 0.3f;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(m_playerCollider.transform.position, _endPosition);

        float _detectionRadius = _playerHeight * 0.5f - 0.3f;
        Vector3 _wallJumpSpherePos = new Vector3(m_playerCollider.transform.position.x, m_playerCollider.transform.position.y - 0.5f, m_playerCollider.transform.position.z);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(_wallJumpSpherePos, _detectionRadius);
    }
}