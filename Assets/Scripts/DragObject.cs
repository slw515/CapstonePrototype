using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
#if UNITY_IPHONE
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
#endif
public class DragObject : MonoBehaviour
{
#if UNITY_IPHONE
  ARSessionOrigin origin;
  ARRaycastManager raycastManager;
  static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
#endif
  private float mZCoord;
  private float rotateSpeed = 35;
  private Vector3 mouseOffset;
  public int editingMode;
  private GameObject Cam;
  public GameObject objectHit;
  float holdTime = 0;

  void Start()
  {
#if UNITY_IPHONE && !UNITY_EDITOR
    origin = GetComponent<ARSessionOrigin>();
    raycastManager = GetComponent<ARRaycastManager>();
    Cam = GameObject.Find("AR Session Origin");
#endif
#if UNITY_EDITOR
    Cam = GameObject.Find("Main Camera");
#endif
  }
  void Update()
  {
#if UNITY_EDITOR
    editingMode = Cam.GetComponent<RaycastManager>().editingMode;

    if (Input.GetMouseButton(0))
    {
      if (holdTime < 0.6)
      {
        Ray theRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hitInfo;

        if (Physics.Raycast(theRay, out hitInfo))
        {
          if (editingMode == 3)
          {
            GameObject localHit = hitInfo.transform.gameObject;
            if (objectHit != null && objectHit.name != "Plane")
            {
              if (localHit.transform.position.x != objectHit.transform.position.x)
              {
                var color = objectHit.GetComponent<Renderer>().material.color;
                color.a = 1.0f;
                objectHit.GetComponent<Renderer>().material.SetColor("_Color", color);
                objectHit = null;
                objectHit = hitInfo.transform.gameObject;
              }
            }
            else
            {
              objectHit = hitInfo.transform.gameObject;
            }
          }
          else if (editingMode == 2)
          {
            objectHit = hitInfo.transform.gameObject;
          }
        }
      }

      if (objectHit.name == "PlacedShape(Clone)")
      {
        if (editingMode == 2)
        {
          float rotX = Input.GetAxis("Mouse X") * rotateSpeed * Mathf.Deg2Rad;
          float rotY = Input.GetAxis("Mouse Y") * rotateSpeed * Mathf.Deg2Rad;
          var col = objectHit.GetComponent<Renderer>().material.color;
          col.a = 0.2f;
          ChangeAlpha(objectHit.GetComponent<Renderer>().material, 0.2f);
          objectHit.GetComponent<Renderer>().material.SetColor("_Color", col);
          objectHit.transform.Rotate(Camera.main.transform.up, -rotX * 200, Space.World);
          objectHit.transform.Rotate(Camera.main.transform.right, rotY * 200, Space.World);
        }
        else if (editingMode == 3)
        {
          var color = objectHit.GetComponent<Renderer>().material.color;
          if (color.a == 1.0f)
          {
            color.a = 0.2f;
            objectHit.GetComponent<Renderer>().material.SetColor("_Color", color);
          }
        }
      }
      holdTime += 0.1f;
    }

    else if (Input.GetMouseButton(0) == false)
    {
      if (editingMode == 2)
      {
        if (objectHit != null && objectHit.name == "PlacedShape(Clone)")
        {
          var color = objectHit.GetComponent<Renderer>().material.color;
          color.a = 1.0f;
          objectHit.GetComponent<Renderer>().material.SetColor("_Color", color);
          objectHit = null;
        }
      }
      holdTime = 0;
    }
    float scrollX = Input.mouseScrollDelta.y;
    if (editingMode == 3 && objectHit.name == "PlacedShape(Clone)")
    {
      float scrollY = (Input.mouseScrollDelta.y / 5) * -1;
      objectHit.transform.localScale += new Vector3(scrollY, scrollY, scrollY);
    }
#endif
#if UNITY_IPHONE && !UNITY_EDITOR
    editingMode = Cam.GetComponent<RaycastManager>().editingMode;

    if (Input.touchCount > 0)
    {
      if (holdTime < 0.6)
      {
        Ray theRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hitInfo;

        if (Physics.Raycast(theRay, out hitInfo))
        {
          if (editingMode == 3)
          {
            GameObject localHit = hitInfo.transform.gameObject;
            if (objectHit != null && objectHit.name != "Plane")
            {
              if (localHit.transform.position.x != objectHit.transform.position.x)
              {
                var color = objectHit.GetComponent<Renderer>().material.color;
                color.a = 1.0f;
                objectHit.GetComponent<Renderer>().material.SetColor("_Color", color);
                objectHit = null;
                objectHit = hitInfo.transform.gameObject;
              }
            }
            else
            {
              objectHit = hitInfo.transform.gameObject;
            }
          }
          else if (editingMode == 2)
          {
            objectHit = hitInfo.transform.gameObject;
          }
        }
      }

      if (objectHit.name == "PlacedShape(Clone)")
      {
        if (editingMode == 2)
        {
          float rotX = Input.touches[0].deltaPosition.x;
          float rotY = Input.touches[0].deltaPosition.y;
          var col = objectHit.GetComponent<Renderer>().material.color;
          col.a = 0.2f;
          ChangeAlpha(objectHit.GetComponent<Renderer>().material, 0.2f);
          objectHit.GetComponent<Renderer>().material.SetColor("_Color", col);
          objectHit.transform.Rotate(Camera.main.transform.up, -rotX * 200, Space.World);
          objectHit.transform.Rotate(Camera.main.transform.right, rotY * 200, Space.World);
        }
        else if (editingMode == 3)
        {
          var color = objectHit.GetComponent<Renderer>().material.color;
          if (color.a == 1.0f)
          {
            color.a = 0.2f;
            objectHit.GetComponent<Renderer>().material.SetColor("_Color", color);
          }
        }
      }
      holdTime += 0.1f;
    }

    else if (Input.touchCount == 0)
    {
      if (editingMode == 2)
      {
        if (objectHit != null && objectHit.name == "PlacedShape(Clone)")
        {
          var color = objectHit.GetComponent<Renderer>().material.color;
          color.a = 1.0f;
          objectHit.GetComponent<Renderer>().material.SetColor("_Color", color);
          objectHit = null;
        }
      }
      holdTime = 0;
    }
    if (editingMode == 3 && objectHit.name == "PlacedShape(Clone)")
    {
      if (Input.touchCount >= 2)
      {
        Vector2 touch0, touch1;

        float distance;
        touch0 = Input.GetTouch(0).position;
        touch1 = Input.GetTouch(1).position;
        distance = Vector2.Distance(touch0, touch1);
        Debug.Log("The pinch distance is: " + distance);
        objectHit.transform.localScale = new Vector3(distance / 650, distance / 650, distance / 420);
      }
    }
#endif
  }

  void ChangeAlpha(Material mat, float alphaVal)
  {
    Color oldColor = mat.color;
    Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alphaVal);
    mat.SetColor("_Color", newColor);
  }

  void OnMouseDown()
  {
    mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
    mouseOffset = gameObject.transform.position - GetMouseWorldPos();
  }

  void OnMouseDrag()
  {
    if (editingMode == 1)
    {
      transform.position = GetMouseWorldPos() + mouseOffset;
    }
  }

  private Vector3 GetMouseWorldPos()
  {
    Vector3 mousePoint = Input.mousePosition;
    mousePoint.z = mZCoord;
    return Camera.main.ScreenToWorldPoint(mousePoint);
  }


  private bool IsPointerOverUIObject()
  {
    PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
    eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    List<RaycastResult> results = new List<RaycastResult>();
    EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
    return results.Count > 0;
  }
}
