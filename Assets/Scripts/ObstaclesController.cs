using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesController : MonoBehaviour
{
    private ObstaclesConfig _config;

    private Obstacle _currentObstacle;
    private List<Obstacle> _spawnedObstacles;

    public void Initialize(ObstaclesConfig config)
    {
        _config = config;
        _spawnedObstacles = new List<Obstacle>();
    }


    public void NextObstacle()
    {
        _currentObstacle?.Hide();

        Obstacle newObstacle = _config.GetRandomObstacle();

        if(_currentObstacle != null)
            while (_currentObstacle.Key == newObstacle.Key)
            {
                newObstacle = _config.GetRandomObstacle();
            }

        if (_spawnedObstacles.Exists(x => x.Key == newObstacle.Key))
        {
            _currentObstacle = _spawnedObstacles.Find(x => x.Key == newObstacle.Key);
        }
        else
        {
            _currentObstacle = Instantiate(newObstacle, transform);
            _spawnedObstacles.Add(_currentObstacle);
        }

        _currentObstacle.Show();
    }

    public void LastObstacle()
    {
        _currentObstacle?.Show();
    }

    public void ClearScene()
    {
        _currentObstacle?.Hide();
    }
}
