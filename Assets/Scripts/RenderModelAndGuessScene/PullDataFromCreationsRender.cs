using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.SceneManagement;

public class PullDataFromCreationsRender : MonoBehaviour
{
  // Start is called before the first frame update
  public static JSONNode parsedPositionData;
  void Start()
  {
    Debug.Log(DataForRender.objectID);
    if (DataForRender.objectID != 0)
      StartCoroutine(PullObjectsDataFromCreationTable());
    else
      SceneManager.LoadScene("AstronautGame");

  }

  public static IEnumerator PullObjectsDataFromCreationTable()
  {
    WWWForm form = new WWWForm();

    form.AddField("id", DataForRender.objectID);
    WWW www = new WWW("http://stevenwyks.com/PullObjectsDataFromCreationTable.php", form);
    yield return www;
    Debug.Log(www.text);
    parsedPositionData = JSON.Parse(www.text);
    for (int i = 0; i < parsedPositionData.Count; i++)
    {
      Debug.Log("type is: " + parsedPositionData[i]["objectType"]);
      GameObject spawn = GameObject.CreatePrimitive(PrimitiveType.Cube);

      if (parsedPositionData[i]["objectType"] == "Cube")
      {
        spawn = GameObject.CreatePrimitive(PrimitiveType.Cube);
      }
      else if (parsedPositionData[i]["objectType"] == "Sphere")
      {
        spawn = GameObject.CreatePrimitive(PrimitiveType.Sphere);
      }
      else if (parsedPositionData[i]["objectType"] == "Trail")
      {
        spawn = GameObject.CreatePrimitive(PrimitiveType.Cube);
      }
      spawn.transform.position = new Vector3(parsedPositionData[i]["posX"], parsedPositionData[i]["posY"], parsedPositionData[i]["posZ"]);
    }
  }

  // Update is called once per frame
  void Update()
  {

  }

  public void returnToMap()
  {
    SceneManager.LoadScene("AstronautGame");

  }
}
