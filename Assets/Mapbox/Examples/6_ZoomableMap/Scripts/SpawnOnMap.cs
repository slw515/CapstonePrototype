namespace Mapbox.Examples
{
  using UnityEngine;
  using Mapbox.Utils;
  using Mapbox.Unity.Map;
  using Mapbox.Unity.MeshGeneration.Factories;
  using Mapbox.Unity.Utilities;
  using System.Collections.Generic;
  using SimpleJSON;
  using System.Collections;


  public class SpawnOnMap : MonoBehaviour
  {
    [SerializeField]
    AbstractMap _map;

    [SerializeField]
    [Geocode]
    List<string> _locationStrings;
    Vector2d[] _locations;

    [SerializeField]
    float _spawnScale = 100f;

    [SerializeField]
    GameObject _markerPrefab;

    List<GameObject> _spawnedObjects;
    private JSONNode parsedData;
    void Start()
    {
      StartCoroutine(CoroutineDemonstration());
      _spawnedObjects = new List<GameObject>();
      Debug.Log("Hey there: " + parsedData[0]);
    }

    IEnumerator CoroutineDemonstration()
    {
      while (true)
      {
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(PullLongLat.PullLongLatFromCreationTable());
        // yield return new WaitForSeconds(1.1f);
        parsedData = PullLongLat.parsedData;
        Debug.Log("parsed data is: " + parsedData);
        _locations = new Vector2d[parsedData.Count];
        // if (parsedData.Count != _spawnedObjects.Count)
        // {
        //   Debug.Log(parsedData.Count + " : " + _spawnedObjects.Count);
        for (int i = 0; i < _spawnedObjects.Count; i++)
        {
          _spawnedObjects[i].Destroy();
          _spawnedObjects.Remove(_spawnedObjects[i]);
        }
        _spawnedObjects = new List<GameObject>();
        // }

        for (int i = 0; i < parsedData.Count; i++)
        {
          string LatLonString = parsedData[i]["latitude"] + "," + parsedData[i]["longitude"];
          _locations[i] = Conversions.StringToLatLon(LatLonString);
          var instance = Instantiate(_markerPrefab);
          instance.GetComponent<DisplayModelFromDB>().modelID = parsedData[i]["id"];
          instance.GetComponent<DisplayModelFromDB>().latitude = parsedData[i]["latitude"];
          instance.GetComponent<DisplayModelFromDB>().longitude = parsedData[i]["longitude"];

          instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
          instance.name = parsedData[i]["id"];
          instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
          instance.transform.localPosition = new Vector3(i, 1, i);

          _spawnedObjects.Add(instance);
        }
        yield return new WaitForSeconds(3f);
      }
    }

    private void Update()
    {
#if UNITY_EDITOR
      // int count = _spawnedObjects.Count;
      // for (int i = 0; i < count; i++)
      // {
      //   var spawnedObject = _spawnedObjects[i];
      //   var location = _locations[i];
      //   spawnedObject.transform.localPosition = new Vector3(i, 1, i);
      //   spawnedObject.transform.localScale = new Vector3(1, 1, 1);
      // }
#endif
#if UNITY_IPHONE && !UNITY_EDITOR
      int count = _spawnedObjects.Count;
      for (int i = 0; i < count; i++)
      {
        var spawnedObject = _spawnedObjects[i];
        var location = _locations[i];
        spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
        spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
      }
#endif
    }
  }
}