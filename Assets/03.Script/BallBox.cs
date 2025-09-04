using DG.Tweening;
using System;
using UnityEngine;

public class BallBox : MonoBehaviour
{
    private Sequence closeBoxAnimation;
    private Sequence dropBoxAnimation;

    [SerializeField] private Transform boxBody;
    [SerializeField] private Transform leftBoxWing;
    [SerializeField] private Transform rightBoxWing;
    [SerializeField] private Transform frontBoxWing;
    [SerializeField] private Transform backBoxWing;
    
    public bool isRotate = false;

    void Start()
    {
        SetupBoxAnimation();
    }

    private void SetupBoxAnimation ()
    {
        closeBoxAnimation = DOTween.Sequence().SetAutoKill(false)
            .Append(leftBoxWing.DOLocalRotate(new Vector3(90f, 0f, -90f), 1))
            .Join(rightBoxWing.DOLocalRotate(new Vector3(90f, 0f, 90f), 1))
            .Append(frontBoxWing.DOLocalRotate(new Vector3(0, 0, 0), 1))
            .Join(backBoxWing.DOLocalRotate(new Vector3(180f, 0f, 0f), 1))
            .Append(boxBody.DOLocalRotate(new Vector3(0, 0, -180), 1))
            .AppendCallback(() => { isRotate = true; });

        dropBoxAnimation = DOTween.Sequence().SetAutoKill(false)
            .Append(leftBoxWing.DOLocalRotate(new Vector3(90f, 0f, 45f), 1))
            .Join(rightBoxWing.DOLocalRotate(new Vector3(90f, 0f, -45f), 1))
            .Join(frontBoxWing.DOLocalRotate(new Vector3(130f, 0, 0), 1))
            .Join(backBoxWing.DOLocalRotate(new Vector3(45f, 0f, 0f), 1));
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            closeBoxAnimation.PlayForward();
        }

        if(Input.GetKeyUp(KeyCode.A))
        {
            if(isRotate)
            {
                dropBoxAnimation.PlayForward();
                return;
            }

            closeBoxAnimation.PlayBackwards();
        }
    }
}
