using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PullAvailableWords : MonoBehaviour
{
  public GameObject wordOne;
  public GameObject wordsOfTheDay;
  public GameObject noWords;
  public GameObject wordTwo;
  public GameObject wordThree;
  public InputField passwordField;
  private List<string> words;
  // Start is called before the first frame update
  void Start()
  {
    words = DBManager.wordsAvailable.Split(',').ToList<string>();
    Debug.Log("words.count = " + words.Count);
    // Debug.Log("worfs two is: " + words[2]);
    if (DBManager.wordsOnSameDay == 3)
    {
      wordOne.transform.parent.gameObject.SetActive(false);
      wordTwo.transform.parent.gameObject.SetActive(false);
      wordThree.transform.parent.gameObject.SetActive(false);
      noWords.SetActive(true);
      wordsOfTheDay.SetActive(false);
    }
    else if (DBManager.wordsOnSameDay == 2)
    {
      wordThree.transform.parent.gameObject.SetActive(false);
      wordOne.GetComponent<Text>().text = words[0];
      wordTwo.transform.parent.gameObject.SetActive(false);
    }
    else if (DBManager.wordsOnSameDay == 1)
    {
      wordThree.transform.parent.gameObject.SetActive(false);
      wordTwo.GetComponent<Text>().text = words[1];
      wordOne.GetComponent<Text>().text = words[0];
    }
    else
    {
      wordTwo.GetComponent<Text>().text = words[1];
      wordOne.GetComponent<Text>().text = words[0];
      wordThree.GetComponent<Text>().text = words[2];
    }
  }

  // Update is called once per frame
  void Update()
  {
  }

  public void ChangeSceneToCreate()
  {
    GameObject go = EventSystem.current.currentSelectedGameObject;
    if (go != null)
    {
      DataForPostingObject.objectName = go.transform.GetChild(0).GetComponent<Text>().text;
      Debug.Log("data for posting object name: " + DataForPostingObject.objectName);
#if UNITY_IPHONE && !UNITY_EDITOR
      SceneManager.LoadScene("XRTest");
#endif
#if UNITY_EDITOR
      SceneManager.LoadScene("CameraPanTest");
#endif
    }
    else
      Debug.Log("currentSelectedGameObject is null");
  }
}
