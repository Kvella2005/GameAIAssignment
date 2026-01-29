using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.UIElements;
using UnityEngine.Rendering;

//Helped with AI

public class GladiatorMLAgentAi : Agent
{
    float health, maxHealth, ammo, maxAmmo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void OnEpisodeBegin()
    {
        base.OnEpisodeBegin();
        health = 50f;
        maxHealth = 100f;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);

        sensor.AddObservation(health / maxHealth);
        sensor.AddObservation(ammo / maxAmmo);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        base.OnActionReceived(actions);

        float moveStep = actions.ContinuousActions[0];
        float rotateStep = actions.ContinuousActions[1];
        transform.Translate(Vector3.forward * moveStep * Time.deltaTime);
        transform.Rotate(Vector3.up * rotateStep * Time.deltaTime * 100f);
        AddReward(.01f);

        AddReward(-.0005f);

        if(health <= 0)
        {
            SetReward(-1.0f);
            EndEpisode();
        }
    }

    public void RegisteredHit()
    {
        AddReward(.5f);
    }

    public void WonMatch()
    {
        SetReward(2.0f);
        EndEpisode();
    }
}
