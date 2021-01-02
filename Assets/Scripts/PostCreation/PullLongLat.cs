using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class PullLongLat : MonoBehaviour
{
  ArrayList arlist = new ArrayList();
  public static JSONNode parsedData;
  public void Awake()
  {
    if (DBManager.username == null)
    {
      UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
  }

  public static IEnumerator PullLongLatFromCreationTable()
  {
    WWWForm form = new WWWForm();
    float latHardCode = 14.3f;
    float longHardCode = 121f;

    Debug.Log("current latitude is: " + GeoLocation.UserLatitude);

    // form.AddField("latitude", GeoLocation.UserLatitude.ToString());
    // form.AddField("longitude", GeoLocation.UserLongitude.ToString());

    form.AddField("latitude", latHardCode.ToString());
    form.AddField("longitude", longHardCode.ToString());
    WWW www = new WWW("http://stevenwyks.com/pullLongLatFromDB.php", form);
    yield return www;
    Debug.Log(www.text);
    parsedData = JSON.Parse(www.text);
    Debug.Log("data in IEnumerator call: " + parsedData);
  }
}
