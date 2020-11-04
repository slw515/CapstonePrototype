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
    editingMode = GameObject.Find("Main Camera").GetComponent<RaycastManager>().editingMode;
    if (editingMode == 5)
    {
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
    }
  }
}
