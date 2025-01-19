using System;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindAnyObjectByType<GameManager>();

                if(instance == null )
                    instance = new GameManager();
            }
                
            return instance;
        }
    }

    [SerializeField] private Material ballMaterial;
    [SerializeField] private GameObject ballPrefab;

    private BallColor ballColor = BallColor.RED;
    public BallColor CurrentBallColor => ballColor;
    
    public void CreateBall(CollisionBox collisionBox)
    {

    }

    public enum BallColor
    {
        RED,
        BLUE
    }

    private void Awake()
    {
    }

    

}
