using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public static class DBManager
{
  public static string username = "stevenwyks";
  public static int score;

  public static bool LoggedIn { get { return username != null; } }

  public static void LogOut()
  {
    username = null;
  }
}
