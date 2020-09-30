using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperatingRecordMove : MonoBehaviour
{
    private IEnumerator moveCoroutine;

    public void Move(int x, int y, int nextX, int nextY, float time)
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = MoveCoroutine(x, y, nextX,nextY,time);
        StartCoroutine(moveCoroutine);
    }

    //实现移动的动画，参数表示移动的数据和时间
    public IEnumerator MoveCoroutine(int x,int y,int nextX,int nextY,float time)
    {
        Vector3 startPos = new Vector3(x, y, 0);
        Vector3 endPos = new Vector3(nextX, nextY, 0);
        for (float t = 0; t < time; t+=Time.deltaTime)
        {
            transform.localPosition = Vector3.Lerp(startPos, endPos, t/time);
            yield return 0;
        }
        transform.localPosition = endPos;
    }
}
