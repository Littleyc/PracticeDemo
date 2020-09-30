using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using TMPro;
using UnityEngine;

public class ResSvc : MonoBehaviour
{
    public static ResSvc instance;
    private void Awake()
    {
        instance = this;
        //Debug.Log("init resSvc...");
        InitCardCfg();

        //test
        //UnityEngine.Debug.Log(cardLst[0].name);
        //UnityEngine.Debug.Log(cardLst[0].quality);
        //UnityEngine.Debug.Log(cardLst[0].cost);
        //UnityEngine.Debug.Log(cardLst[0].attack);
        //UnityEngine.Debug.Log(cardLst[0].hp);
    }

    #region 卡牌信息输入

    public List<CardInfo> cardLst = new List<CardInfo>();
    private int id;
    private string name;
    private int typeId;
    private int qualityId;
    private int jobId;
    private int cost;
    private int attack;
    private int hp;
    private string fileName;
    private string des;
    private void InitCardCfg()
    {
        TextAsset xml = Resources.Load<TextAsset>(PathDefine.CardInfoXml);
        if (!xml)
        {
            UnityEngine.Debug.LogError("xml file:" + PathDefine.CardInfoXml + "not exist");
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);
            XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;
            //XmlElement ele = nodLst[0] as XmlElement;
            //Debug.Log(nodLst.Count);

            foreach (XmlElement item in nodLst)
            {
                //输出的是row，返回的是每一行的row标签，row标签内才是全部的数据
                //Debug.Log(item.Name);
                foreach (XmlElement ele in item.ChildNodes)
                {
                    switch (ele.Name)
                    {
                        case "ID":
                            id = Convert.ToInt32(ele.InnerText);
                            break;
                        case "Name":
                            name = ele.InnerText;
                            break;
                        case "Type":
                            typeId = Convert.ToInt32(ele.InnerText);
                            break;
                        case "Quality":
                            qualityId = Convert.ToInt32(ele.InnerText);
                            break;
                        case "Job":
                            jobId = Convert.ToInt32(ele.InnerText);
                            break;
                        case "Cost":
                            cost = Convert.ToInt32(ele.InnerText);
                            break;
                        case "Attack":
                            attack = Convert.ToInt32(ele.InnerText);
                            break;
                        case "HealthPoint":
                            hp = Convert.ToInt32(ele.InnerText);
                            break;
                        case "FileName":
                            fileName = ele.InnerText;
                            break;
                        case "Description":
                            des = ele.InnerText;
                            break;
                    }
                }
                CardInfo cardInfo = new CardInfo(id, name, typeId, qualityId, jobId, cost, attack, hp, fileName, des);
                cardLst.Add(cardInfo);
            }
        }
    }
    #endregion

    public Sprite LoadCard(string path)
    {
        return Resources.Load<Sprite>(PathDefine.CardPathCfg + path);
    }
}
