using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensY;
    public float sensX;

    public Transform orientation;

    float xRotation;
    float yRotation;

    Camera cam;

    bool crouchCam = false;

    private void Start()
    {
        sensX = playerSettings.Instance.sens;
        sensY = playerSettings.Instance.sens;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void FieldOfView(float targetFOV, bool crouch)
    {
        float currentFOV = cam.fieldOfView;
        float smoothSpeed = 10f;

        float newFOV = Mathf.Lerp(currentFOV, 60 + targetFOV * 1.5f, Time.deltaTime * smoothSpeed);

        if (crouch)
        {
            newFOV = Mathf.Lerp(currentFOV, 57, Time.deltaTime * smoothSpeed);
            if (!crouchCam)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - 0.05f, transform.position.z);
                crouchCam = true;
            }
        }
        else
        {
            if(crouchCam)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.05f, transform.position.z);
            }
            crouchCam = false;
        }

        cam.fieldOfView = newFOV;
    }

}
