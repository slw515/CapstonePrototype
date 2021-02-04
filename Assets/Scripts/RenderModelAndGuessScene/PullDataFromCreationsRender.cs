using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;
public class PullDataFromCreationsRender : MonoBehaviour
{
  // Start is called before the first frame update
  [SerializeField]
  private GameObject trailRenderPrefab;
  public string word;
  public static JSONNode parsedPositionData;
  public GameObject containerForPrimitives;

  public InputField inputWordGuess;
  public Sprite wrongLetterSprite;
  public Sprite correctLetterSprite;

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
    parsedPositionData = JSON.Parse(www.text);

    for (int i = 0; i < parsedPositionData.Count; i++)
    {
      if (i == 0)
      {
        word = parsedPositionData[i]["word"];
        foreach (char c in word)
        {
          GameObject EmptyObj = new GameObject(c.ToString());
          EmptyObj.transform.parent = GameObject.Find("GridGuess").transform;
          GameObject appendedImage = Instantiate(new GameObject(), new Vector3(0, -80, 0), Quaternion.identity) as GameObject;
          appendedImage.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
          appendedImage.transform.SetParent(EmptyObj.GetComponent<Transform>());
          appendedImage.AddComponent<Image>();
          appendedImage.GetComponent<Image>().sprite = wrongLetterSprite;
          Text myText = EmptyObj.AddComponent<Text>();
          myText.text = "_";
          myText.font = Resources.Load("OpenSans-Regular") as Font;
          myText.fontSize = 73;
          myText.alignment = TextAnchor.UpperCenter;
          myText.color = new Color(0, 0, 0, 1);
        }
        DataForRender.creatorUsername = parsedPositionData[i]["username"];
      }
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

  public void onChangeWordGuess()
  {
    Debug.Log("clicked check for delete button");
    bool checkerCount = true;
    if (inputWordGuess.text.Length == 0)
      checkerCount = false;

    for (int i = 0; i < inputWordGuess.text.Length; i++)
    {
      if (inputWordGuess.text.Length != GameObject.Find("GridGuess").transform.childCount)
      {
        checkerCount = false;
      }
      if (i < GameObject.Find("GridGuess").transform.childCount)
      {
        char c = inputWordGuess.text[i];
        GameObject.Find("GridGuess").transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        Text gridText = GameObject.Find("GridGuess").transform.GetChild(i).gameObject.GetComponent<Text>();
        // Debug.Log(c + " , " + GameObject.Find("GridGuess").transform.GetChild(i).gameObject.name);
        gridText.text = c.ToString();

        if (c.ToString().ToLower() == GameObject.Find("GridGuess").transform.GetChild(i).gameObject.name.ToLower())
        {
          GameObject.Find("GridGuess").transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = correctLetterSprite;
        }
        else if (c.ToString().ToLower() != GameObject.Find("GridGuess").transform.GetChild(i).gameObject.name.ToLower())
        {
          checkerCount = false;
          GameObject.Find("GridGuess").transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = wrongLetterSprite;
        }

      }
    }
    for (int i = inputWordGuess.text.Length; i < GameObject.Find("GridGuess").transform.childCount; i++)
    {
      Text gridText = GameObject.Find("GridGuess").transform.GetChild(i).gameObject.GetComponent<Text>();

      if (i > inputWordGuess.text.Length)
      {
        GameObject.Find("GridGuess").transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = null;
        gridText.text = "_";
        GameObject.Find("GridGuess").transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
      }
      if (i == 0)
      {
        GameObject.Find("GridGuess").transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = null;
        gridText.text = "_";
        GameObject.Find("GridGuess").transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
      }

    }
    if (checkerCount == true)
    {
      StartCoroutine(SuccessfullyGuessed());
      Debug.Log("input true!");
    }
  }

  IEnumerator SuccessfullyGuessed()
  {
    WWWForm form = new WWWForm();
    Debug.Log(DBManager.username + ", " + DataForRender.creatorUsername + ", " + DataForRender.objectID);
    form.AddField("user", DBManager.username);
    form.AddField("creatorOfObject", DataForRender.creatorUsername);
    form.AddField("objectID", DataForRender.objectID);

    WWW www = new WWW("http://stevenwyks.com/successfulGuessesTable.php", form);
    yield return www;
    Debug.Log(www.text);
  }

  public void returnToMap()
  {
    SceneManager.LoadScene("AstronautGame");

  }
}
