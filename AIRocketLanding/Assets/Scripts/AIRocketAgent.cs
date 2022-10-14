using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
public class AIRocketAgent : Agent
{
    public Transform BargeCentrePos;
    public override void OnEpisodeBegin()
    {
        // BargePos.position = new Vector3(Random.Range(-1000, 1000), 0, Random.Range(-1000, 1000));

        RB.velocity = Vector3.zero;
        RB.angularVelocity = Vector3.zero;
        transform.position = new Vector3(0f + Random.Range(-10f, 10f), 7266f + Random.Range(-10f, 10f), 718f + Random.Range(-10f, 10f));
        transform.rotation = Quaternion.Euler(-72 + Random.Range(-1f, 1f), 0, 0);
        RB.velocity = -transform.forward * (423f + Random.Range(-10f, 10f));
        RB.angularVelocity = Vector3.zero;

        Throttle = 0f;
        
        BoosterCentrePos.localRotation = Quaternion.Euler(0, 0, 0);

    }
   
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(BoosterCentrePos.position * 0.0001f);
        sensor.AddObservation(BargeCentrePos.position * 0.0001f);
        sensor.AddObservation(transform.rotation);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {

        //Debug.Log("c1 " + actions.ContinuousActions[0] + "  c2 " + actions.ContinuousActions[1] + "  c3 " + actions.ContinuousActions[2]);


       
        Throttle = actions.ContinuousActions[0];

        Throttle = Mathf.Clamp(Throttle, 0f, 1f);

        float GimbalX = actions.ContinuousActions[1] * 10f;
        float GimbalY = actions.ContinuousActions[2] * 10f;

        GimbalX = (GimbalX > 180) ? GimbalX - 360 : GimbalX;
        GimbalX = Mathf.Clamp(GimbalX, -10f, 10f);

        GimbalY = (GimbalY > 180) ? GimbalY - 360 : GimbalY;
        GimbalY = Mathf.Clamp(GimbalY, -10f, 10f);

        BoosterCentrePos.localRotation = Quaternion.Euler(GimbalX, GimbalY, 0);

        RB.AddForceAtPosition(BoosterCentrePos.forward * Throttle * 781000 * Time.fixedDeltaTime, BoosterCentrePos.position, ForceMode.Impulse);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {

        ActionSegment<float> ContinuousActions = actionsOut.ContinuousActions;

        if (Input.GetKey(KeyCode.Space))
        {
            ContinuousActions[0] = 1f;
        }else
        {
            ContinuousActions[0] = 0f;
        }
       
        
        ContinuousActions[1] = Input.GetAxis("Vertical");
        ContinuousActions[2] = Input.GetAxis("Horizontal");
    }

    private Rigidbody RB;
    private void Start()
    {
        RB = gameObject.GetComponent<Rigidbody>();
    }

    public Transform BoosterCentrePos;
    public float Throttle = 0f;
    public ParticleSystem Exhaust;
    public ParticleSystem Smoke;


    public AudioSource Sound;


    void FixedUpdate()
    {
        Velocity = RB.velocity.magnitude;
        AngularVelocity = RB.angularVelocity.magnitude;


        if (GameController.GetIsRender())
        {

            var ExhaustEmmission = Exhaust.emission;
            ExhaustEmmission.enabled = true;

            var main = Exhaust.main;
            main.startSpeed = Throttle * 30f;

            var main2 = Smoke.main;
            main2.startSpeed = Throttle * 30f;

            var SmokeEmmission = Smoke.emission;

            if (Throttle == 0)
            {
                Sound.mute = true;
                SmokeEmmission.enabled = false;
            }
            else
            {
                Sound.mute = false;
                SmokeEmmission.enabled = true;
            }

        }
        else {
            Sound.mute = true;
            var SmokeEmmission = Smoke.emission;
            SmokeEmmission.enabled = false;

            var ExhaustEmmission = Exhaust.emission;
            ExhaustEmmission.enabled = false;
        }
       
        //Debug.Log(StepCount + " " + MaxStep);
        if (StepCount >= MaxStep - 5)
        {     
            deathReward();
        }

    }

    public GameObject ExplosionObject;

    float Velocity;
    float AngularVelocity;
    
  
    private void OnCollisionEnter(Collision collision)
    {
        OnCollision(collision);
    }
    private void OnCollisionStay(Collision collision)
    {
        OnCollision(collision);
    }

    private void OnCollision(Collision collision)
    {
        if (Velocity > 10f || AngularVelocity > 0.3f || collision.gameObject.CompareTag("Ocean"))
        {
            if (GameController.GetIsRender())
            {
                Instantiate(ExplosionObject, BoosterCentrePos.position, new Quaternion(0, 0, 0, 0));
            }

            deathReward();
        }
       
    }

    private void deathReward() {
        float total = -Vector3.Distance(BoosterCentrePos.position, BargeCentrePos.position);
        total -= Velocity;
        total -= -Vector3.Angle(transform.forward, Vector3.up);

        SetReward(total);

        //Debug.LogWarning(total);
        EndEpisode();

    }


}
