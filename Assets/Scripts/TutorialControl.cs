using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TutorialControl : MonoBehaviour
{
  public GameObject firstSeq;
  public GameObject secondSeq;
  public GameObject thirdSeq;
  public GameObject fourthSeq;
  public GameObject tutorialContainer;

  // Start is called before the first frame update
  void Start()
  {
    if (DBManager.score > 1)
    {
      tutorialContainer.SetActive(false);
    }
  }

  // Update is called once per frame
  void Update()
  {

  }

  public void changeState(int value)
  {
    if (value == 1)
    {
      firstSeq.SetActive(false);
    }
    if (value == 2)
    {
      secondSeq.SetActive(false);
    }
    if (value == 3)
    {
      thirdSeq.SetActive(false);
    }
    if (value == 4)
    {
      fourthSeq.SetActive(false);
    }
  }
}
