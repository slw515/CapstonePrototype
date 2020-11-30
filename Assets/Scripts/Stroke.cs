using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stroke : MonoBehaviour
{
  public GameObject penPoint;
  public GameObject cam;
  // Start is called before the first frame update
  void Start()
  {
    penPoint = GameObject.Find("PenPoint");
  }

  // Update is called once per frame
  void Update()
  {

#if UNITY_IPHONE && !UNITY_EDITOR
    int editingMode = GameObject.Find("AR Session Origin").GetComponent<RaycastManager>().editingMode;
#endif

#if UNITY_EDITOR
    int editingMode = GameObject.Find("Main Camera").GetComponent<RaycastManager>().editingMode;
#endif

    if (Draw.drawing && editingMode == 4)
    {
      this.transform.position = penPoint.transform.position;
      this.transform.rotation = penPoint.transform.rotation;
    }
    else
    {
      this.enabled = false;
    }

  }
}