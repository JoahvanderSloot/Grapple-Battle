using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCam : MonoBehaviourPunCallbacks
{
    public float m_SensY;
    public float m_SensX;

    public Transform m_Orientation;

    float m_xRotation;
    float m_yRotation;

    Camera m_cam;

    bool m_crouchCam = false;

    PlayerMovement m_movement;

    private void Start()
    {
        m_movement = gameObject.GetComponentInParent<PlayerMovement>();

        m_SensX = playerSettings.Instance.sens;
        m_SensY = playerSettings.Instance.sens;

        m_cam = GetComponent<Camera>();
        if (!m_movement.photonView.IsMine)
        {
            m_cam.enabled = false;
        }
    }

    private void Update()
    {
        if (m_movement.m_inFocus && m_movement.photonView.IsMine)
        {
            float _mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * m_SensX;
            float _mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * m_SensY;

            m_yRotation += _mouseX;

            m_xRotation -= _mouseY;
            m_xRotation = Mathf.Clamp(m_xRotation, -90f, 90f);

            transform.rotation = Quaternion.Euler(m_xRotation, m_yRotation, 0);
            m_Orientation.rotation = Quaternion.Euler(0, m_yRotation, 0);
        }
    }

    public void FieldOfView(float _targetFOV, bool _crouch)
    {
        float _currentFOV = m_cam.fieldOfView;
        float _smoothSpeed = 10f;

        float _newFOV = Mathf.Lerp(_currentFOV, 60 + _targetFOV * 1.5f, Time.deltaTime * _smoothSpeed);

        if (_crouch)
        {
            _newFOV = Mathf.Lerp(_currentFOV, 57, Time.deltaTime * _smoothSpeed);
            if (!m_crouchCam)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - 0.05f, transform.position.z);
                m_crouchCam = true;
            }
        }
        else
        {
            if(m_crouchCam)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.05f, transform.position.z);
            }
            m_crouchCam = false;
        }

        m_cam.fieldOfView = _newFOV;
    }
}
