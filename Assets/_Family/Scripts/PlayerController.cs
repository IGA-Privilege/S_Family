using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    public enum CharacterType { Child, Mother, Father }
    public CharacterType thisType;
    public Camera playerCamera;
    public float speed = 5f;
    private Vector2 movement = Vector2.zero;
    public Vector2 lookDirection = Vector2.zero;
    public UI_InteractBar interactProgressBar;
    public LayerMask momInteractableLM;

    private CharacterController cc;

    float sensitivityX = 100;
    float sensitivityY = 100;
    float minimumY = -60;
    float maximumY = 60;
    private float rotationX = 0;

    private float interactTimer;
    private float interactStartSec = 3f;
    private float interactEndSec = 8f;
    private bool isInteracting { get { return interactTimer > interactStartSec; } }
    private float interactPercentage { get { return (interactTimer - interactStartSec) / (interactEndSec - interactStartSec); } }


    void Start()
    {
        cc = GetComponent<CharacterController>();
        interactProgressBar.gameObject.SetActive(false);
    }

    private void Update()
    {
        PlayerMovement();
        CameraRotation();
        if (thisType == CharacterType.Mother)
        {
            TickMotherInteraction();
        }
    }

    private void TickMotherInteraction()
    {
        Collider[] objs = Physics.OverlapSphere(transform.position, 2f, momInteractableLM);
        if (objs.Length > 0)
        {
            if (objs[0].TryGetComponent<O_MotherInteractable>(out O_MotherInteractable interactable))
            {
                if (interactable.canInteract)
                {
                    interactTimer += Time.deltaTime;
                    if (isInteracting)
                    {
                        interactProgressBar.gameObject.SetActive(true);
                        interactProgressBar.UpdateInteractProgress(interactPercentage);
                        if (interactTimer > interactEndSec)
                        {
                            interactable.OnPlayerInteract();
                        }
                    }
                    else
                    {
                        interactProgressBar.gameObject.SetActive(false);
                    }
                    return;
                }

            }
        }

        interactTimer = 0f;
        interactProgressBar.gameObject.SetActive(false);

    }

    private void PlayerMovement()
    {
        Vector3 move = new Vector3(movement.x, 0, movement.y);
        cc.Move(transform.TransformDirection(move) * Time.deltaTime * speed);
    }

    private void CameraRotation()
    {
        rotationX -= (lookDirection.y * Time.deltaTime) * sensitivityY;
        rotationX = Mathf.Clamp(rotationX, minimumY, maximumY);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.Rotate(Vector3.up * (lookDirection.x * Time.deltaTime) * sensitivityX);
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context) 
    {
        lookDirection = context.ReadValue<Vector2>();
    }
}
