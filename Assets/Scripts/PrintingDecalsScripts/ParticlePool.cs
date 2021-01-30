using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]

public class ParticlePool : MonoBehaviour
{
  public GameObject thisObj;
  public GameObject thisAstprefab;

  public ParticleSystem particles;
  ParticleSystem.Particle[] gos = new ParticleSystem.Particle[1000];

  void Awake()
  {
    int index = 0;
    int total = 0;
    total = particles.GetParticles(gos);
    Vector3 pos = Vector3.zero;

    while (index < total)
    {
      pos = gos[index].position;
      pos.x = Mathf.RoundToInt(pos.x);
      pos.y = Mathf.RoundToInt(pos.y);
      gos[index].position = pos;
      Instantiate(thisAstprefab, gos[index].position, Quaternion.identity);
      index++;
    }
    if (index >= total)
    {
      thisObj.SetActive(false);
    }
    //particles.SetParticles(gos, total);
  }
}