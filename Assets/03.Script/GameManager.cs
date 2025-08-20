using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public enum BallColor
{
    RED,
    BLUE
}

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


    public CinemachineTargetGroup group;

    public static Action ClickEvent;


    private void OnEnable()
    {
        ClickEvent += ChangeBallColor;
    }

    private void OnDisable()
    {
        ClickEvent -= ChangeBallColor;
    }

    private void ChangeBallColor()
    {
        ballColor= ballColor == BallColor.RED ? BallColor.BLUE : BallColor.RED;
    }

}
