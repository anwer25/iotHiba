using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class TrustedInstance
{
    public string Id;
    public DroneInfo DroneInstance;
}

public class MlAgent : Agent
{
    public RayPerceptionSensorComponent3D rayPerceptionSensor;
    public string droneId;
    public string zoneName;
    public bool trusted;
    public bool canMove;
    public bool leader;
    public string error;
    public int energy;
    private MeshRenderer _meshRenderer;
    public Material[] newMaterial;
    private Methods.Methods _methods = new Methods.Methods();
    private const string targetTagName = "drone";
    private const int numZones = 3; // Replace with the actual number of zones
    private float episodeReward;
    private StatsRecorder statsRecorder;

    public override void Initialize()
    {
        rayPerceptionSensor = GetComponent<RayPerceptionSensorComponent3D>();
        _meshRenderer = GetComponent<MeshRenderer>();

        // Get the StatsRecorder from the Academy
        statsRecorder = Academy.Instance.StatsRecorder;
        energy = UnityEngine.Random.Range(1000, 5000);
    }

    public override void OnEpisodeBegin()
    {
        // Implement logic to reset the environment when a new episode begins
        MaterialChanger(0);
        string[] zones = { "A", "B", "Any" }; 
        droneId = _methods.GenerateUniqueId();
        trusted = false;
        zoneName = _methods.GenerateRandomZone(zones);

        // Reset the episode reward
        episodeReward = 0f;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        var rayPerceptionInput = rayPerceptionSensor.GetRayPerceptionInput();
        var output = RayPerceptionSensor.Perceive(rayPerceptionInput);

        // Add the length of the ray outputs as an observation
        // sensor.AddObservation(output.RayOutputs.Length);

        // Loop through the ray outputs
        for (int i = 0; i < output.RayOutputs.Length; i++)
        {
            var hit = output.RayOutputs[i].HitGameObject;

            // Check if a hit occurred and if the hit object has the target tag
            if (hit != null && hit.CompareTag(targetTagName))
            {
                MlAgent droneInfo = hit.GetComponent<MlAgent>();

                // Check if the hit object has the DroneInfo component
                if (droneInfo != null)
                {
                    Debug.Log(droneInfo.zoneName);
                    // Add relevant property of DroneInfo as an observation
                    int zoneIndex = GetZoneIndex(droneInfo.zoneName);
                    AddOneHotObservation(sensor, zoneIndex, numZones);
                }
                else
                {
                    // DroneInfo component not found on the detected GameObject
                    Debug.LogWarning("DroneInfo component not found on the detected GameObject.");
                }
            }
            else
            {
                // No GameObject with the specified tag detected
                Debug.Log("No GameObject with the specified tag detected.");
            }
        }
    }

    private int GetZoneIndex(string zoneName)
    {
        Debug.Log(zoneName);
        // Implement the logic to map zone names to corresponding one-hot encoding indices
        // Return the appropriate index based on the zone name
        return zoneName switch
        {
            "Any" => 0,
            "B" => 1,
            "A" => 2,
            _ => 0
        };
    }

    private void AddOneHotObservation(VectorSensor sensor, int index, int numClasses)
    {
        for (var i = 0; i < numClasses; i++)
        {
            sensor.AddObservation(i == index ? 1f : 0f);
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Get the action value from the buffer
        int action = actions.DiscreteActions[1];
        MaterialChanger(action);

        // Calculate the reward
        float reward = CalculateReward();
        episodeReward += reward;
        SetReward(reward);
    }

    private float CalculateReward()
    {
        float reward = 0f;

        // Implement your reward calculation logic here
        // Example:
        // If trusted and in zone A or B, reward +1
        // If not trusted, reward -1

        if (trusted && (zoneName == "A" || zoneName == "B"))
        {
            reward = 1f;
        }
        else
        {
            reward = -1f;
        }
        statsRecorder.Add("Episode Reward", episodeReward);
        return reward;
    }



    private void MaterialChanger(int value)
    {
        
        bool result = value > 0 ? true : false;
        if (_meshRenderer == null) return;
        // check if drone are trusted and not leader to add blue color to it
        if (result && (zoneName == "A" || zoneName == "B"))
        {
            Debug.Log("materialChanger" + value);
            _meshRenderer.material = newMaterial[0];
            trusted = true;
        }
        // add red color to drone which means not trusted
        else
        {
            trusted = false;
            _meshRenderer.material = newMaterial[1];
        }
    }
}
