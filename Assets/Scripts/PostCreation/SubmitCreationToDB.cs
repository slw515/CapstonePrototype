using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class SubmitCreationToDB : MonoBehaviour
{
  [SerializeField]
  private GameObject ContainerForCreation;
  int currIndex;
  public Text playerDisplay;

  public void Awake()
  {
    if (DBManager.username == null)
    {
      UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    playerDisplay.text = "Current Player: " + DBManager.username;
  }

  public void Update()
  {
    // Debug.Log("Current number of children of container: " + ContainerForCreation.transform.childCount);

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

    foreach (Transform child in ContainerForCreation.transform)
    {
      StartCoroutine(SubmitIENumerator(child, currIndex));
    }
  }


  //Saves the data to db
  IEnumerator SubmitIENumerator(Transform childInContainer, int currentIndex)
  {
    // if (string.IsNullOrEmpty(childInContainer.name))
    //   throw new System.ArgumentNullException(nameof(childInContainer.name));
    string[] objectType = new string[1];
    WWWForm form = new WWWForm();

    if (childInContainer.GetComponent<MeshFilter>() == null)
    {
      objectType[0] = "Trail";
      Vector3[] positions = new Vector3[1000];
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
    Debug.Log("uploading type: " + objectType[0]);
    form.AddField("username", DBManager.username);
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

    if (www.text == "0")
    {
      Debug.Log("Object info posted.");
    }
    else
    {
      Debug.Log("Save failed. Error #:" + www.text);
    }
  }

}