using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
  public InputField nameField;
  public InputField passwordField;
  public Button submitButton;

  public void CallLogin()
  {
    StartCoroutine(LoginIEnumerator());
  }

  // Update is called once per frame
  IEnumerator LoginIEnumerator()
  {
    WWWForm form = new WWWForm();
    form.AddField("name", nameField.text);
    form.AddField("password", passwordField.text);

    // WWW www = new WWW("http://localhost/sqlconnect/login.php", form);
    WWW www = new WWW("http://stevenwyks.com/loginDeploy.php", form);

    yield return www;
    if (www.text[0] == '0')
    {
      DBManager.wordsOnSameDay = int.Parse(www.text.Split('\t')[3]);

      DBManager.username = nameField.text;
      DBManager.score = int.Parse(www.text.Split('\t')[1]);
      DBManager.wordsAvailable = www.text.Split('\t')[2];
      UnityEngine.SceneManagement.SceneManager.LoadScene("AstronautGame");
    }
    else
    {
      Debug.Log("user log in failed. Err #" + www.text);
    }

  }
  public void VerifyInputs()
  {
    submitButton.interactable = (nameField.text.Length >= 8 && passwordField.text.Length >= 8);
  }

  public void ReturnToMain()
  {
    UnityEngine.SceneManagement.SceneManager.LoadScene(0);
  }
}
