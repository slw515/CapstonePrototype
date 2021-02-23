using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class RaycastManager : MonoBehaviour
{
  ARSessionOrigin origin;
  static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
  [SerializeField]
  private GameObject placedPrefab;
  // Start is called before the first frame update
  [SerializeField]
  private GameObject previewShape;

  [SerializeField]
  private GameObject placementIndicator;
  private ARRaycastManager arRaycastManager;
  private GameObject reticle;
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
  private Color currentColor;
  public GameObject colorWheelUIElement;
  public GameObject brushPanelUIElement;
  public GameObject selectedOverlay;
  public GameObject Decal;
  private bool snapMode = true;
  public GameObject emptyObject;
  private GameObject[] setActiveOverlay;
  private float penDistance = 0.4f;
  private bool isShortenedDrawing = false;
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
  public GameObject uiForMobile;
  void Start()
  {
    currentColor = new Color(0, 0, 0);
    setActiveOverlay = GameObject.FindGameObjectsWithTag("ShowActiveOverlay");
    foreach (GameObject overlay in setActiveOverlay)
    {
      overlay.SetActive(false);
    }
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
      shape.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
      shape.layer = LayerMask.NameToLayer("Ignore Raycast");
      shape.SetActive(false);
    }

    foreach (GameObject shape in opaqueObjects)
    {
      shape.transform.position = new Vector3(-2, -500, -2);
      shape.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
      shape.name = "PlacedShape";
    }
    placedPrefab.name = "PlacedShape";
    previewObjects[0].SetActive(true);
    previewShape = previewObjects[0];
    placedPrefab = opaqueObjects[0];
    // #if UNITY_IPHONE
    arRaycastManager = GetComponent<ARRaycastManager>();
    placementIndicator = placementIndicator.transform.GetChild(0).gameObject;
    reticle = GameObject.Find("ReticleDraw");
    setActiveOverlay[5].SetActive(true);
    selectedOverlay = setActiveOverlay[5];
    // #endif
  }

  // Update is called once per frame
  void Update()
  {
    if (selectedOverlay.GetComponent<Image>().color != currentColor)
    {
      selectedOverlay.GetComponent<Image>().color = currentColor;
    }
    if (GameObject.Find("ColorPalette")?.GetComponent<ColorPaletteController>())
    {
      currentColor = GameObject.Find("ColorPalette").GetComponent<ColorPaletteController>().SelectedColor;
      // colorPicker.GetComponent<Image>().color = ;
    }
#if UNITY_EDITOR
    if (editingMode == 0 || editingMode == 6 || editingMode == 7)
    {
      Vector3 startPoint = Input.mousePosition;
      reticle.SetActive(false);
      Ray theRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
      RaycastHit hitInfo;
      if (Input.GetMouseButtonDown(0))
      {
        if (!IsPointerOverUIObject())
        {
          if (Physics.Raycast(theRay, out hitInfo))
          {
            if (editingMode == 0)
            {
              Vector3 position = hitInfo.transform.position + Vector3.Scale(hitInfo.normal, new Vector3(0.05f, 0.05f, 0.05f));
              Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
              objectHit = hitInfo.transform.gameObject;
              if (objectHit.name == "Plane")
              {
                Vector3 planeHitPos = new Vector3(round05(hitInfo.point.x), round05(hitInfo.point.y), round05(hitInfo.point.z));
                var instantiatedObject = Instantiate(placedPrefab, planeHitPos, objectHit.transform.rotation) as GameObject;
                instantiatedObject.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                instantiatedObject.transform.parent = emptyObject.transform;
                instantiatedObject.GetComponent<MeshRenderer>().material = placedMaterial;

                instantiatedObject.GetComponent<Renderer>().material.color = currentColor;
                instantiatedObject.AddComponent<DragObject>();
              }
              else if (objectHit.name == "PlacedShape(Clone)")
              {
                if (snapMode)
                {
                  var instantiatedObject = Instantiate(placedPrefab, position, rotation) as GameObject;
                  instantiatedObject.transform.localScale = objectHit.transform.localScale;
                  instantiatedObject.transform.parent = emptyObject.transform;
                  instantiatedObject.GetComponent<MeshRenderer>().material = placedMaterial;
                  instantiatedObject.GetComponent<Renderer>().material.color = currentColor;
                  instantiatedObject.AddComponent<DragObject>();
                }
                else
                {
                  var instantiatedObject = Instantiate(placedPrefab, hitInfo.point, rotation) as GameObject;
                  instantiatedObject.transform.localScale = objectHit.transform.localScale;
                  instantiatedObject.GetComponent<MeshRenderer>().material = placedMaterial;
                  instantiatedObject.GetComponent<Renderer>().material.color = currentColor;
                  instantiatedObject.transform.parent = emptyObject.transform;
                  instantiatedObject.AddComponent<DragObject>();
                }
              }
            }
            else if (editingMode == 6)
            {
              objectHit = hitInfo.transform.gameObject;
              if (objectHit.name == "PlacedShape(Clone)")
              {
                Destroy(objectHit);
              }
            }
            else if (editingMode == 7)
            {
              objectHit = hitInfo.transform.gameObject;
              if (objectHit.name == "PlacedShape(Clone)")
              {
                objectHit.GetComponent<Renderer>().material.color = currentColor;
                // 
                // cubeRenderer.material.SetColor("_Color", currentColor);
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
            Vector3 position = hitInfo.transform.position + Vector3.Scale(hitInfo.normal, new Vector3(0.05f, 0.05f, 0.05f));
            // position = new Vector3(Mathf.Round(position.x), Mathf.Round(position.y), Mathf.Round(position.z));

            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            objectHit = hitInfo.transform.gameObject;
            if (objectHit.name != "Transparent")
            {
              if (snapMode)
              {
                if (objectHit.name == "Plane")
                {
                  Vector3 planeHitPos = new Vector3(round05(hitInfo.point.x), round05(hitInfo.point.y), round05(hitInfo.point.z));

                  previewShape.transform.position = planeHitPos;
                  previewShape.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                  placementIndicator.transform.position = hitInfo.point;
                  placementIndicator.transform.localRotation = rotation;
                }
                else
                {
                  previewShape.transform.position = position;
                  previewShape.transform.localRotation = rotation;
                  previewShape.transform.localScale = objectHit.transform.localScale;
                  placementIndicator.transform.position = position;
                  placementIndicator.transform.localRotation = rotation;

                }
              }
              else
              {
                previewShape.transform.position = hitInfo.point;
                placementIndicator.transform.position = hitInfo.point;
                placementIndicator.transform.localRotation = rotation;
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
        Vector3 startPoint = Input.mousePosition;

        Ray theRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hitInfo;

        if (!IsPointerOverUIObject())
        {
          if (Physics.Raycast(theRay, out hitInfo))
          {
            objectHit = hitInfo.transform.gameObject;
          }
        }
      }
      else
      {
        if (editingMode == 4)
        {
          reticle.GetComponent<Renderer>().material.color = new Color(currentColor.r, currentColor.g, currentColor.b, 0.4f);
          reticle.SetActive(true);
          Vector3 startPoint = Input.mousePosition;
          GameObject penPoint = GameObject.Find("PenPoint");
          Ray theRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)); ;
          RaycastHit hitInfo;
          if (Physics.Raycast(theRay, out hitInfo, 0.4f))
          {
            // reticle.GetComponent<CircleDraw>().isIntersecting = true;
            reticle.transform.position = hitInfo.point;
            penPoint.transform.position = hitInfo.point;
            objectHit = hitInfo.transform.gameObject;

            if (Input.GetMouseButton(0))
            {
              penDistance = hitInfo.point.z;
              isShortenedDrawing = true;
            }
            else
            {
              penDistance = 0.4f;
              isShortenedDrawing = false;
            }

          }
          else
          {
            if (isShortenedDrawing == true)
            {
              if (!Input.GetMouseButton(0))
              {
                isShortenedDrawing = false;
              }
              penPoint.transform.position = Camera.main.transform.position + Camera.main.transform.forward * penDistance;
              reticle.transform.position = Camera.main.transform.position + Camera.main.transform.forward * penDistance;
            }
            else
            {
              penPoint.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.4f;
              reticle.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.4f;
            }
            // reticle.GetComponent<CircleDraw>().isIntersecting = false;
          }
        }
      }
    }
#endif
#if UNITY_IPHONE && !UNITY_EDITOR
    if (editingMode == 0 || editingMode == 6 || editingMode == 7)
    {
      Ray theRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
      RaycastHit hitInfo;
      if (Input.touchCount == 0) {
        if (Physics.Raycast(theRay, out hitInfo))
        {
          Vector3 position = hitInfo.transform.position + Vector3.Scale(hitInfo.normal, new Vector3(0.05f, 0.05f, 0.05f));
          Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);

          objectHit = hitInfo.transform.gameObject;

          if (objectHit.name != "Transparent")
          {
            if (snapMode)
            {
              if (objectHit.name == "PlacedShape(Clone)")
              {
                previewShape.transform.position = position;
                previewShape.transform.localRotation = rotation;
                previewShape.transform.localScale = objectHit.transform.localScale;
              }            
              else if (arRaycastManager.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), s_Hits, TrackableType.Planes))
              {
                var hitPose = s_Hits[0].pose;
                Vector3 planeHitPos = new Vector3(round05(hitPose.position.x), round05(hitPose.position.y), round05(hitPose.position.z));
                placementIndicator.transform.position = planeHitPos;
                previewShape.transform.position = planeHitPos;
                previewShape.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                previewShape.transform.localRotation = rotation;
                placementIndicator.transform.localRotation = Quaternion.identity;
              }
            }
            else
            {
              previewShape.transform.position = hitInfo.point;
            }
          }
        }
      }
      if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
      {
        if (!IsPointerOverUIObject())
        {
          if (Physics.Raycast(theRay, out hitInfo))
          {
            if (editingMode == 0)
            {
              Vector3 position = hitInfo.transform.position + Vector3.Scale(hitInfo.normal, new Vector3(0.05f, 0.05f, 0.05f));
              Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
              objectHit = hitInfo.transform.gameObject;
              if (objectHit.name == "PlacedShape(Clone)")
              {
                if (snapMode)
                {
                  var instantiatedObject = Instantiate(placedPrefab, position, rotation) as GameObject;
                  instantiatedObject.transform.localScale = objectHit.transform.localScale;
                  instantiatedObject.transform.parent = emptyObject.transform;
                  instantiatedObject.GetComponent<MeshRenderer>().material = placedMaterial;
                  instantiatedObject.GetComponent<Renderer>().material.color = currentColor;
                  instantiatedObject.AddComponent<DragObject>();
                }
                else
                {
                  var instantiatedObject = Instantiate(placedPrefab, hitInfo.point, rotation) as GameObject;
                  instantiatedObject.transform.localScale = objectHit.transform.localScale;
                  instantiatedObject.transform.parent = emptyObject.transform;
                  instantiatedObject.GetComponent<MeshRenderer>().material = placedMaterial;
                  instantiatedObject.GetComponent<Renderer>().material.color = currentColor;
                  instantiatedObject.AddComponent<DragObject>();
                }
              }
              else if (arRaycastManager.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), s_Hits, TrackableType.Planes))
              {
                var hitPose = s_Hits[0].pose;
                Vector3 planeHitPos = new Vector3(round05(hitPose.position.x), round05(hitPose.position.y), round05(hitPose.position.z));
                var instantiatedObject = Instantiate(placedPrefab, planeHitPos, Quaternion.identity) as GameObject;
                instantiatedObject.transform.parent = emptyObject.transform;
                instantiatedObject.GetComponent<MeshRenderer>().material = placedMaterial;
                instantiatedObject.GetComponent<Renderer>().material.color = currentColor;
                instantiatedObject.AddComponent<DragObject>();
              }
            }
            else if (editingMode == 6)
            {
              objectHit = hitInfo.transform.gameObject;
              if (objectHit.name == "PlacedShape(Clone)" || objectHit.name == "Stroke(Clone)")
              {
                Destroy(objectHit);
              }
            }
            else if (editingMode == 7)
            {
              objectHit = hitInfo.transform.gameObject;
              if (objectHit.name == "PlacedShape(Clone)")
              {
                objectHit.GetComponent<Renderer>().material.color = currentColor;
              }
            }
          }
        }
      }

    }
    else
    {
      if (editingMode == 4)
        {

          reticle.GetComponent<Renderer>().material.color = new Color(currentColor.r, currentColor.g, currentColor.b, 0.4f);
          GameObject penPoint = GameObject.Find("PenPoint");
          Ray theRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
          RaycastHit hitInfo;
          if (Physics.Raycast(theRay, out hitInfo, 0.4f))
          {
            if (hitInfo.transform.name != "AR Default Plane") {
              // reticle.GetComponent<CircleDraw>().isIntersecting = true;
              reticle.transform.position = hitInfo.point;
              penPoint.transform.position = hitInfo.point;
              objectHit = hitInfo.transform.gameObject;

              if (Input.touchCount > 0)
              {
                penDistance = hitInfo.point.z;
                isShortenedDrawing = true;
              }
              else
              {
                penDistance = 0.4f;
                isShortenedDrawing = false;
              }
            }
          }
          else
          {
            if (isShortenedDrawing == true)
            {
              if (Input.touchCount == 0)
              {
                isShortenedDrawing = false;
              }
              penPoint.transform.position = GameObject.Find("Main Camera").transform.position + Camera.main.transform.forward * penDistance;
              reticle.transform.position = GameObject.Find("Main Camera").transform.position + Camera.main.transform.forward * penDistance;
            }
            else
            {
              penPoint.transform.position = GameObject.Find("Main Camera").transform.position + Camera.main.transform.forward * 0.4f;
              reticle.transform.position = GameObject.Find("Main Camera").transform.position + Camera.main.transform.forward * 0.4f;
            }
            // reticle.GetComponent<CircleDraw>().isIntersecting = false;
          }
        }
    }
