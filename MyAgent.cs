using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using static UnityEngine.GraphicsBuffer;

public class MyAgent : Agent
{
    public Transform bone1;
    public Transform bone2;
    public Transform bone3;

    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    float step;

    public override void OnEpisodeBegin()
    {
        bone1.localRotation = Quaternion.identity;
        bone2.localRotation = Quaternion.identity;

        target.localPosition = new Vector3(Random.value * 8 - 4, Random.value * 4, Random.value * 8 - 4);
        step = 0;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        /*sensor.AddObservation(bone1.localRotation);
        sensor.AddObservation(bone2.position);
        sensor.AddObservation(bone2.localRotation);
        sensor.AddObservation(bone3.position);*/

        sensor.AddObservation(step/500);
        sensor.AddObservation(target.position/8f);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        step++;

        bone1.localRotation = Quaternion.Euler(
            actionBuffers.ContinuousActions[0] * 180,
            actionBuffers.ContinuousActions[1] * 180,
            actionBuffers.ContinuousActions[2] * 180);

        bone2.localRotation = Quaternion.Euler(         
            actionBuffers.ContinuousActions[3] * 180,
            actionBuffers.ContinuousActions[4] * 180,
            actionBuffers.ContinuousActions[5] * 180);

        float distanceToTarget = Vector3.Distance(bone3.position, target.position);

        float addRew = Mathf.Lerp(0, bone2.position.y * 0.01f, step/500);

        // Reached target
        if (distanceToTarget < 2f)
        {
            SetReward(2 - distanceToTarget + addRew);

            if(distanceToTarget == 0)
            {
                EndEpisode();
            }
        }
    }

}
