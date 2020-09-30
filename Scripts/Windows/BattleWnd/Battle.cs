using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battle : BaseWnd
{
    public GameObject cardPrefab;

    #region 水晶显示相关数据
    private Text txtPlayerCrystal;//玩家水晶数
    private Text txtEnemyCrystal;//敌人水晶数
    private GridLayoutGroup crystalLst;//水晶列表
    public Sprite crystalBright;//当前可用水晶图片
    public Sprite crystalDark;//当前已用水晶图片
    private int currentCrystalBright;//当前可用水晶数
    private int currentCrystalDark;//当前已用水晶数
    #endregion

    #region 操作记录相关数据
    private GameObject operatingRecord;//操作记录的对象
    public GameObject operatedPrefab;//操作记录预制体
    private Sprite operatedSprite;//操作记录上显示的图片      //todo
    private Vector3 enterPos;//操作记录预制体生成的位置      本地坐标
    private Vector3 outPos;//操作记录预制体结束的位置
    private int xDistance;//操作记录预制体进入和出去时水平移动的距离
    private int yDistance;//操作记录预制体竖直方向移动的距离
    private float time;//移动的动画时间
    #endregion

    #region 回合结束按钮
    private Text txtTurn;
    private Button btnTurn;
    #endregion

    private Image imgRope;

    private ArrangeCard handCardManager;

    public FightCard fightCard;

    public override void Init()
    {
        //Debug.Log("Battle init...");
        #region 水晶显示相关数据初始化
        Text[] txts = transform.GetComponentsInChildren<Text>();
        foreach (Text item in txts)
        {
            //Debug.Log(item.gameObject.name);
            if(item.transform.parent.name == "imgHeadPortraitHeroPlayer")
            {
                txtPlayerCrystal = item;
            }
            else if(item.transform.parent.name == "imgHeadPortraitHeroEnemy")
            {
                txtEnemyCrystal = item;
            }
        }

        crystalLst = transform.GetComponentInChildren<GridLayoutGroup>();
        currentCrystalBright = 0;
        currentCrystalDark = 0;

        RefreshUI();
        #endregion

        #region 操作记录数据初始化
        operatingRecord = transform.Find("OperatingRecord").gameObject;
        time = 0.1f;
        xDistance = 400;
        yDistance = 70;
        enterPos = new Vector3(-400, 175, 0);
        outPos = new Vector3(-400, -175, 0);
        #endregion


        #region 回合结束按钮数据初始化
        btnTurn = transform.Find("btnTurn").GetComponent<Button>();
        txtTurn = btnTurn.GetComponentInChildren<Text>();
        txtTurn.text = "结束回合";
        txtTurn.color = new Color(0, 1, 0, 1);
        //GameManager.instance.isPlayerTurn = true;
        btnTurn.interactable = true;

        #endregion

        resSvc = ResSvc.instance;
        imgRope = transform.Find("imgRope").GetComponent<Image>();
        imgRope.color = new Color(1, 1, 1, 0);

        handCardManager = transform.GetComponentInChildren<ArrangeCard>();
        handCardManager.Init();

        fightCard = transform.Find("FightCardArea").GetComponent<FightCard>();
    }

    private void Update()
    {

        //测试脚本
        //模拟操作记录
        if (Input.GetKeyDown(KeyCode.J))
        {
            RecordManager();
        }

        //模拟抽卡
        if (Input.GetKeyDown(KeyCode.K))
        {
            CardControl newCard = CreateCard(resSvc.cardLst[3]);
            //TODO
            //把卡牌加入手牌
            StartCoroutine(CardToAddInHand(resSvc.cardLst[3], newCard));
            
        }

        //模拟召唤随从
        if (Input.GetKeyDown(KeyCode.M))
        {
            fightCard.AddCardInFightArea(resSvc.cardLst[4]);
        }
    }

    //在抽卡动画结束后生成要添加进手牌的卡
    public IEnumerator CardToAddInHand(CardInfo newCard ,CardControl card)
    {
        yield return new WaitForSeconds(1f);
        Vector3 newCardPos = new Vector3(471, 381, 0);
        //GameObject newCardAddInCard = Instantiate(cardPrefab, handCardManager.transform);

        GameObject newCardAddInCard = Instantiate(cardPrefab, handCardManager.transform);
        CardControl cardCtrl = newCardAddInCard.GetComponent<CardControl>();
        cardCtrl.Init(newCard);
        handCardManager.cardsInHand.Add(cardCtrl);

        DestroyImmediate(newCardAddInCard.GetComponent<Animator>());
        newCardAddInCard.transform.localScale = new Vector3(0.25f, 0.25f, 1);
        newCardAddInCard.transform.localPosition = newCardPos;
        newCardAddInCard.transform.localRotation = Quaternion.identity;
        handCardManager.Arrange();

        Destroy(card.gameObject);
    }

    public override void OnHide()
    {
        base.OnHide();
    }

    public override void OnShow()
    {
        base.OnShow();
    }

    //每次有数据更新时就刷新UI
    public override void RefreshUI()
    {
        GameManager gameManager = GameManager.instance;
        //Debug.Log(GameManager.instance.isPlayerTurn);
        //Debug.Log(txtPlayerCrystal.text);
        //Debug.Log(gameManager.player.currentCrystal);
        //Debug.Log(gameManager.player.totalCrystal);
        txtPlayerCrystal.text = gameManager.player.currentCrystal + "/" + gameManager.player.totalCrystal;

        //玩家可用水晶和已用水晶
        int playerBrightCrystal = gameManager.player.currentCrystal;
        int playerDarkCrystal = gameManager.player.totalCrystal - playerBrightCrystal;
        int playerTotalCrystal = gameManager.player.totalCrystal;

        if (playerTotalCrystal == currentCrystalDark + currentCrystalBright && playerBrightCrystal != currentCrystalBright)
        {
            //水晶总数一样，但可用水晶发生改变
            //求出亮水晶变化量
            int crystalDelta = playerBrightCrystal - currentCrystalBright;
            //找到变化水晶的位置
            int indexDark = 0;

            if(playerTotalCrystal == currentCrystalBright && crystalDelta<0)
            {
                //之前是满的亮水晶的情况下,是没有暗水晶的
                indexDark = playerTotalCrystal;
            }
            else
            {
                for (int i = 0; i < playerTotalCrystal; i++)
                {
                    //先找到第一个暗水晶的位置
                    if (crystalLst.transform.GetChild(i).GetComponent<Image>().sprite == crystalDark)
                    {
                        indexDark = i;
                        break;
                    }
                }
            }
            


            for (int i = indexDark , j = 0; j < Mathf.Abs(crystalDelta); j++)
            {
                if (crystalDelta > 0)
                {
                    crystalLst.transform.GetChild(i).GetComponent<Image>().sprite = crystalBright;
                    i++;
                }
                else if (crystalDelta < 0)
                {
                    crystalLst.transform.GetChild(i - 1).GetComponent<Image>().sprite = crystalDark;
                    i--;
                }

            }

            currentCrystalBright = playerBrightCrystal;
            currentCrystalDark = playerDarkCrystal;
        }
        else if (playerTotalCrystal > currentCrystalDark + currentCrystalBright)
        {
            //水晶总数发生了变化

            //1、增加空水晶
            //2、增加可用水晶

            if (currentCrystalBright < playerBrightCrystal)
            {
                //2、增加可用水晶
                for (int i = 0; i < playerTotalCrystal - currentCrystalDark - currentCrystalBright; i++)
                {
                    GameObject crystal = new GameObject("Crystal");
                    crystal.AddComponent<RectTransform>();
                    crystal.AddComponent<Image>();
                    GameObject newCrystal = Instantiate(crystal, crystalLst.transform);
                    newCrystal.GetComponent<Image>().sprite = crystalBright;
                    newCrystal.transform.SetSiblingIndex(gameManager.player.totalCrystal - 1);
                    currentCrystalDark += playerTotalCrystal - currentCrystalDark - currentCrystalBright;
                }
            }
            else
            {
                //1、增加空水晶
                for (int i = 0; i < playerTotalCrystal - currentCrystalDark - currentCrystalBright; i++)
                {
                    GameObject crystal = new GameObject("Crystal");
                    crystal.AddComponent<RectTransform>();
                    crystal.AddComponent<Image>();
                    GameObject newCrystal = Instantiate(crystal, crystalLst.transform);
                    newCrystal.GetComponent<Image>().sprite = crystalDark;
                    newCrystal.transform.SetSiblingIndex(gameManager.player.totalCrystal - 1);
                    currentCrystalDark += playerTotalCrystal - currentCrystalDark - currentCrystalBright;
                }

            }

            
        }
        currentCrystalBright = playerBrightCrystal;
        currentCrystalDark = playerDarkCrystal;

        #region 废弃代码
        //if (gameManager.player.currentCrystal> currentCrystalBright)
        //{
        //    int indexDark = 0;
        //    for (int i = 0; i < gameManager.player.totalCrystal; i++)
        //    {
        //        //先找到第一个暗水晶的位置
        //        if(crystalLst.transform.GetChild(i).GetComponent<Image>().sprite == crystalDark)
        //        {
        //            indexDark = i;
        //            break;
        //        }
        //    }
        //    for (int i = indexDark; i < gameManager.player.currentCrystal - currentCrystalBright; i++)
        //    {
        //        crystalLst.transform.GetChild(i).GetComponent<Image>().sprite = crystalBright;
        //    }
        //    currentCrystalBright = gameManager.player.currentCrystal;
        //}

        //if(gameManager.player.totalCrystal - gameManager.player.currentCrystal > currentCrystalDark)
        //{
        //    GameObject crystal = new GameObject("Crystal");
        //    crystal.AddComponent<RectTransform>();
        //    crystal.AddComponent<Image>();
        //    GameObject newCrystal = Instantiate(crystal, crystalLst.transform);
        //    newCrystal.GetComponent<Image>().sprite = crystalDark;
        //    newCrystal.transform.SetSiblingIndex(gameManager.player.totalCrystal-1);
        //    currentCrystalDark = gameManager.player.totalCrystal - gameManager.player.currentCrystal;
        //}
        //for (int i = 0; i < gameManager.player.currentCrystal; i++)
        //{
        //    GameObject crystal = new GameObject("Crystal" + (i + 1));
        //    crystal.AddComponent<RectTransform>();
        //    crystal.AddComponent<Image>();
        //    GameObject newCrystal = Instantiate(crystal, crystalLst.transform);
        //    newCrystal.GetComponent<Image>().sprite = crystalBright;
        //}

        //for (int i = gameManager.player.currentCrystal; i < gameManager.player.totalCrystal; i++)
        //{
        //    GameObject crystal = new GameObject("Crystal" + (i + 1));
        //    crystal.AddComponent<RectTransform>();
        //    crystal.AddComponent<Image>();
        //    GameObject newCrystal = Instantiate(crystal, crystalLst.transform);
        //    newCrystal.GetComponent<Image>().sprite = crystalDark;
        //}
        #endregion
    }

    //变更回合
    public void ChangeTurn()
    {
        if (GameManager.instance.isPlayerTurn)
        {
            //当前是玩家回合，玩家点击结束
            txtTurn.text = "对方回合";
            txtTurn.color = new Color(1, 1, 1, 0.5f);
            //GameManager.instance.isPlayerTurn = false;
            btnTurn.interactable = false;
        }
        else
        {
            //当前是对方回合，对方点击结束
            txtTurn.text = "结束回合";
            txtTurn.color = new Color(0, 1, 0, 1);
            //GameManager.instance.isPlayerTurn = true;
            btnTurn.interactable = true;
        }

        GameManager.instance.ResetTurnInfo();
    }

    //管理操作记录部分的显示
    public void RecordManager()
    {
        //新生成的一个
        GameObject newOperateRecordPrefab = Instantiate(operatedPrefab, transform.position + enterPos, Quaternion.identity, operatingRecord.transform);
        newOperateRecordPrefab.GetComponent<OperatingRecordMove>().Move(-xDistance, 175, 0, 175, time);

        //其他的向下移动
        for (int i = 0; i < operatingRecord.transform.childCount - 1; i++)
        {
            int count = operatingRecord.transform.childCount;
            Transform child = operatingRecord.transform.GetChild(i);
            child.GetComponent<OperatingRecordMove>().Move(0, (int)child.localPosition.y, 0, (int)(child.localPosition.y - 70), time);
        }

        if (operatingRecord.transform.childCount > 6)
        {
            Transform child = operatingRecord.transform.GetChild(0);
            child.GetComponent<OperatingRecordMove>().Move(0, (int)child.localPosition.y, -400, (int)child.localPosition.y, time);
            Destroy(child.gameObject, time);
        }
    }

    //抽一张卡
    public CardControl CreateCard(CardInfo cardInfo)
    {
        GameObject newCard = Instantiate(cardPrefab, transform);
        CardControl cardCtrl = newCard.GetComponent<CardControl>();
        cardCtrl.Init(cardInfo);
        return cardCtrl;
    }

    //烧绳子的进度
    public void BurnRopeProgress(float pro)
    {
        imgRope.fillAmount = pro;
    }
    //绳子的显示状态
    public void BurnRopeState(bool isShow)
    {
        imgRope.fillAmount = 1;
        if (isShow)
        {
            imgRope.color = new Color(1, 1, 1, 1);
        }
        else
        {
            imgRope.color = new Color(1, 1, 1, 0);
        }
    }
}
