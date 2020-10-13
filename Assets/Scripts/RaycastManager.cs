using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RaycastManager : MonoBehaviour
{
  [SerializeField]
  private GameObject placedPrefab;
  // Start is called before the first frame update
  [SerializeField]
  private GameObject previewShape;

  [SerializeField]
  private Material previewMaterial;

  [SerializeField]
  private Material placedMaterial;
  GameObject objectHit;
  [SerializeField]
  public GameObject[] previewObjects;
  [SerializeField]
  public GameObject[] opaqueObjects;
  [SerializeField]
  GameObject ForwardArrow;

  public GameObject colorWheelUIElement;
  private bool snapMode = true;
  public GameObject emptyObject;
  public GameObject PlacedPrefab
  {
    get
    {
      return placedPrefab;
    }
    set
    {
      placedPrefab = value;
    }
  }

  public int editingMode = 0;

  void Start()
  {
    previewObjects[0] = GameObject.CreatePrimitive(PrimitiveType.Cube);
    previewObjects[1] = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
    previewObjects[2] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    previewObjects[3] = GameObject.CreatePrimitive(PrimitiveType.Plane);

    opaqueObjects[0] = GameObject.CreatePrimitive(PrimitiveType.Cube);
    opaqueObjects[1] = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
    opaqueObjects[2] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    opaqueObjects[3] = GameObject.CreatePrimitive(PrimitiveType.Plane);

    foreach (GameObject shape in previewObjects)
    {
      shape.transform.position = new Vector3(-2, -500, -2);
      shape.name = "Transparent";
      shape.GetComponent<MeshRenderer>().material = previewMaterial;
      shape.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    foreach (GameObject shape in opaqueObjects)
    {
      shape.transform.position = new Vector3(-2, -500, -2);
      shape.name = "PlacedShape";

    }
    placedPrefab.name = "PlacedShape";
    previewShape = previewObjects[2];
  }

  // Update is called once per frame
  void Update()
  {
    Vector3 startPoint = Input.mousePosition;

    Ray theRay = Camera.main.ScreenPointToRay(startPoint);
    RaycastHit hitInfo;
    if (editingMode == 0)
    {
      if (Input.GetMouseButtonDown(0))
      {
        if (!IsPointerOverUIObject())
        {
          if (Physics.Raycast(theRay, out hitInfo))
          {
            Vector3 position = hitInfo.transform.position + hitInfo.normal;
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            objectHit = hitInfo.transform.gameObject;
            if (objectHit.name == "Plane")
            {
              var instantiatedObject = Instantiate(placedPrefab, hitInfo.point, objectHit.transform.rotation) as GameObject;
              instantiatedObject.transform.parent = emptyObject.transform;

              instantiatedObject.AddComponent<DragObject>();

            }
            else if (objectHit.name == "PlacedShape(Clone)")
            {
              if (snapMode)
              {
                var instantiatedObject = Instantiate(placedPrefab, position, rotation) as GameObject;
                instantiatedObject.transform.parent = emptyObject.transform;
                instantiatedObject.AddComponent<DragObject>();
              }
              else
              {
                var instantiatedObject = Instantiate(placedPrefab, hitInfo.point, rotation) as GameObject;
                instantiatedObject.transform.parent = emptyObject.transform;
                instantiatedObject.AddComponent<DragObject>();
              }
            }
          }
        }
      }
      else
      {
        if (Physics.Raycast(theRay, out hitInfo))
        {
          if (!IsPointerOverUIObject())
          {
            Vector3 position = hitInfo.transform.position + hitInfo.normal;
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            Debug.Log(rotation);
            objectHit = hitInfo.transform.gameObject;
            if (objectHit.name != "Transparent")
            {
              if (snapMode)
              {
                if (objectHit.name == "Plane")
                {
                  previewShape.transform.position = hitInfo.point;
                }
                else
                {
                  previewShape.transform.position = hitInfo.transform.position + hitInfo.normal;
                  previewShape.transform.localRotation = rotation;
                }
              }
              else
              {
                previewShape.transform.position = hitInfo.point;

              }
            }
          }
        }
      }
    }
    else
    {
      if (Input.GetMouseButton(0))
      {
        if (!IsPointerOverUIObject())
        {
          if (Physics.Raycast(theRay, out hitInfo))
          {
            objectHit = hitInfo.transform.gameObject;
          }
        }
      }
    }

    if (Input.GetKeyDown("1"))
    {
      placedPrefab = opaqueObjects[0];
      previewShape = previewObjects[0];
      foreach (GameObject shape in previewObjects)
      {
        shape.transform.position = new Vector3(-2, -2, -2);
      }

    }
    else if (Input.GetKeyDown("2"))
    //cube
    {

      placedPrefab = opaqueObjects[1];
      previewShape = previewObjects[1];
      foreach (GameObject shape in previewObjects)
      {
        shape.transform.position = new Vector3(-2, -2, -2);
      }
    }
    else if (Input.GetKeyDown("3"))
    //cube
    {

      placedPrefab = opaqueObjects[2];
      previewShape = previewObjects[2];
      foreach (GameObject shape in previewObjects)
      {
        shape.transform.position = new Vector3(-2, -2, -2);
      }
    }
    else if (Input.GetKeyDown("4"))
    //cube
    {
      placedPrefab = opaqueObjects[3];
      previewShape = previewObjects[3];
      foreach (GameObject shape in previewObjects)
      {
        shape.transform.position = new Vector3(-2, -2, -2);
      }
    }
    else if (Input.GetKeyDown("e"))
    {
      editingMode = 1;

      // previewShape.GetComponent<Renderer>().enabled = true;
      // ForwardArrow.SetActive(false);
      previewShape.GetComponent<Renderer>().enabled = false;

    }
    else if (Input.GetKeyDown("c"))
    {
      editingMode = 0;

      previewShape.GetComponent<Renderer>().enabled = true;
      // ForwardArrow.SetActive(false);
      // previewShape.GetComponent<Renderer>().enabled = false;

    }
    else if (Input.GetKeyDown("r"))
    {
      editingMode = 2;

      previewShape.GetComponent<Renderer>().enabled = false;
      // ForwardArrow.SetActive(false);
      // previewShape.GetComponent<Renderer>().enabled = false;

    }
    else if (Input.GetKeyDown("q"))
    {
      editingMode = 3;

      previewShape.GetComponent<Renderer>().enabled = false;
    }
    else if (Input.GetKeyDown("t"))
    {
      editingMode = 4;

      previewShape.GetComponent<Renderer>().enabled = false;

    }
  }

  public void changeState(int value)
  {
    if (value == 1)
    {
      editingMode = 0;

      placedPrefab = opaqueObjects[0];
      previewShape = previewObjects[0];
      foreach (GameObject shape in previewObjects)
      {
        shape.transform.position = new Vector3(-2, -2, -2);
      }
    }
    else if (value == 2)
    //cube
    {
      editingMode = 0;

      placedPrefab = opaqueObjects[1];
      previewShape = previewObjects[1];
      foreach (GameObject shape in previewObjects)
      {
        shape.transform.position = new Vector3(-2, -2, -2);
      }
    }
    else if (value == 3)
    //cube
    {
      editingMode = 0;
      placedPrefab = opaqueObjects[2];
      previewShape = previewObjects[2];
      foreach (GameObject shape in previewObjects)
      {
        shape.transform.position = new Vector3(-2, -2, -2);
      }
    }
    else if (value == 4)
    //cube
    {
      editingMode = 0;
      placedPrefab = opaqueObjects[3];
      previewShape = previewObjects[3];
      foreach (GameObject shape in previewObjects)
      {
        shape.transform.position = new Vector3(-2, -2, -2);
      }
    }
    // else if (Input.GetKeyDown("e"))
    // {
    //   editingMode = 1;

    //   // previewShape.GetComponent<Renderer>().enabled = true;
    //   // ForwardArrow.SetActive(false);
    //   previewShape.GetComponent<Renderer>().enabled = false;

    // }
    // else if (Input.GetKeyDown("c"))
    // {
    //   editingMode = 0;

    //   previewShape.GetComponent<Renderer>().enabled = true;
    //   // ForwardArrow.SetActive(false);
    //   // previewShape.GetComponent<Renderer>().enabled = false;

    // }
    else if (value == 5)
    {
      editingMode = 3;

      previewShape.GetComponent<Renderer>().enabled = false;
    }
    else if (value == 6)
    {
      editingMode = 2;

      previewShape.GetComponent<Renderer>().enabled = false;
      // ForwardArrow.SetActive(false);
      // previewShape.GetComponent<Renderer>().enabled = false;

    }

    else if (value == 7)
    {
      editingMode = 4;

      previewShape.GetComponent<Renderer>().enabled = false;

    }
    else if (value == 8)
    {
      editingMode = 1;

      previewShape.GetComponent<Renderer>().enabled = false;

    }
  }
  //When Touching UI
  private bool IsPointerOverUIObject()
  {
    PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
    eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    List<RaycastResult> results = new List<RaycastResult>();
    EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
    return results.Count > 0;
  }

  public void toggleColorWheel()
  {
    colorWheelUIElement.SetActive(!colorWheelUIElement.activeSelf);
  }

  public void toggleSnap()
  {
    snapMode = !snapMode;
  }

  public void rotateParent()
  {
    emptyObject.transform.Rotate(0, 90, 0);
  }
}
