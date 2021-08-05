using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticles : MonoBehaviour
{
    private IEnumerator Start()
    {
        var main = GetComponent<ParticleSystem>().main;
        yield return new WaitForSeconds(main.duration);
        Destroy(gameObject);
    }
}
