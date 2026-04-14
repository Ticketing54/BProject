using UnityEngine;

public class StaticObstacle : MonoBehaviour, IObstacle
{
    [SerializeField] private DataBundle.ObstacleType obstacleType;

    public void ApplyData(ObstacleData data)
    {
        transform.position = data.position;
        transform.rotation = data.rotation;
        transform.localScale = data.scale;
    }

    public ObstacleData GetObstacleData()
    {
        return new StaticObstacleData
        {
            prefabtype = obstacleType,
            position = transform.position,
            rotation = transform.rotation,
            scale = transform.localScale
        };
    }
}