#endif


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
    else if (Input.GetKeyDown("c"))
    {
      editingMode = 0;

      previewShape.GetComponent<Renderer>().enabled = true;
      // ForwardArrow.SetActive(false);
      // previewShape.GetComponent<Renderer>().enabled = false;

    }
    else if (Input.GetKeyDown("e"))
    {
      editingMode = 1;

      // previewShape.GetComponent<Renderer>().enabled = true;
      // ForwardArrow.SetActive(false);
      previewShape.GetComponent<Renderer>().enabled = false;

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

  public static float round05(float num)
  {
    return Mathf.Round(num * 20f) / 20.0f;
  }

  public void changeState(int value)
  {
    if (value == 1)
    {
      editingMode = 0;

      placedPrefab = opaqueObjects[0];
      foreach (GameObject shape in previewObjects)
      {
        shape.transform.position = new Vector3(-2, -2, -2);
        shape.SetActive(false);
      }
      previewObjects[0].SetActive(true);
      previewShape = previewObjects[0];

      selectedOverlay.SetActive(false);
      selectedOverlay = null;
      selectedOverlay = setActiveOverlay[5];

      selectedOverlay.SetActive(true);

    }
    else if (value == 2)
    //cube
    {
      editingMode = 0;

      placedPrefab = opaqueObjects[1];
      foreach (GameObject shape in previewObjects)
      {
        shape.transform.position = new Vector3(-2, -2, -2);
        shape.SetActive(false);

      }
      previewObjects[1].SetActive(true);
      previewShape = previewObjects[1];

    }
    else if (value == 3)
    //sphere create. 
    {
      editingMode = 0;
      placedPrefab = opaqueObjects[2];
      foreach (GameObject shape in previewObjects)
      {
        shape.transform.position = new Vector3(-2, -2, -2);
        shape.SetActive(false);

      }
      previewObjects[2].SetActive(true);


      selectedOverlay.SetActive(false);
      selectedOverlay = null;
      // selectedOverlay = setActiveOverlay[4];
      selectedOverlay = setActiveOverlay[3];

      selectedOverlay.SetActive(true);


      previewShape = previewObjects[2];

    }
    else if (value == 4)
    //cube
    {
      editingMode = 0;
      placedPrefab = opaqueObjects[3];
      foreach (GameObject shape in previewObjects)
      {
        shape.transform.position = new Vector3(-2, -2, -2);
        shape.SetActive(false);
      }
      previewObjects[3].SetActive(true);
      previewShape = previewObjects[3];

    }

    else if (value == 5)
    {
      editingMode = 3;

      previewShape.SetActive(false);
    }
    else if (value == 6)
    {
      editingMode = 2;

      previewShape.SetActive(false);
      // ForwardArrow.SetActive(false);
      // previewShape.GetComponent<Renderer>().enabled = false;

    }

    else if (value == 7)
    {
      editingMode = 4;

      previewShape.SetActive(false);

      selectedOverlay.SetActive(false);
      selectedOverlay = null;
      // selectedOverlay = setActiveOverlay[2];
      selectedOverlay = setActiveOverlay[1];

      selectedOverlay.SetActive(true);

    }
    else if (value == 8)
    {
      editingMode = 1;

      previewShape.SetActive(false);

      selectedOverlay.SetActive(false);
      selectedOverlay = null;
      // selectedOverlay = setActiveOverlay[0];
      selectedOverlay = setActiveOverlay[2];

      selectedOverlay.SetActive(true);

    }
    else if (value == 9)
    {
      editingMode = 5;

      previewShape.SetActive(false);
    }
    else if (value == 10)
    {
      editingMode = 6;
      previewShape.SetActive(false);

      selectedOverlay.SetActive(false);
      selectedOverlay = null;
      // selectedOverlay = setActiveOverlay[3];
      selectedOverlay = setActiveOverlay[4];

      selectedOverlay.SetActive(true);
    }
    else if (value == 11)
    {
      editingMode = 7;
      previewShape.SetActive(false);
    }
  }
  //When Touching UI
  private bool IsPointerOverUIObject()
  {

#if UNITY_EDITOR
    PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);

    eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);


    List<RaycastResult> results = new List<RaycastResult>();
    EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
    return results.Count > 0;
