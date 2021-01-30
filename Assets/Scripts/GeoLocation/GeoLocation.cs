﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeoLocation : MonoBehaviour
{
  public static float UserLongitude = 10;
  public static float UserLatitude = 10;
  // Start is called before the first frame update
  void Start()
  {
    Debug.Log("In start!");
    if (Input.location.isEnabledByUser)
    {
      Debug.Log("Input is enabled by user!");
      StartCoroutine(GetLocation());
    }
    InvokeRepeating("GetLocationUpdate", 2.0f, 0.5f);
  }


  private IEnumerator GetLocation()
  {
    Input.location.Start();
    while (Input.location.status == LocationServiceStatus.Initializing)
    {
      yield return new WaitForSeconds(0.4f);
    }
    UserLatitude = Input.location.lastData.latitude;
    UserLongitude = Input.location.lastData.longitude;
    Debug.Log("In IEnumerator: " + UserLatitude);
    yield break;
  }
  void GetLocationUpdate()
  {
    UserLatitude = Input.location.lastData.latitude;
    UserLongitude = Input.location.lastData.longitude;
  }

  // Update is called once per frame
  void Update()
  {
  }
}
