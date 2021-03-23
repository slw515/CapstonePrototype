using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class PullLongLat : MonoBehaviour
{
  ArrayList arlist = new ArrayList();
  static float latHardCode = 0;
  static float longHardCode = 0;
  public static JSONNode parsedData;
  public void Awake()
  {
    if (DBManager.username == null)
    {
      UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
#if UNITY_EDITOR
    StartCoroutine(changeLongLatTest());
#endif
  }

  public static IEnumerator changeLongLatTest()
  {
    yield return new WaitForSeconds(4f);
    longHardCode = 100;
    latHardCode = 100;
    Debug.Log(latHardCode + " changed from latHardCode");
  }

  public static IEnumerator PullLongLatFromCreationTable()
  {
    WWWForm form = new WWWForm();

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
    yield return parsedData;
    Debug.Log("Amount of data in IEnumerator call: " + parsedData.Count);
  }
}
