using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollisionBox : MonoBehaviour
{
    [SerializeField] private int count = 0;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI_Count;
    [SerializeField] private Renderer render;

    [SerializeField] private BallColor currentState;
    private BallColor CurrentState => currentState;

    private HashSet<GameObject> collisionData = new HashSet<GameObject>();

    private void OnEnable()
    {
        GameManager.ClickEvent += ChangeState;
    }

    private void OnDisable()
    {
        GameManager.ClickEvent -= ChangeState;
    }



    private void ChangeState()
    {

    }

    public void Setup()
    {
        textMeshProUGUI_Count.text = $"{count} X";
    }

    private void OnTriggerExit(Collider other)
    {
        if (collisionData.Contains(other.gameObject))
            return;

        GameManager.Instance.ReleaseBall(other.gameObject);

        if(CurrentState == GameManager.Instance.CurrentBallColor)
        {
            StartCoroutine(CoCollision(other.transform.position));
        }

    }

    private void AddCollisionBox(GameObject _obj)
    {
        if (collisionData.Contains(_obj))
            return;

        collisionData.Add(_obj);
    }

    private IEnumerator CoCollision(Vector3 _position)
    {
        for (int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(0.01f);

            GameManager.Instance.CreateBall(_position, AddCollisionBox);
        }
    }



    private void OnValidate()
    {
        if (textMeshProUGUI_Count == null || render == null)
            return;

        Setup();
    }

}
