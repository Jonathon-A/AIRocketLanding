using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetCOM : MonoBehaviour
{
   public Vector3 COM;
    void Awake()
    {
        Rigidbody RB = gameObject.GetComponent<Rigidbody>();
        RB.centerOfMass = COM;
        
    }
}