#endif
#if UNITY_IPHONE && !UNITY_EDITOR
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);

        eventDataCurrentPosition.position = new Vector2(Input.touches[0].position.x, Input.touches[0].position.y);


        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
#endif

    // var pointer = new PointerEventData(EventSystem.current) { position = new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y) };
    // var raycastResults = new List<RaycastResult>();
    // EventSystem.current.RaycastAll(pointer, raycastResults);
    // if (raycastResults.Count > 0)
    // {
    //   var ui = raycastResults.Find((obj) => obj.gameObject.layer == 5);
    //   if (ui.gameObject != null)
    //   {
    //     Debug.Log("touching UI");
    //     return true;
    //   }
    //   Debug.Log("not touching UI");
    //   return false;

    // }

    // return false;
  }

  private bool IsPointerOverUIObjectMobile()
  {
    // get current pointer position and raycast it
    PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
    eventDataCurrentPosition.position = new Vector2(Input.touches[0].position.x, Input.touches[0].position.y);
    List<RaycastResult> results = new List<RaycastResult>();
    EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

    // check if the target is in the UI
    foreach (RaycastResult r in results)
    {
      bool isUIClick = r.gameObject.transform.IsChildOf(uiForMobile.transform);
      if (isUIClick)
      {
        return true;
      }
    }
    return false;
  }


  public void toggleColorWheel()
  {
    colorWheelUIElement.SetActive(!colorWheelUIElement.activeSelf);
  }

  public void toggleBrushPanel()
  {
    brushPanelUIElement.SetActive(!brushPanelUIElement.activeSelf);
  }

  public void toggleSnap()
  {
    snapMode = !snapMode;
  }

  public void rotateParent()
  {
    emptyObject.transform.Rotate(0, 45, 0);
  }
}
