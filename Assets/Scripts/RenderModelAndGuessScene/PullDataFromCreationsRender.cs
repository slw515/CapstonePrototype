using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class PullDataFromCreationsRender : MonoBehaviour
{
  // Start is called before the first frame update
  public static JSONNode parsedPositionData;
  void Start()
  {
    Debug.Log(DataForRender.objectID);
    if (DataForRender.objectID != 0)
      StartCoroutine(PullObjectsDataFromCreationTable());
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
      GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
      cube.transform.position = new Vector3(parsedPositionData[i]["posX"], parsedPositionData[i]["posY"], parsedPositionData[i]["posZ"]);
    }
  }

  // Update is called once per frame
  void Update()
  {

  }
}
