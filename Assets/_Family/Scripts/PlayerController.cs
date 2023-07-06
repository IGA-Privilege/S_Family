using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public enum CharacterType { Child, Mother, Father }
    public CharacterType thisType;
    public Camera playerCamera;
    public float speed = 5f;
    private Vector2 movement = Vector2.zero;
    public Vector2 lookDirection = Vector2.zero;

    private CharacterController cc;

    float sensitivityX = 100;
    float sensitivityY = 100;
    float minimumY = -60;
    float maximumY = 60;
    private float rotationX = 0;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        PlayerMovement();
        CameraRotation();
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
