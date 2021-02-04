using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using System.IO;

public class Registration : MonoBehaviour
{
  public InputField nameField;
  public InputField passwordField;
  public Button submitButton;
  private JSONNode parsedDBWords;
  string str;

  public void CallRegister()
  {
    StartCoroutine(Register());

  }

  IEnumerator Register()
  {
    string path = Application.dataPath + "/Scripts/LoginRegisterScripts/WordDB.json";
    string jsonString = File.ReadAllText(path);
    JSONObject playerJson = (JSONObject)JSON.Parse(jsonString);
    string text = playerJson["words"].ToString().Substring(1, playerJson["words"].ToString().Length - 2);
    text = text.Replace("\"", "");
    string[] words = text.Split(',');
    string[] shuffledWords = reshuffle(words);
    Debug.Log(shuffledWords[0] + ", " + shuffledWords[1]);
    for (var i = 0; i < shuffledWords.Length; i++)
    {
      str += shuffledWords[i].ToString() + ",";
    }
    WWWForm form = new WWWForm();
    form.AddField("name", nameField.text);
    form.AddField("password", passwordField.text);
    form.AddField("words", str);
    WWW www = new WWW("http://stevenwyks.com/registerDeploy.php", form);

    yield return www;
    if (www.text == "0")
    {
      Debug.Log("User created successfully!");
      UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    else
    {
      Debug.Log("User creation failed. Error #: " + www.text);
    }
  }
  public void VerifyInputs()
  {
    submitButton.interactable = (nameField.text.Length >= 8 && passwordField.text.Length >= 8);
  }

  string[] reshuffle(string[] texts)
  {
    for (int t = 0; t < texts.Length; t++)
    {
      string tmp = texts[t];
      int r = Random.Range(t, texts.Length);
      texts[t] = texts[r];
      texts[r] = tmp;
    }
    return texts;
  }

  public void ReturnToMain()
  {
    UnityEngine.SceneManagement.SceneManager.LoadScene(0);
  }

}
