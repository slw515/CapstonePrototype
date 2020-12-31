using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    WWWForm form = new WWWForm();
    form.AddField("username", DBManager.username);
    form.AddField("id", currentIndex);
    form.AddField("posX", childInContainer.position.x.ToString());
    form.AddField("posY", childInContainer.position.y.ToString());
    form.AddField("posZ", childInContainer.position.z.ToString());
    form.AddField("latitude", GeoLocation.UserLatitude.ToString());
    form.AddField("longitude", GeoLocation.UserLongitude.ToString());

    WWW www = new WWW("http://stevenwyks.com/postCreations.php", form);
    Debug.Log(www.text);
    // // WWW www = new WWW("http://localhost/sqlconnect/savedata.php", form);
    yield return www;
    if (www.text == "0")
    {
      Debug.Log("Object info posted.");
    }
    else
    {
      Debug.Log("Save failed. Error #:" + www.text);
    }
    // DBManager.LogOut();
    // UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    // Debug.Log("posting object with x coordinate: " + childInContainer.position.x);
    // yield break;
  }



  // public void CallLogin()
  // {
  //   StartCoroutine(LoginIEnumerator());
  // }

  // // Update is called once per frame
  // IEnumerator LoginIEnumerator()
  // {
  //   WWWForm form = new WWWForm();
  //   form.AddField("name", nameField.text);
  //   form.AddField("password", passwordField.text);

  //   // WWW www = new WWW("http://localhost/sqlconnect/login.php", form);
  //   WWW www = new WWW("http://stevenwyks.com/loginDeploy.php", form);

  //   yield return www;
  //   if (www.text[0] == '0')
  //   {
  //     DBManager.username = nameField.text;
  //     DBManager.score = int.Parse(www.text.Split('\t')[1]);
  //     UnityEngine.SceneManagement.SceneManager.LoadScene(0);

  //   }
  //   else
  //   {
  //     Debug.Log("user log in failed. Err #" + www.text);
  //   }

  // }
  // public void VerifyInputs()
  // {
  //   submitButton.interactable = (nameField.text.Length >= 8 && passwordField.text.Length >= 8);
  // }
}