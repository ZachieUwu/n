using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PLayerMovement : MonoBehaviour
{
    private CharacterController characterController;
    private Animator animator;
    private PlayerActions playerActions;

    public float moveSpeed, walkSpeed, runSpeed;

    public Vector2 moveInput;

    public bool isJumping, isRunning;

    private void OnEnable()
    {
        playerActions = new PlayerActions();
        playerActions.ActionMap.KeyboardAction.performed += OnMove;
        playerActions.ActionMap.KeyboardAction.canceled += OnMove;
        playerActions.ActionMap.Sprint.performed += Sprint_performed;
        playerActions.ActionMap.Sprint.canceled += Sprint_canceled;
        playerActions.Enable();
    }

    private void Sprint_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (obj.performed)
        {
            isRunning = true;
        }
    }

    private void Sprint_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {

        if (obj.performed)
        {
            isRunning = false;
        }

    }

    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        moveInput = obj.ReadValue<Vector2>();
    }

    private void OnDisable()
    {
        playerActions.Disable();
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        Move();
    }

    private void Move()
    {
        moveSpeed = isRunning ? moveSpeed : walkSpeed;
        animator.SetFloat("Speed", moveInput.magnitude <= 0 ? 0f : (isRunning ? 0.5f : 0.25f));

        if (moveInput.magnitude <= 0)
        {
            animator.SetFloat("Speed", 0f);
        }

        Vector3 moveDir = new Vector3(moveInput.x, 0, moveInput.y);
        moveDir = transform.TransformDirection(moveDir) * moveSpeed;
        characterController.Move(moveDir * Time.deltaTime);
    }
}
