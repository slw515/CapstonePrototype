using UnityEngine;
using System.Collections;

public class CircleDraw : MonoBehaviour
{
  float theta_scale = 0.01f;        //Set lower to add more points
  int size; //Total number of points in circle
  float radius = 3f;
  LineRenderer lineRenderer;
  public float distance;
  public bool isIntersecting = false;
  void Awake()
  {
    // float sizeValue = (2.0f * Mathf.PI) / theta_scale;
    // size = (int)sizeValue;
    // size++;
    // lineRenderer = gameObject.AddComponent<LineRenderer>();
    // lineRenderer.material = new Material(Shader.Find("Transparent/Diffuse"));
    // lineRenderer.SetWidth(0.05f, 0.05f); //thickness of line
    // lineRenderer.SetVertexCount(size);
  }

  void Update()
  {
    // Vector3 pos;
    // float theta = 0f;
    // for (int i = 0; i < size; i++)
    // {
    //   theta += (2.0f * Mathf.PI * theta_scale);
    //   float x = radius * Mathf.Cos(theta);
    //   float y = radius * Mathf.Sin(theta);
    //   x += gameObject.transform.parent.position.x;
    //   y += gameObject.transform.parent.position.y;
    //   pos = new Vector3(x, y, 0);
    //   lineRenderer.SetPosition(i, pos);
    // if (!isIntersecting)
    // {
    //   gameObject.transform.position = Camera.main.transform.position + Camera.main.transform.forward * distance;
    // }
  }
}
