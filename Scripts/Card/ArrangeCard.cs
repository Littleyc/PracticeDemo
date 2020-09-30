using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 手牌管理器
/// </summary>
public class ArrangeCard : MonoBehaviour
{
    public List<CardControl> cardsInHand;//手牌的列表
    public float offsetX;//手牌间距
    public float maxOffsetAngle;//手牌最大旋转角度

    public void Init()
    {
        //在将第一张手牌添加进来时先初始化
        offsetX = 1000;
        maxOffsetAngle = 60;
        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    cardsInHand.Add(transform.GetChild(i).GetComponent<CardControl>());
        //}
    }


    //对手牌进行排列,每次手牌数量发生变化时需要调用一次
    public void Arrange()
    {
        if (cardsInHand.Count <= 0)
        {
            return;
        }

        float eachOffsetX = offsetX / (float)(cardsInHand.Count + 1);
        float eachOffsetAngel = maxOffsetAngle/ (float)(cardsInHand.Count + 1);
        float originalX = -offsetX / 2;
        float originalAngel = maxOffsetAngle / 2;

        for (int i = 0; i < cardsInHand.Count; i++)
        {
            cardsInHand[i].transform.localPosition = new Vector3(originalX + eachOffsetX * (i + 1),0,0);
            cardsInHand[i].transform.localRotation = Quaternion.Euler(0, 0, originalAngel - eachOffsetAngel * (i + 1));
        }

        //if (!isEven)
        //{
        //    float eachAngel = maxOffsetAngle / (middleCardIndex - 1);
        //    //是奇数，中间的卡牌就不需要旋转
        //    for (int i = 0; i < middleCardIndex - 1; i++)
        //    {
        //        cardsInHand[i].transform.localRotation = Quaternion.Euler(0, 0, (middleCardIndex - 1 - i) * eachAngel);
        //        cardsInHand[i].transform.localPosition = - new Vector3(offsetX, 0, 0);
        //    }
        //    for (int i = middleCardIndex; i < cardsInHand.Count; i++)
        //    {
        //        cardsInHand[i].transform.localRotation = Quaternion.Euler(0, 0, -(middleCardIndex - 1 - i) * eachAngel);
        //        cardsInHand[i].transform.localPosition = new Vector3(offsetX, 0, 0);
        //    }
        //}
        //else
        //{
        //    float eachAngel = maxOffsetAngle / middleCardIndex;
        //    //是偶数
        //    for (int i = 0; i <= middleCardIndex - 1; i++)
        //    {
        //        cardsInHand[i].transform.localRotation = Quaternion.Euler(0, 0, (middleCardIndex - i) * eachAngel);
        //        cardsInHand[i].transform.localPosition = - new Vector3(offsetX, 0, 0);
        //    }
        //    for (int i = middleCardIndex; i < cardsInHand.Count; i++)
        //    {
        //        cardsInHand[i].transform.localRotation = Quaternion.Euler(0, 0, -(i - middleCardIndex + 1) * eachAngel);
        //        cardsInHand[i].transform.localPosition = new Vector3(offsetX, 0, 0);
        //    }
        //}
    }

    //将抽到的卡放入手牌
    public void AddCardInHand()
    {

    }
}
