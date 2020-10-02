using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAtCamera : MonoBehaviour
{

  public Camera m_Camera;
  private float holdTime = 0.8f; //or whatever
  private float acumTime = 0;
  // Start is called before the first frame update

  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.back,
     m_Camera.transform.rotation * Vector3.down);
    // transform.LookAt(m_Camera.transform.position);
    // Vector3 targetPostition = new Vector3(m_Camera.transform.position.x,
    //                                     transform.position.x,
    //                                     m_Camera.transform.position.z);
    // transform.LookAt(targetPostition);
    // transform.Rotate(-90.0f, 0.0f, 0.0f, Space.World);

    // transform.eulerAngles.x = -90;
    Vector3 startPoint = Input.mousePosition;

    Ray theRay = Camera.main.ScreenPointToRay(startPoint);
    RaycastHit hitInfo;
    if (Input.touchCount == 1)
    {
      acumTime += Input.GetTouch(0).deltaTime;
      Debug.Log("Helloooo");

      if (acumTime >= holdTime)
      {

        //Long tap
        if (Physics.Raycast(theRay, out hitInfo))
        {
          GameObject objectHit = hitInfo.transform.gameObject;
          if (objectHit.name == "ForwardArrow")
          {
            Debug.Log("hit arrow");
          }
        }
      }

      if (Input.GetTouch(0).phase == TouchPhase.Ended)
      {
        acumTime = 0;
      }
    }
  }

}
