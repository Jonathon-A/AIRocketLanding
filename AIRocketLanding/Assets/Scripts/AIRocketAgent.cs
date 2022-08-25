using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
public class AIRocketAgent : Agent
{
    private bool EpisodeBegun;
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
        EpisodeBegun = true;
        
        
        GimbalX = 0f;
        GimbalY = 0f;
        Throttle = 0f;
        
        BoosterCentrePos.localRotation = Quaternion.Euler(GimbalX, GimbalY, 0);

    }
   
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(BoosterCentrePos.position * 0.0001f);
        sensor.AddObservation(BargeCentrePos.position * 0.0001f);
        sensor.AddObservation(transform.rotation);
        sensor.AddObservation(Throttle);
        sensor.AddObservation(GimbalX);
        sensor.AddObservation(GimbalY);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        
        //Debug.Log("d1 " + actions.DiscreteActions[0]);
      //  Debug.Log("c1 " + actions.ContinuousActions[0]);
        //Debug.Log("c2 " + actions.ContinuousActions[1]);

        if (actions.DiscreteActions[0] == 0)
        {
            Throttle += 0.1f * 20f * Time.fixedDeltaTime;

        }
        else
        {
            Throttle -= 0.1f * 20f * Time.fixedDeltaTime;
        }
        Throttle = Mathf.Clamp(Throttle, 0f, 1f);

        InputGimbalX = actions.ContinuousActions[0] * 20f;
        InputGimbalY = actions.ContinuousActions[1] * 20f;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {

        ActionSegment<float> ContinuousActions = actionsOut.ContinuousActions;
        ActionSegment<int> DiscreteActions = actionsOut.DiscreteActions;

        if (Input.GetKey(KeyCode.Space))
        {
            DiscreteActions[0] = 0;

        }
        else
        {
            DiscreteActions[0] = 1;
        }
       
        ContinuousActions[0] = Input.GetAxis("Horizontal");
        ContinuousActions[1] = Input.GetAxis("Vertical");
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

    private float GimbalX;
    private float GimbalY;
    private float InputGimbalX;
    private float InputGimbalY;

    public AudioSource Sound;


    void FixedUpdate()
    {
        Velocity = RB.velocity.magnitude;
        AngularVelocity = RB.angularVelocity.magnitude;

        if (!EpisodeBegun)
        {

        
        GimbalX = BoosterCentrePos.localEulerAngles.x + InputGimbalX * Time.fixedDeltaTime;
        GimbalY = BoosterCentrePos.localEulerAngles.y + InputGimbalY * Time.fixedDeltaTime;

        GimbalX = (GimbalX > 180) ? GimbalX - 360 : GimbalX;
        GimbalX = Mathf.Clamp(GimbalX, -10f,10f);

        GimbalY = (GimbalY > 180) ? GimbalY - 360 : GimbalY;
        GimbalY = Mathf.Clamp(GimbalY, -10f, 10f);

        BoosterCentrePos.localRotation = Quaternion.Euler(GimbalX, GimbalY, 0);
        }

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

        if (!EpisodeBegun)
        {

        
        RB.AddForceAtPosition(BoosterCentrePos.forward * Throttle * 781000 * Time.fixedDeltaTime, BoosterCentrePos.position, ForceMode.Impulse);
        }

        if (StepCount >= MaxStep - 1)
        {

            AddReward(-Vector3.Distance(BoosterCentrePos.position, BargeCentrePos.position));
            AddReward(-Velocity);

            AddReward(-Vector3.Angle(transform.forward, Vector3.up));

         //   Debug.Log(GetCumulativeReward());
            EndEpisode();
        }
        EpisodeBegun = false;


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
          

            AddReward(-Vector3.Distance(BoosterCentrePos.position, BargeCentrePos.position));
            AddReward(-Velocity);

            AddReward(-Vector3.Angle(transform.forward, Vector3.up));

         //   Debug.LogWarning(GetCumulativeReward());
             EndEpisode();
        }
       
    }


}
