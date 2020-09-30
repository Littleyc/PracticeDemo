using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightCard : MonoBehaviour
{
    public List<FightCardControl> fightCardLst = new List<FightCardControl>();//拿到战斗区域的卡牌列表
    public float offsetX = 128;
    public GameObject cardPrefabInFightArea;

    //召唤随从，往战斗区域添加卡牌
    public void AddCardInFightArea(CardInfo card)
    {
        GameObject newCard = Instantiate(cardPrefabInFightArea, this.transform);
        //设置cardPrefabInFightArea里面的卡牌信息
        //todo
        if (fightCardLst == null)
        {
            fightCardLst = new List<FightCardControl>();
        }
        fightCardLst.Add(newCard.GetComponent<FightCardControl>());


        CalculatePos();
    }

    //新的卡牌添加到战斗区域时，重新计算一次所有卡牌的位置
    public void CalculatePos()
    {
        int count = fightCardLst.Count;

        for (int i = 0; i < count; i++)
        {
            fightCardLst[i].transform.localPosition = Vector3.zero;
        }

        if (count % 2 == 0)
        {
            //偶数张
            int midIndex = (count + 1) / 2;
            for (int i = 0; i < midIndex; i++)
            {
                fightCardLst[i].transform.localPosition -= new Vector3((midIndex - i) * offsetX, 0, 0);
            }
            for (int i = midIndex; i < count; i++)
            {
                fightCardLst[i].transform.localPosition += new Vector3((i - midIndex) * offsetX, 0, 0);
            }
            for (int i = 0; i < count; i++)
            {
                fightCardLst[i].transform.localPosition += new Vector3(offsetX / 2.0f, 0, 0);
            }
        }
        else
        {
            //奇数张
            int midIndex = count / 2;
            fightCardLst[midIndex].transform.localPosition = Vector3.zero;
            if (count <= 1)
            {
                return;
            }

            for (int i = 0; i < midIndex; i++)
            {
                fightCardLst[i].transform.localPosition -= new Vector3((midIndex - i) * offsetX, 0, 0);
            }
            for (int i = midIndex+1; i < count; i++)
            {
                fightCardLst[i].transform.localPosition += new Vector3((i - midIndex) * offsetX, 0, 0);
            }
        }
    }
}
