using UnityEngine;
using System.Collections;

public class SimpleMoveObject : MonoBehaviour {

    private float _moveTime;
    private Vector3 _targetPos;


    public void StartMove(Vector3 targetPos, float moveTime)
    {
        _moveTime = moveTime;
        _targetPos = targetPos;

        StartCoroutine("MoveToPosition");
    }

    IEnumerator MoveToPosition()
    {
        Vector3 startPosition = transform.position;

        float counter = _moveTime;

        while (counter > 0)
        {
            transform.position = Vector3.Lerp(startPosition, _targetPos, 1 - counter / _moveTime);
            counter -= Time.deltaTime;
            yield return null;
        }
    }
}
