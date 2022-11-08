using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : NetworkBehaviour
{
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    public bool IsGroundedIndicator = true;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
    public bool canMove = true;
    public bool IsMouseLocked;

    public void Start()
    {
        if(!isLocalPlayer)
        {
            GetComponentInChildren<Camera>().gameObject.SetActive(false);
            return;
        }
        MeshRenderer[] renderers;
        renderers = GetComponentsInChildren<MeshRenderer>();
        for(int i = 0; i < renderers.Length; i++)
        {
            if(renderers[i].transform.name == "Visor")
            {
                renderers[i].enabled = false;
            }
        }
        
        playerCamera = GetComponentInChildren<Camera>();
        //playerCamera.transform.SetParent(this.gameObject.transform);
        //playerCamera.transform.localPosition = new Vector3(0, 0.6f, 0); //playerCamera.transform.localPosition = new Vector3(0, 1.9f, 0);
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if(!isLocalPlayer){return;}
        /*if(Input.GetKeyDown(KeyCode.Escape))
        {
            IsMouseLocked = !IsMouseLocked;
        }
        if(IsMouseLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }*/
        IsGroundedIndicator = characterController.isGrounded;
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }
}