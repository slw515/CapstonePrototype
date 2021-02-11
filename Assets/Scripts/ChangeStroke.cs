using UnityEngine;
using System.Collections;

public class ChangeStroke : MonoBehaviour
{
  public GameObject StrokeOfTrail;
  public Material whiteStreakMaterial;
  public Material standardSolidMaterial;
  public GameObject brushPanelUIElement;


  public void changeStrokeWhiteStreak()
  {
    StrokeOfTrail.GetComponent<TrailRenderer>().material = whiteStreakMaterial;
    toggleBrushPanel();
  }
  public void changeStrokeSolidPlain()
  {
    StrokeOfTrail.GetComponent<TrailRenderer>().material = standardSolidMaterial;
    toggleBrushPanel();
  }

  public void toggleBrushPanel()
  {
    brushPanelUIElement.SetActive(!brushPanelUIElement.activeSelf);
  }
}