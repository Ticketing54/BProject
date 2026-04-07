using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplicateController : MonoBehaviour
{
    [Header("BoxList")]
    [SerializeField] private List<ReplicateRange> boxList;

    private HashSet<Ball> ignoreBall_HashSet = new HashSet<Ball>();
    private GameManager gameManager => GameManager.Instance;

    private void Awake()
    {
        Setup();
    }

    private void Setup()
    {
        foreach (ReplicateRange boxData in boxList)
        {
            if (boxData != null)
            {
                boxData.Setup(ReplicateBall);
            }
        }
    }

    public void AddBoxList(ReplicateRange _range)
    {
        if (boxList.Contains(_range))
            return;

        boxList.Add(_range);
        _range.Setup(ReplicateBall);

    }

    private void ReplicateBall(Ball _ball, ReplicateRange _targetBox)
    {
        if (ignoreBall_HashSet.Contains(_ball))
            return;

        if (gameManager.CurrentBallColor != _targetBox.TargetColor)
        {
            gameManager.ReturnBall(_ball);  // 공이 ReplicateBox의 색과 다르면 공을 되돌려보냄
            return;
        }

        // 공이 ReplicateBox의 색과 같으면 공을 복제
        StartCoroutine(CoReplicateBall(_ball, _targetBox.Count));
    }

    private IEnumerator CoReplicateBall(Ball _ball, int _count)
    {
        ignoreBall_HashSet.Add(_ball);

        int ballCount = 1;
        Vector3 position = _ball.transform.position;

        while (ballCount < _count)
        {
            float xOffset =
                UnityEngine.Random.Range(-DataBundle.DUPLICATE_SPRAY_RANGE, DataBundle.DUPLICATE_SPRAY_RANGE);

            Vector3 sprayPosition = position;
            sprayPosition.x += xOffset;

            Ball newBall = gameManager.CreateBall(sprayPosition);

            ignoreBall_HashSet.Add(newBall);        // 무한생성 방지 

            newBall.gameObject.SetActive(true);

            ballCount++;

            yield return DataBundle.DUPLICATE_SPRAY_RANGE;
        }

        yield return null;
    }

}
