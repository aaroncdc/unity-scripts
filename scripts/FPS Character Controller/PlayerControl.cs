using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public bool lockCursor = true;

    public float mouseSensitivity = 1.0f;
    public float controllerCameraSensitivity = 1.6f;

    public float headOffset = .7f;
    public float lowerHeadAngle = 60.0f;
    public float upperHeadAngle = 290.0f;
    public float minClampAngle = -90.0f;
    public float maxClampAngle = 90.0f;

    public float feetHeight = -1.5f;
    public float runningSpeed = .75f;
    public float walkingSpeed = .35f;
    public float acceleration = .05f;
    public float maxAcceleration = 1.0f;

    public float jumpValue = 10.0f;
    public float jumpTimerSubstract = 0.1f;
    public float gravity = -9.8f;

    public float crouchHeightModifier = 0.5f;

    public GameObject playerHead;

    Transform head;
    CharacterController controller;
    BoxCollider feetCollider;

    bool grounded = false;
    bool walk = false;
    public bool crouching = false;
    float mX = .0f;
    float mY = .0f;
    float accel = .0f;
    float ymovement = .0f;
    float originalHeight = .0f;
    float originalFeetHeight = .0f;

    Vector3 movement;

    void OnTriggerEnter(Collider col)
    {
        grounded = true;
    }

    void OnTriggerExit(Collider col)
    {
        grounded = false;
    }

    public bool isGrounded()
    {
        return grounded;
    }

    public bool toggleCursor()
    {
        return lockCursor = !lockCursor;
    }

    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    // Start is called before the first frame update
    void Start()
    {
        if(playerHead == null)
        {
            playerHead = transform.Find("PlayerHead").gameObject;
        }

        head = playerHead.transform;
        
        controller = gameObject.GetComponent<CharacterController>();
        originalHeight = controller.height;
        feetCollider = transform.GetComponent<BoxCollider>();
        originalFeetHeight = feetCollider.transform.position.y;
        Physics.IgnoreCollision(feetCollider, controller);
    }

    // Update is called once per frame
    void Update()
    {
        /* Mouse look */

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        mX = Input.GetAxis("Mouse X") * mouseSensitivity;
        mY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        if (Input.GetAxis("RightStickX") > 0.4f || Input.GetAxis("RightStickX") < -0.4f)
        {
            mX = Input.GetAxis("RightStickX") * controllerCameraSensitivity;
        }

        if (Input.GetAxis("RightStickY") > 0.4f || Input.GetAxis("RightStickY") < -0.4f)
        {
            mY = -Input.GetAxis("RightStickY") * controllerCameraSensitivity;
        }

        transform.Rotate(transform.up, mX);

        head.Rotate(Vector3.right, -ClampAngle(mY, minClampAngle, maxClampAngle));

        if (head.localEulerAngles.x > lowerHeadAngle && head.localEulerAngles.x < 180.0f)
        {
            head.localEulerAngles = new Vector3(lowerHeadAngle, head.localEulerAngles.y, .0f);
        }

        if (head.localEulerAngles.x > 180.0f && head.localEulerAngles.x < upperHeadAngle)
        {
            head.localEulerAngles = new Vector3(upperHeadAngle, head.localEulerAngles.y, .0f);
        }

        if(head.localEulerAngles.z != 0.0f)
        {
            head.localEulerAngles = new Vector3(head.localEulerAngles.x, head.localEulerAngles.y, .0f);
        }

        /* Player Movement */

        walk = Input.GetButton("Walk");
        crouching = Input.GetButton("Crouch");

        if(Input.GetButtonDown("ToggleWalk"))
        {
            walk = !walk;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if(isGrounded())
            {
                ymovement += jumpValue;
            }
        }


        if(crouching)
        {
            controller.height = originalHeight - crouchHeightModifier;

            float headY = head.localPosition.y;

            if (headY > controller.height / 4)
            {
                 headY -= 0.02f;
            }
                
            if(headY <= controller.height / 4)
            {
                headY = controller.height / 4;
            }

            head.localPosition = new Vector3(head.localPosition.x, headY, head.localPosition.z);
            feetCollider.center = new Vector3(feetCollider.center.x, feetHeight / 1.5f, feetCollider.center.z);
        }
        else
        {
            controller.height = originalHeight;

            float headY = head.localPosition.y;

            if (headY <= headOffset)
            {
                headY += 0.02f;
            }

            if (headY > headOffset)
            {
                headY = headOffset;
            }

            head.localPosition = new Vector3(head.localPosition.x, headY, head.localPosition.z);
            feetCollider.center = new Vector3(feetCollider.center.x, feetHeight, feetCollider.center.z);
        }

        if (Input.GetAxis("Horizontal") == .0f && Input.GetAxis("Vertical") == .0f)
        {
            if (accel > .0f)
            {
                accel -= acceleration;
            }
            else if (accel < .0f)
            {
                accel += acceleration;
            }
        }
        else
        {
            if(accel < maxAcceleration)
            {
                accel += acceleration;
            }
            
        }

        ymovement += gravity;

        if(isGrounded() && ymovement < .0f)
        {
            ymovement = 0.0f;
        }

        movement = (((transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical"))) *
            (walk ? walkingSpeed : runningSpeed) * accel) + (transform.up * ymovement);

        //print(movement + " " + isGrounded() + " " + ymovement);

        //controller.SimpleMove(movement);
        controller.Move(movement);
    }
}
