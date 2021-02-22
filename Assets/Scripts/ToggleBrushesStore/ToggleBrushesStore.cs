using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleBrushesStore : MonoBehaviour
{
  public GameObject brushPanelUIElement;
  public GameObject scoreText;

  public void toggleBrushPanel()
  {
    scoreText.GetComponent<Text>().text = DBManager.score.ToString();
    brushPanelUIElement.SetActive(!brushPanelUIElement.activeSelf);
  }
}
