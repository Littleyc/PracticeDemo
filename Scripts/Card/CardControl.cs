using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardControl : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public Sprite[] qualitySprites;

    private ResSvc resSvc;
    private Sprite cardBack;//卡背
    public Sprite cardBg;
    private Sprite cardSprite;//当前卡的图片
    private float cardBackScale = 0.48f;//卡背的比例，实际测试后最适合的比例
    private bool isBack;//记录卡牌当前状态，是正面还是背面
    private Image imgBg;

    private Image imgCard;
    private Text txtCardName;
    private Text txtCardDes;
    private Text txtHp;
    private Text txtAttack;
    private Text txtCost;
    private Image imgHp;//拿到这两个组件主要是对于没有该属性的卡牌需要删除
    private Image imgAttack;
    private Image imgQuality;
    private Image imgCost;

    private int indexInHand;
    private Quaternion angel;

    

    public void Init(CardInfo cardInfo)//传入要显示的数据
    {
        resSvc = ResSvc.instance;
        //Debug.Log(PathDefine.CardPathCfg + PathDefine.CardBackName);
        cardBack = Resources.Load<Sprite>(PathDefine.CardPathCfg + PathDefine.CardBackName);
        isBack = true;
        imgBg = GetComponent<Image>();

        imgCard = transform.Find("Mask").Find("imgCard").GetComponent<Image>();
        txtCardName = transform.Find("imgCardName").Find("txtCardName").GetComponent<Text>();
        txtCardDes = transform.Find("txtDescription").GetComponent<Text>();
        imgHp = transform.Find("imgHp").GetComponent<Image>();
        imgAttack = transform.Find("imgAttack").GetComponent<Image>();
        txtHp = imgHp.transform.GetComponentInChildren<Text>();
        txtAttack = imgAttack.transform.GetComponentInChildren<Text>();
        imgQuality = transform.Find("imgQuality").GetComponent<Image>();
        imgCost = transform.Find("imgCost").GetComponent<Image>();
        txtCost = imgCost.transform.GetComponentInChildren<Text>();

        //imgCard.sprite = cardBack;
        //imgCard.rectTransform.localScale = new Vector3(cardBackScale, cardBackScale, 1);
        //imgCard.transform.SetSiblingIndex(transform.childCount);
        imgBg.sprite = cardBack;
        imgBg.rectTransform.localEulerAngles = new Vector3(cardBackScale, cardBackScale, 1);

        SetCardInfoState(false);

        cardSprite = resSvc.LoadCard(cardInfo.fileName);
        txtCardName.text = cardInfo.name;
        txtCardDes.text = cardInfo.description;
        txtAttack.text = cardInfo.attack.ToString();
        txtHp.text = cardInfo.hp.ToString();
        txtCost.text = cardInfo.cost.ToString();
        if (cardInfo.quality != Quality.BASE)
            imgQuality.sprite = qualitySprites[cardInfo.qualityId - 2];
        else
            imgQuality.color = new Color(0, 0, 0, 0);
    }

    private void Update()
    {
        //检测是否需要将卡牌从背面翻转到正面
        if (isBack)
        {
            //Debug.Log(imgBg.transform.rotation.y);
            //Debug.Log(Quaternion.Euler(-0.3831f, -88.984f, 20.379f));
            //这边涉及到四元数问题，w是四元数通过角度计算后的值，具体数值是动画第35帧的数据
            if (imgBg.rectTransform.localRotation.w >Quaternion.Euler(-0.3831f, -88.984f, 20.379f).w)
            {
                imgBg.sprite = cardBg;
                imgCard.sprite = cardSprite;
                isBack = false;
                SetCardInfoState(true);
            }
        }

    }

    //控制卡面的显示状态
    private void SetCardInfoState(bool isShow = false)
    {
        transform.Find("Mask").gameObject.SetActive(isShow);
        transform.Find("imgCardName").gameObject.SetActive(isShow);
        txtCardDes.gameObject.SetActive(isShow);
        imgAttack.gameObject.SetActive(isShow);
        imgHp.gameObject.SetActive(isShow);
        imgQuality.gameObject.SetActive(isShow);
        imgCost.gameObject.SetActive(isShow);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localRotation = angel;
        if (isDrag)
        {
            isDrag = false;
            return;
        }

        this.transform.localScale = new Vector3(0.25f, 0.25f, 1);
        this.transform.position -= Vector3.up * 150;
        transform.SetSiblingIndex(indexInHand);
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.transform.localScale = new Vector3(0.4f, 0.4f, 1);
        originPos = this.transform.position;
        this.transform.position += Vector3.up * 150;
        indexInHand = transform.GetSiblingIndex();
        transform.SetSiblingIndex(transform.parent.childCount - 1);

        angel = transform.localRotation;
        transform.localRotation = Quaternion.identity;
    }


    private Vector3 originPos;
    private bool isDrag = false; //解决OnPointerExit和OnEndDrag在拖动卡牌结束时的冲突问题

    //拖拽卡牌还有两个问题
    //1、不能将卡牌脱出屏幕外
    //2、不能让卡牌在拖动的时候经过其他卡牌


    //开始拖拽时获取被拖拽的卡牌
    public void OnBeginDrag(PointerEventData eventData)
    {
        this.transform.localScale = new Vector3(0.25f, 0.25f, 1);
        isDrag = true;
    }

    //拖拽的时候移动卡牌
    public void OnDrag(PointerEventData eventData)
    {
        //this.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = Input.mousePosition;
    }

    //结束拖拽的时候判断卡牌状态
    //1、费用足够，召唤，将卡牌添加到战斗区域
    //2、费用不够，卡牌回到手牌原始位置
    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.position = originPos;
        this.transform.localScale = new Vector3(0.25f, 0.25f, 1);
        transform.SetSiblingIndex(indexInHand);

    }

    
}
