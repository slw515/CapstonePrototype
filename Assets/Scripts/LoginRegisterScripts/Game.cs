﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Game : MonoBehaviour
{

  public Text playerDisplay;
  public Text scoreDisplay;
  // Start is called before the first frame update
  private void Awake()
  {
    // if (DBManager.username == null)
    // {
    //   UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    // }
    playerDisplay.text = "Player: " + DBManager.username;
    scoreDisplay.text = "Score: " + DBManager.score;
  }

  public void CallSaveData()
  {
    StartCoroutine(SavePlayerData());
  }
  //Saves the data to db
  IEnumerator SavePlayerData()
  {
    WWWForm form = new WWWForm();
    form.AddField("name", DBManager.username);
    form.AddField("score", DBManager.score);
    WWW www = new WWW("http://stevenwyks.com/savedataDeploy.php", form);

    // WWW www = new WWW("http://localhost/sqlconnect/savedata.php", form);
    yield return www;
    if (www.text == "0")
    {
      Debug.Log("Game Saved.");
    }
    else
    {
      Debug.Log("Save failed. Error #:" + www.text);
    }
    DBManager.LogOut();
    UnityEngine.SceneManagement.SceneManager.LoadScene(0);

  }

  public void IncreaseScore()
  {
    DBManager.score++;
    scoreDisplay.text = "Score: " + DBManager.score;
  }
}
