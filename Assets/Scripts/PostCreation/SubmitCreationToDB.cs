using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
public class SubmitCreationToDB : MonoBehaviour
{
  [SerializeField]
  private GameObject ContainerForCreation;

  public GameObject loadingPanel;

  int currIndex;
  public Text playerDisplay;
  public Text playerScore;

  public void Awake()
  {
    if (DBManager.username == null)
    {
      UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    playerDisplay.text = "Current Player: " + DBManager.username;
    playerScore.text = DBManager.score.ToString();

  }

  public void Update()
  {
  }

  IEnumerator callMapDataAgain()
  {
    yield return StartCoroutine(PullLongLat.PullLongLatFromCreationTable());
    ReturnToMap();
  }

  public void ReturnToMap()
  {
    SceneManager.LoadScene("AstronautGame");
  }

  public void SubmitContainerToDB()
  {
    StartCoroutine(PullDataFromCreationTable());
  }
  IEnumerator PullDataFromCreationTable()
  {
    WWWForm form = new WWWForm();

    WWW www = new WWW("http://stevenwyks.com/pullDataFromCreations.php", form);

    yield return www;
    currIndex = int.Parse(www.text.Split('\t')[1]);
    yield return currIndex;
    int numObjects = 0;
    // foreach (Transform child in ContainerForCreation.transform)
    // {
    //   StartCoroutine(SubmitIENumerator(child, currIndex));
    //   numObjects++;
    // }
    while (numObjects < ContainerForCreation.transform.childCount)
    {
      StartCoroutine(SubmitIENumerator(ContainerForCreation.transform.GetChild(numObjects), currIndex));
      numObjects++;
      yield return new WaitForSeconds(0.3f);
      loadingPanel.SetActive(true);
    }
    Debug.Log("NUM OBJECTS = " + numObjects);
    loadingPanel.SetActive(false);
    SceneManager.LoadScene("AstronautGame");
  }


  //Saves the data to db
  IEnumerator SubmitIENumerator(Transform childInContainer, int currentIndex)
  {
    // if (string.IsNullOrEmpty(childInContainer.name))
    //   throw new System.ArgumentNullException(nameof(childInContainer.name));
    string[] objectType = new string[1];
    WWWForm form = new WWWForm();
    WWWForm sliceWordsForm = new WWWForm();

    if (childInContainer.GetComponent<MeshFilter>() == null)
    {
      objectType[0] = "Trail";
      Vector3[] positions = new Vector3[25000];
      string xString = "";
      string yString = "";
      string zString = "";

      int counter = childInContainer.GetComponent<TrailRenderer>().GetPositions(positions);
      for (int i = 0; i < counter; i++)
      {
        xString = xString + Math.Round(positions[i].x, 3) + ",";
        yString = yString + Math.Round(positions[i].y, 3) + ",";
        zString = zString + Math.Round(positions[i].z, 3) + ",";
      }
      xString = xString + "1";
      yString = yString + "1";
      zString = zString + "1";
      form.AddField("trailPositionsX", xString);
      form.AddField("trailPositionsY", yString);
      form.AddField("trailPositionsZ", zString);
    }
    else
    {
      objectType[0] = childInContainer.GetComponent<MeshFilter>().mesh.name.Split(' ')[0];
      form.AddField("trailPositionsX", "1");
      form.AddField("trailPositionsY", "1");
      form.AddField("trailPositionsZ", "1");
    }
    Debug.Log("uploading color R: " + childInContainer.GetComponent<Renderer>().material.color.r);
    form.AddField("colorR", childInContainer.GetComponent<Renderer>().material.color.r.ToString());
    form.AddField("colorG", childInContainer.GetComponent<Renderer>().material.color.g.ToString());
    form.AddField("colorB", childInContainer.GetComponent<Renderer>().material.color.b.ToString());
    form.AddField("username", DBManager.username);
    form.AddField("word", DataForPostingObject.objectName);
    sliceWordsForm.AddField("wordToRemove", DataForPostingObject.objectName);
    sliceWordsForm.AddField("username", DBManager.username);

    form.AddField("id", currentIndex);
    form.AddField("posX", childInContainer.position.x.ToString());
    form.AddField("posY", childInContainer.position.y.ToString());
    form.AddField("posZ", childInContainer.position.z.ToString());
    form.AddField("latitude", GeoLocation.UserLatitude.ToString());
    form.AddField("longitude", GeoLocation.UserLongitude.ToString());
    form.AddField("longitude", GeoLocation.UserLongitude.ToString());
    form.AddField("objectType", objectType[0]);

    WWW www = new WWW("http://stevenwyks.com/postCreations.php", form);



    // // WWW www = new WWW("http://localhost/sqlconnect/savedata.php", form);
    yield return www;
    Debug.Log(www.text);

    WWW sliceFormRequest = new WWW("http://stevenwyks.com/sliceWords.php", sliceWordsForm);

    yield return sliceFormRequest;
    if (www.text == "0")
    {
      Debug.Log("Object info posted.");
      // ReturnToMap();
    }
    else
    {
      Debug.Log("Save failed. Error #:" + www.text);
    }
  }
}