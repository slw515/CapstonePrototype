using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragObject : MonoBehaviour
{
  private float mZCoord;
  private float rotateSpeed = 35;
  private Vector3 mouseOffset;
  public int editingMode;
  public GameObject objectHit;
  float holdTime = 0;
  void Update()
  {
    editingMode = GameObject.Find("Main Camera").GetComponent<RaycastManager>().editingMode;
    if (Input.GetMouseButton(0))
    {

      if (holdTime < 0.6)
      {
        Vector3 startPoint = Input.mousePosition;

        Ray theRay = Camera.main.ScreenPointToRay(startPoint);
        RaycastHit hitInfo;

        if (Physics.Raycast(theRay, out hitInfo))
        {
          if (editingMode == 3 && objectHit.name != "PlacedShape(Clone)")
          {
            var color = objectHit.GetComponent<Renderer>().material.color;
            if (color.a == 1.0f)
            {
              color.a = 0.2f;
              objectHit.GetComponent<Renderer>().material.SetColor("_Color", color);
            }
          }
          objectHit = hitInfo.transform.gameObject;
        }
      }

      if (objectHit.name == "PlacedShape(Clone)")
      {
        if (editingMode == 3)
        {
          var color = objectHit.GetComponent<Renderer>().material.color;
          if (color.a == 1.0f)
          {
            color.a = 0.2f;
            objectHit.GetComponent<Renderer>().material.SetColor("_Color", color);
          }
        }
        if (editingMode == 2)
        {
          // transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * Time.deltaTime * rotateSpeed);
          // Debug.Log(new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * Time.deltaTime * rotateSpeed);
          // objectHit.GetComponent<Renderer>().material.color.a = 1.0f;
          float rotX = Input.GetAxis("Mouse X") * rotateSpeed * Mathf.Deg2Rad;
          float rotY = Input.GetAxis("Mouse Y") * rotateSpeed * Mathf.Deg2Rad;
          var col = objectHit.GetComponent<Renderer>().material.color;
          col.a = 0.2f;
          Debug.Log(col);
          ChangeAlpha(objectHit.GetComponent<Renderer>().material, 0.2f);
          objectHit.GetComponent<Renderer>().material.SetColor("_Color", col);
          objectHit.transform.Rotate(Camera.main.transform.up, -rotX * 200, Space.World);
          objectHit.transform.Rotate(Camera.main.transform.right, rotY * 200, Space.World);

          // transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * Time.deltaTime * rotSpeed);
        }
      }
      holdTime += 0.1f;
    }
    if (editingMode == 3 && objectHit.name == "PlacedShape(Clone)")
    {
      float scrollY = (Input.mouseScrollDelta.y / 5) * -1;

      objectHit.transform.localScale += new Vector3(scrollY, scrollY, scrollY);
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
