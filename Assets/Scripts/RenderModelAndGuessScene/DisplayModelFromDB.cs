using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;
public class DisplayModelFromDB : MonoBehaviour
{
  public int modelID;
  public float longitude;
  public float latitude;
  public GameObject tooFar;
  void Awake()
  {
    tooFar = GameObject.Find("Canvas/TooFarImage");
  }
  void OnMouseDown()
  {
    float distance = Vector2.Distance(new Vector2(GeoLocation.UserLatitude, GeoLocation.UserLongitude), new Vector2(latitude, longitude));
    Debug.Log("clicked marker: " + modelID + ", distance is: " + distance);
    if (distance > 0.000167192f)
    {
      Debug.Log("too far");
      StartCoroutine(FadeImage(true, tooFar));
    }
    else
    {
      DataForRender.objectID = modelID;
      SceneManager.LoadScene("RenderAndGuessModel");
    }
  }
  void FadeFinished()
  {
    LeanTween.alpha(tooFar, 0f, 1f).setEase(LeanTweenType.linear);
  }

  IEnumerator FadeImage(bool fadeAway, GameObject img)
  {
    // fade from opaque to transparent
    if (fadeAway)
    {
      // loop over 1 second backwards
      for (float i = 1; i >= 0; i -= Time.deltaTime / 1.3f)
      {
        // set color with i as alpha
        img.GetComponent<Image>().color = new Color(1, 1, 1, i);
        if (img.transform.GetChild(0) != null)
        {
          img.transform.GetChild(0).GetComponent<Text>().color = new Color(0.2f, 0.2f, 0.2f, i);
        }
        yield return null;
      }
    }
    // fade from transparent to opaque
    else
    {
      // loop over 1 second
      for (float i = 0; i <= 1; i += Time.deltaTime)
      {
        // set color with i as alpha
        img.GetComponent<Image>().color = new Color(1, 1, 1, i);
        if (img.transform.GetChild(0) != null)
        {
          img.transform.GetChild(0).GetComponent<Text>().color = new Color(0.2f, 0.2f, 0.2f, i);
        }
        yield return null;
      }
    }
  }
}
