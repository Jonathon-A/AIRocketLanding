using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploder : MonoBehaviour
{
    private Rigidbody RB;
    private void Start()
    {
        RB = gameObject.GetComponent<Rigidbody>();
    }

    public GameObject ExplosionObject;

    float Velocity;
    float AngularVelocity;
    public Transform Pos;
    private void FixedUpdate()
    {
        Velocity = RB.velocity.magnitude;
        AngularVelocity = RB.angularVelocity.magnitude;
    }
    private void OnCollisionEnter(Collision collision)
    {
        OnCollision(collision);

    }
    private void OnCollisionStay(Collision collision)
    {;
        OnCollision(collision);
    }

    private void OnCollision(Collision collision)
    {
        //  Debug.Log("v " + Velocity);
        // Debug.Log("av " + AngularVelocity);

        if (Velocity > 10f || AngularVelocity > 0.3f || collision.gameObject.CompareTag("Ocean"))
        {
            GameObject ThisExplosion = Instantiate(ExplosionObject, Pos.position, new Quaternion(0,0,0,0));
            //      ThisExplosion.transform.position = ;
            //      ThisExplosion.transform.rotation = transform.rotation;
            //   Debug.Log(ThisExplosion.name);
        //    Destroy(gameObject);
        }
    }
}
