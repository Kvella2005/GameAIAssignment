using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.UIElements;
using UnityEngine.Rendering;
using System.Data.Common;

//Helped with AI

public class GladiatorMLAgentAi : Agent
{
    float health, maxHealth, ammo, maxAmmo, episodeTimer, maxEpisodeTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void OnEpisodeBegin()
    {
        base.OnEpisodeBegin();
        health = 50f;
        maxHealth = 100f;
        ammo = 20f;
        maxAmmo = 20f;
        episodeTimer = 0f;
        maxEpisodeTime = 30f;
    }

    void Update()
    {
        RequestAction();
        RequestDecision();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(health / maxHealth);
        sensor.AddObservation(ammo / maxAmmo);

        Rigidbody rb = GetComponent<Rigidbody>();
        sensor.AddObservation(rb.linearVelocity.normalized);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        base.OnActionReceived(actions);

        float moveStep = actions.ContinuousActions[0];
        float rotateStep = actions.ContinuousActions[1];
        Rigidbody rb = GetComponent<Rigidbody>();
        Vector3 moveForward = transform.forward * moveStep * 5f;
        rb.linearVelocity = new Vector3(moveForward.x, rb.linearVelocity.y, moveForward.z);
        transform.Rotate(Vector3.up * rotateStep * Time.deltaTime * 100f);

        episodeTimer += Time.deltaTime;
        if(episodeTimer >= maxEpisodeTime || health <= 0)
        {
            SetReward(-1.0f);
            EndEpisode();
        }

        AddReward(-0.001f);
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

    public void TakeDamage(float damageValue)
    {
        health -= damageValue;

        AddReward(-.05f);

        if(health <= 0)
        {
            SetReward(-1.0f);
            EndEpisode();

            gameObject.SetActive(false);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        string layerName = LayerMask.LayerToName(collision.gameObject.layer);
        if(layerName == "AI")
        {
            collision.gameObject.SendMessage("TakeDamage", 25f, SendMessageOptions.DontRequireReceiver);

            RegisteredHit();

            Debug.Log("Gladiator scored a hit on " + collision.gameObject.name);
            AddReward(.1f);
        }
    }
}
