using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DisplayModelFromDB : MonoBehaviour
{
  public int modelID;

  void OnMouseDown()
  {
    Debug.Log("clicked marker: " + modelID);
    DataForRender.objectID = modelID;
    SceneManager.LoadScene("RenderAndGuessModel");
  }
}
