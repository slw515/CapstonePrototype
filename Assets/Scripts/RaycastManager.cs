using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (Physics.Raycast(theRay, out hitInfo))
        {
          objectHit = hitInfo.transform.gameObject;

          var instantiatedObject = Instantiate(placedPrefab, hitInfo.point, objectHit.transform.rotation) as GameObject;
          instantiatedObject.AddComponent<DragObject>();

        }
      }
      else
      {
        if (Physics.Raycast(theRay, out hitInfo))
        {

          objectHit = hitInfo.transform.gameObject;
          if (objectHit.name != "Transparent")
          {
            previewShape.transform.position = hitInfo.point;
          }
        }
      }
    }
    else
    {
      if (Input.GetMouseButton(0))
      {

        if (Physics.Raycast(theRay, out hitInfo))
        {
          objectHit = hitInfo.transform.gameObject;
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
}
