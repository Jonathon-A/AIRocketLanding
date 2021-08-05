using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform BargeCentrePos;
    private List<Transform> CentrePosArray = new List<Transform>();
    // Update is called once per frame
    private void Start()
    {
        CentrePosArray = new List<Transform>();
        foreach (GameObject CentrePos in GameObject.FindGameObjectsWithTag("RocketCentre"))
        {
            CentrePosArray.Add(CentrePos.transform);
        }
    }
    void Update()
    {
        if (GameController.GetIsRender())
        {

        
        float Distance = Vector3.Distance(CentrePosArray[0].position, BargeCentrePos.position);
        Transform Target = CentrePosArray[0];
        for (int i = 1; i < CentrePosArray.Count; i++)
        {
            float NewDistance = Vector3.Distance(CentrePosArray[i].position, BargeCentrePos.position);
            if (NewDistance < Distance)
            {
                Distance = NewDistance;
                Target = CentrePosArray[i];
            }
        }
        
        transform.LookAt(Target, Vector3.left);
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0);
      
        transform.localPosition = new Vector3(0, 50, 20);
        transform.Translate(Vector3.forward * (Distance / zoomdiv));
        }
    }
    public float zoomdiv;
}
