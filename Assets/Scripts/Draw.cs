using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw : MonoBehaviour
{
  public bool mouseLookTesting;

  private float pitch = 0;
  private float yaw = 0;
  public GameObject drawObject;
  int editingMode;
  public GameObject spacePenPoint;
  public GameObject stroke;
  public static bool drawing = false;

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

    editingMode = GameObject.Find("Main Camera").GetComponent<RaycastManager>().editingMode;

    if (mouseLookTesting)
    {
      yaw += 2 * Input.GetAxis("Mouse X");
      pitch -= 2 * Input.GetAxis("Mouse Y");

      transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }

    if (Input.GetMouseButton(0) && editingMode == 4)
    {
      StartStroke();
      // drawObject.SetActive(true);
    }
    else if (Input.GetMouseButtonUp(0) == true && editingMode == 4)
    {
      EndStroke();
      // drawObject.SetActive(false);
    }
    else
    {
      // drawObject.SetActive(false);
    }
  }

  public void StartStroke()
  {
    GameObject currentStroke;
    drawing = true;
    currentStroke = Instantiate(stroke, spacePenPoint.transform.position, spacePenPoint.transform.rotation) as GameObject;
  }

  public void EndStroke()
  {
    drawing = false;
  }
}