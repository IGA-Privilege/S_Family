using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    public enum CharacterType { Child, Mother, Father }
    public CharacterType thisType;
    public Camera playerCamera;
    public Transform cameraPosition;
    public GameObject characterObj;
    public float speed = 5f;
    private Vector3 movement = Vector3.zero;
    public Vector2 lookDirection = Vector2.zero;
    public UI_InteractBar interactProgressBar;
    public LayerMask momInteractableLM;
    public Animator animator;

    private CharacterController cc;

    private bool isBeingControlled = false;
    private float lookRotationX = 0;
    private float lookRotationY = 0;
    private float lookSpeed = 800f;
    private float lookXLimit = 20f;

    private float interactTimer;
    private float interactStartSec = 3f;
    private float interactEndSec = 8f;
    private bool isInteracting { get { return interactTimer > interactStartSec; } }
    private float interactPercentage { get { return (interactTimer - interactStartSec) / (interactEndSec - interactStartSec); } }


    void Start()
    {
        cc = GetComponent<CharacterController>();
        if (interactProgressBar != null)
        {
            interactProgressBar.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (thisType == CharacterType.Mother)
            {
                animator.SetBool("isMoving", true);
            }
            if (thisType == CharacterType.Child)
            {
                animator.SetBool("isMoving", true);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (thisType == CharacterType.Mother)
            {
                animator.SetTrigger("clean");
            }
            if (thisType == CharacterType.Child)
            {
                animator.SetTrigger("game");
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (thisType == CharacterType.Mother)
            {
                animator.SetTrigger("phone");
            }
            if (thisType == CharacterType.Child)
            {
                animator.SetTrigger("cry");
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (thisType == CharacterType.Child)
            {
                animator.SetTrigger("read");
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isBeingControlled = !isBeingControlled;
        }
        ReadInputMovement();
        PlayerMovement();
        AlignCharacterCameraAndVisual();
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
        if (interactProgressBar != null)
        {
            interactProgressBar.gameObject.SetActive(false);
        }
    }

    private void PlayerMovement()
    {
        if (isBeingControlled)
        {
            Vector3 move = new Vector3(movement.x, 0, movement.z);
            cc.Move(move * Time.deltaTime * speed);
            if (move.magnitude != 0)
            {
                animator.SetBool("isMoving", true);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    private void AlignCharacterCameraAndVisual()
    {
        if (isBeingControlled)
        {
            lookRotationX += -Input.GetAxis("Mouse Y") * lookSpeed * Time.deltaTime;
            lookRotationX = Mathf.Clamp(lookRotationX, -lookXLimit, lookXLimit);
            lookRotationY += Input.GetAxis("Mouse X") * lookSpeed * Time.deltaTime;
            playerCamera.transform.position = cameraPosition.position;
            playerCamera.transform.localRotation = Quaternion.Euler(lookRotationX, lookRotationY, 0);
            transform.localRotation = Quaternion.Euler(0, lookRotationY, 0);
        }
    }

    private void ReadInputMovement()
    {
        movement = Input.GetAxisRaw("Horizontal") * transform.right + Input.GetAxisRaw("Vertical") * transform.forward;
    }
}
