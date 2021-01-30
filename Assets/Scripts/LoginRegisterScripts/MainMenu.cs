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
    SceneManager.LoadScene("registermenu");
  }

  public void GoToLogin()
  {
    SceneManager.LoadScene("loginmenu");
  }

  public void GoToGame()
  {
    SceneManager.LoadScene("AstronautGame");
  }
}
