using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.SceneManagement;
using System.Linq;
public class PullDataFromCreationsRender : MonoBehaviour
{
  // Start is called before the first frame update
  [SerializeField]
  private GameObject trailRenderPrefab;
  public static JSONNode parsedPositionData;
  public GameObject containerForPrimitives;
  void Start()
  {
    Debug.Log(DataForRender.objectID);
    if (DataForRender.objectID != 0)
      StartCoroutine(PullObjectsDataFromCreationTable());
    else
      SceneManager.LoadScene("AstronautGame");

  }

  public IEnumerator PullObjectsDataFromCreationTable()
  {
    WWWForm form = new WWWForm();

    form.AddField("id", DataForRender.objectID);
    WWW www = new WWW("http://stevenwyks.com/PullObjectsDataFromCreationTable.php", form);
    yield return www;
    Debug.Log(www.text);
    parsedPositionData = JSON.Parse(www.text);
    Debug.Log(parsedPositionData.Count + " length of all objects to render.");

    for (int i = 0; i < parsedPositionData.Count; i++)
    {
      GameObject spawn = new GameObject();
      if (parsedPositionData[i]["objectType"] == "Cube")
      {
        spawn = GameObject.CreatePrimitive(PrimitiveType.Cube);
        spawn.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
      }
      else if (parsedPositionData[i]["objectType"] == "Sphere")
      {
        spawn = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        spawn.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
      }
      else if (parsedPositionData[i]["objectType"] == "Trail")
      {
        Debug.Log(parsedPositionData[i]["trailPositionsX"]);
        spawn = Instantiate(trailRenderPrefab) as GameObject;
        spawn.GetComponent<TrailRenderer>().AddPositions(TextToArray(parsedPositionData[i]["trailPositionsX"], parsedPositionData[i]["trailPositionsY"], parsedPositionData[i]["trailPositionsZ"]).ToArray());
      }
      spawn.transform.parent = containerForPrimitives.transform;
      spawn.GetComponent<Renderer>().material.color = new Color(parsedPositionData[i]["colorR"], parsedPositionData[i]["colorG"], parsedPositionData[i]["colorB"]);
      spawn.transform.position = new Vector3(parsedPositionData[i]["posX"], parsedPositionData[i]["posY"], parsedPositionData[i]["posZ"]);
    }
  }

  public static List<Vector3> TextToArray(string TextX, string TextY, string TextZ)
  {
    List<Vector3> positions = new List<Vector3>();
    float[] XPositions = TextX.Split(',').Select(float.Parse).ToArray();
    float[] YPositions = TextY.Split(',').Select(float.Parse).ToArray();
    float[] ZPositions = TextZ.Split(',').Select(float.Parse).ToArray();

    for (int i = 0; i < XPositions.Length - 1; i++)
    {
      positions.Add(new Vector3(XPositions[i], YPositions[i], ZPositions[i]));
    }
    return positions;
  }

  // public Vector3 returnAveragePosition()
  // {

  // }

  // Update is called once per frame
  void Update()
  {

  }

  public void returnToMap()
  {
    SceneManager.LoadScene("AstronautGame");

  }
}
