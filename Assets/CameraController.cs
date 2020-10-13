using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  public Transform cameraTransform;
  public Vector3 zoomAmount;
  public Vector3 newZoom;
  public float movementSpeed;
  public float movementTime;
  public float rotationAmount;
  public Vector3 newPosition;
  public Quaternion newRotation;

  // Start is called before the first frame update
  void Start()
  {
    newPosition = transform.position;
    newRotation = transform.rotation;
    newZoom = cameraTransform.localPosition;
  }

  // Update is called once per frame
  void Update()
  {
    HandleMovementInput();
  }

  void HandleMovementInput()
  {
    if (Input.GetKey(KeyCode.W))
    {
      newPosition += (transform.forward * movementSpeed);
    }
    if (Input.GetKey(KeyCode.S))
    {
      newPosition += (transform.forward * -movementSpeed);
    }
    if (Input.GetKey(KeyCode.D))
    {
      newPosition += (transform.right * movementSpeed);
    }
    if (Input.GetKey(KeyCode.A))
    {
      newPosition += (transform.right * -movementSpeed);
    }
    // if (Input.GetKey(KeyCode.Q))
    // {
    //   newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
    // }
    // if (Input.GetKey(KeyCode.E))
    // {
    //   newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
    // }
    if (Input.GetKey(KeyCode.UpArrow))
    {
      newZoom += zoomAmount;
    }
    if (Input.GetKey(KeyCode.DownArrow))
    {
      newZoom -= zoomAmount;

    }
    cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
    transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
  }
}
