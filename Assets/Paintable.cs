using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paintable : MonoBehaviour
{
  public GameObject emptyObject;
  public GameObject Brush;
  public float brushSize = 0.002f;
  int editingMode;

  void Start()
  {

  }
  void Update()
  {
    editingMode = Camera.main.GetComponent<RaycastManager>().editingMode;
    if (editingMode == 5)
    {
#if UNITY_EDITOR
      if (Input.GetMouseButton(0))
      {
        var Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(Ray, out hit))
        {
          if (hit.transform.gameObject.name != "Plane")
          {
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            var go = Instantiate(Brush, hit.point, rotation);

            go.transform.localScale = Vector3.one * brushSize;
            go.transform.parent = emptyObject.transform;
          }
        }
      }
#endif
#if UNITY_IPHONE
      if (Input.touchCount > 0)
      {
        var Ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        RaycastHit hit;
        if (Physics.Raycast(Ray, out hit))
        {
          if (hit.transform.gameObject.name != "Plane")
          {
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            var go = Instantiate(Brush, hit.point, rotation);

            go.transform.localScale = Vector3.one * brushSize;
            go.transform.parent = emptyObject.transform;
          }
        }
      }
#endif
    }
  }
}
