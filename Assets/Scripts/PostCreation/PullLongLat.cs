using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullLongLat : MonoBehaviour
{
  ArrayList arlist = new ArrayList();
  public void Awake()
  {
    if (DBManager.username == null)
    {
      UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    StartCoroutine(PullLongLatFromCreationTable());
  }

  IEnumerator PullLongLatFromCreationTable()
  {
    WWWForm form = new WWWForm();

    WWW www = new WWW("http://stevenwyks.com/pullLongLatFromDB.php", form);
    Debug.Log("heya!");

    yield return www;
    Debug.Log("DSadas");
    Debug.Log(www.text);

    // yield return currIndex;
  }

  // Update is called once per frame
  void Update()
  {

  }
}
