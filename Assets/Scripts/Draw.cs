using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Draw : MonoBehaviour
{
  public bool mouseLookTesting;

  private float pitch = 0;
  private float yaw = 0;
  public GameObject drawObject;
  public GameObject cam;
  int editingMode;
  public GameObject spacePenPoint;
  public GameObject ContainerToAdd;
  public GameObject stroke;
  public GameObject colorPicker;

  public static bool drawing = false;
  private Color currentColor;
  private GameObject currentStroke;


  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    editingMode = cam.GetComponent<RaycastManager>().editingMode;
    // Debug.Log(editingMode);

    // stroke.GetComponent<Renderer>().material.color = new Color(0.4f, 0.5f, 0.9f);
    if (GameObject.Find("ColorPalette")?.GetComponent<ColorPaletteController>())
    {
      currentColor = GameObject.Find("ColorPalette").GetComponent<ColorPaletteController>().SelectedColor;
      // colorPicker.GetComponent<Image>().color = ;
      colorPicker.GetComponent<Image>().color = currentColor;
    }
    if (mouseLookTesting)
    {
      yaw += 2 * Input.GetAxis("Mouse X");
      pitch -= 2 * Input.GetAxis("Mouse Y");

      transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }
#if UNITY_IPHONE && !UNITY_EDITOR
    //     if (Input.touchCount == 0)
    //       return;
    if (Input.touchCount > 0 && editingMode == 4)
    {
      StartStroke();
      // drawObject.SetActive(true);
    }
    else if (Input.touchCount == 0 && editingMode == 4)
    {
      EndStroke();
      // drawObject.SetActive(false);
    }
#endif
#if UNITY_EDITOR
    if (Input.GetMouseButton(0) && editingMode == 4)
    {
      StartStroke();
      // drawObject.SetActive(true);
    }
    else if (!Input.GetMouseButton(0) && editingMode == 4)
    {
      EndStroke();
      // drawObject.SetActive(false);
    }
#endif
  }

  public void StartStroke()
  {
    if (editingMode == 4 && currentStroke == null)
    {
      drawing = true;
      currentStroke = Instantiate(stroke, spacePenPoint.transform.position, spacePenPoint.transform.rotation) as GameObject;
      currentStroke.GetComponent<Renderer>().material.color = currentColor;
      currentStroke.transform.parent = ContainerToAdd.transform;

    }
    else if (editingMode == 4 && currentStroke != null)
    {

      currentStroke.transform.position = spacePenPoint.transform.position;
      currentStroke.transform.rotation = spacePenPoint.transform.rotation;
    }
  }

  public void EndStroke()
  {
    if (editingMode == 4)
    {
      if (currentStroke.GetComponent<MeshFilter>() == null)
      {
        Vector3[] positions = new Vector3[1000];
        int counter = currentStroke.GetComponent<TrailRenderer>().GetPositions(positions);
        // for (int i = 0; i < counter; i++)
        // {
        Debug.Log(positions[1].x);
        // }
      }
      currentStroke = null;
      drawing = false;
    }
  }
}