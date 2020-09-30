using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectHero : BaseWnd
{
    public string[] heroNameLst =
    {
        "吉安娜",
        "雷克萨",
        "乌瑟尔",
        "加尔鲁什",
        "玛法里奥",
        "古尔丹",
        "萨尔",
        "安度因",
        "瓦莉拉"
    };
    public Text selectedHeroName;
    public Image selectedHeroImg;
    public Dictionary<string, Sprite> heroDict;//存储所有英雄及其对应的图片
    public GameObject heroImgLstGameObject;


    public override void Init()
    {
        heroDict = new Dictionary<string, Sprite>();
        heroImgLstGameObject = GameObject.FindGameObjectWithTag("HeroLst");//拿到管理9个英雄头像的对象
        for (int i = 0; i < heroNameLst.Length; i++)
        {
            Image currentHeroImg = heroImgLstGameObject.transform.GetChild(i).GetComponent<Image>();
            heroDict.Add(heroNameLst[i], currentHeroImg.sprite);
        }

        selectedHeroImg = transform.Find("SelectedHero").Find("imgHero").GetComponent<Image>();
        selectedHeroImg.sprite = heroDict[heroNameLst[0]];//设置初始状态是吉安娜
        selectedHeroName = selectedHeroImg.GetComponentInChildren<Text>();
        selectedHeroName.text = heroNameLst[0];

    }

    public void OnClickHeroImg(int index)
    {
        ChangeSelectedHero(heroNameLst[index - 1]);
    }

    public void ChangeSelectedHero(string name)
    {
        selectedHeroImg.sprite = heroDict[name];
        selectedHeroName.text = name;
    }

    public override void OnShow()
    {
        this.gameObject.SetActive(true);
    }

    public override void OnHide()
    {
        this.gameObject.SetActive(false);
    }

    public void OnClickStartButton()
    {
        GameManager.instance.Init();//进入战斗时初始化战斗数据
        UImanager.Instance.PushWnd(UIWndType.Battle);
        
        //UImanager.Instance.RefreshUIByType(UIWndType.Battle);
        OnHide();
    }
}
