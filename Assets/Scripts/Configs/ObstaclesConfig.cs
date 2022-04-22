using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstaclesConfig", menuName = "Configs/ObstaclesConfig", order = 0)]
public class ObstaclesConfig : ScriptableObject
{
    [SerializeField] private Obstacle[] _obstacles;

    public Obstacle GetRandomObstacle()
    {
        int v = Random.Range(0, _obstacles.Length);
        return _obstacles[v];
    }
}
