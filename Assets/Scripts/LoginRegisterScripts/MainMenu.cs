using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
  public Text playerDisplay;
  void Start()
  {
    if (DBManager.LoggedIn)
    {
      playerDisplay.text = "Player: " + DBManager.username;
    }
  }
  // Start is called before the first frame update
  public void GoToRegister()
  {
    SceneManager.LoadScene(1);
  }

  public void GoToLogin()
  {
    SceneManager.LoadScene(2);
  }

  public void GoToGame()
  {
    SceneManager.LoadScene(3);
  }
}
