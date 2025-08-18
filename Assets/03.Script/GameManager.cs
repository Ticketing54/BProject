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


    public void ReplicateBall(Vector3 _position, int _count, Action<GameObject> _applyCollisionBox = null)
    {
        StartCoroutine(CoReplicateBall(_position, _count, _applyCollisionBox));
    }

    private IEnumerator CoReplicateBall(Vector3 _position, int _count, Action<GameObject> _applyCollisionBox = null)
    {
        WaitForSeconds fixedUpdate = new WaitForSeconds(0.1f);

        for (int i = 0; i < _count; i++)
        {
            GameObject ball = Instantiate<GameObject>(ballPrefab);

            _applyCollisionBox?.Invoke(ball);

            Vector3 offset = UnityEngine.Random.insideUnitSphere * 0.35f +_position;
            offset.y = _position.y;

            ball.transform.position = offset;

            if (group.FindMember(ball.transform) == -1)
                group.AddMember(ball.transform, 1, 1);


            if (ball.TryGetComponent<Rigidbody>(out Rigidbody rig))
            {
                rig.angularVelocity = Vector3.zero;
            }


            yield return fixedUpdate;
        }
    }
}
