using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.MLAgents;

public class Detector : MonoBehaviour
{
    public float detectionDistance = 1000f;
    public Color lineColor = Color.green;
    private DroneInfo _droneInfo;
    private DroneInfo _detectedDroneInfo; 
    private Transform detectedDroneTransform;
    private LineRenderer lineRenderer;
    private DroneInfo _mainDroneInfo;
    private Dictionary<string, int> trustedDronesInZone = new Dictionary<string, int>();
    private GameManager _gameManager;


    private void Start()
    {
        _mainDroneInfo = gameObject.GetComponent<DroneInfo>();
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void  FixedUpdate()
    {
        

        var colliders = Physics.OverlapSphere(transform.position, detectionDistance);
       
           // detect game object
           foreach (var collider in colliders)
           {
               
               // check if game object is drone
               if (collider.gameObject != null && collider.name.Contains("d"))
               {
                   _detectedDroneInfo = collider.GetComponent<DroneInfo>();
                   detectedDroneTransform = collider.transform;
                   // Update line renderer
                   var timer = Vector3.Distance(transform.position, detectedDroneTransform.position);
                   if (detectedDroneTransform == null || _detectedDroneInfo == null) continue;
                   // check if energy is greater or equal to 1000 (2)
                    if (_detectedDroneInfo.energy >= 900 && _mainDroneInfo.energy >= 900 && _detectedDroneInfo.zoneName != "Any" || _detectedDroneInfo.zoneName != "Any")
                    {
                        
                        // check if game object are belong to same zone (3)
                        if (_detectedDroneInfo.zoneName == _mainDroneInfo.zoneName)
                        {
                            // check if _detected drone energy is greater than main drone
                            if (_detectedDroneInfo.energy > _mainDroneInfo.energy )
                            {
                                switch (_detectedDroneInfo.zoneName)
                                {
                                    case "A" when _gameManager.leaderInstanceA != null:
                                    {
                                        // check if leaderInstance energy is less than _detectedDrone energy if true than add _detected drone to leaderInstance
                                        if (_gameManager.leaderInstanceA.energy <= _detectedDroneInfo.energy)
                                        {
                                            _detectedDroneInfo.leader = true;
                                            _gameManager.leaderInstanceA = _detectedDroneInfo;
                                           
                                        }

                                        break;
                                    }
                                    // check if _detected drone belong to zone B
                                    case "A":
                                        _gameManager.leaderInstanceA = _detectedDroneInfo;
                                        
                                        break;
                                    // check if leaderInstance is not null
                                    case "B" when _gameManager.leaderInstanceB != null:
                                    {
                                        // check if leaderInstance energy is less than _detectedDrone energy if true than add _detected drone to leaderInstance
                                        if (_gameManager.leaderInstanceB.energy < _detectedDroneInfo.energy)
                                        {
                                            _detectedDroneInfo.leader = true;
                                            Debug.Log(_detectedDroneInfo.leader);
                                            _gameManager.leaderInstanceB = _detectedDroneInfo;
                                           
                                        }

                                        break;
                                    }
                                    case "B":
                                        _gameManager.leaderInstanceB = _detectedDroneInfo;
                                        break;
                                }
                            }
                            // if condition with number 5 false change detectedDroneInfo leader to false and mainDroneInfo leader to true
                            else
                            {
                                switch (_mainDroneInfo.zoneName)
                                {
                                    // check if _detected drone belong to zone A
                                    // check if leaderInstance is not null
                                    case "A" when _gameManager.leaderInstanceA != null:
                                    {
                                        // check if leaderInstance energy is less than _detectedDrone energy if true than add _detected drone to leaderInstance
                                        if (_gameManager.leaderInstanceA.energy < _mainDroneInfo.energy)
                                        {
                                            _mainDroneInfo.leader = true;
                                            _gameManager.leaderInstanceA = _mainDroneInfo;
                                        }

                                        break;
                                    }
                                    // check if _detected drone belong to zone B
                                    case "A":
                                        _gameManager.leaderInstanceA = _mainDroneInfo;
                                        break;
                                    // check if leaderInstance is not null
                                    case "B" when _gameManager.leaderInstanceB != null:
                                    {
                                        // check if leaderInstance energy is less than _detectedDrone energy if true than add _detected drone to leaderInstance
                                        if (_gameManager.leaderInstanceB.energy < _mainDroneInfo.energy)
                                        {
                                            _mainDroneInfo.leader = true;
                                            _gameManager.leaderInstanceB = _mainDroneInfo;
                                        }

                                        break;
                                    }
                                    case "B":
                                        _gameManager.leaderInstanceB = _mainDroneInfo;
                                        break;
                                }
                            }
                        }
                        else
                        {
                            
                        }
                    }
                    else
                    {
                        
                        _detectedDroneInfo.leader = false;
                        
                        _mainDroneInfo.leader = false;
                    }
               }
               else
               {
                        
               }
                    
           }
    }
    private void DrawLine(Vector3 start, Vector3 end)
    {
        var lr = new GameObject("LineRenderer" + UnityEngine.Random.Range(1000, 2000)).AddComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = lineColor;
        lr.endColor = lineColor;
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.SetPositions(new Vector3[] { start, end });
    }
}