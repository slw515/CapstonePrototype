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
    float latHardCode = 0;
    float longHardCode = 0;
    Debug.Log(DBManager.username + ", " + latHardCode + ", " + longHardCode);
#if UNITY_IPHONE && !UNITY_EDITOR
    form.AddField("latitude", GeoLocation.UserLatitude.ToString());
    form.AddField("longitude", GeoLocation.UserLongitude.ToString());
    form.AddField("username", DBManager.username);
#endif
#if UNITY_EDITOR
    form.AddField("latitude", latHardCode.ToString());
    form.AddField("longitude", longHardCode.ToString());
    form.AddField("username", DBManager.username);
#endif
    WWW www = new WWW("http://stevenwyks.com/pullLongLatFromDB.php", form);
    yield return www;
    Debug.Log(www.text);
    parsedData = JSON.Parse(www.text);
    Debug.Log("data in IEnumerator call: " + parsedData);
  }
}
