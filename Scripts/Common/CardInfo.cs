using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public enum CardType
{
    MINION = 1,//随从
    SPELL =2,//法术
    SECRET =3,//奥秘
    DK=4,//英雄卡
    WEAPON=5 //武器
}
public enum Job
{
    COMMON =1,
    MAGE=2,//法师
    HUNTER=3,//猎人
    WARRIOR=4,//战士
    SHAMAN=5,//萨满
    DRUID=6,//德鲁伊
    PRIEST=7,//牧师
    ROGUE=8,//潜行者
    PALADIN=9,//圣骑士
    WARLOCK=10//术士
}

public enum Quality
{
    BASE=1,
    COMMON=2,
    RARE=3,
    EPIC=4,
    LEGEND=5
}
public class CardInfo 
{
    public int id;
    public string name;
    public int typeId;
    public CardType cardType;
    public int qualityId;
    public Quality quality;
    public int jobId;
    public Job job;
    public int cost;
    public int attack;
    public int hp;
    public string fileName;
    public string description;
    

    public CardInfo(
        int id,
        string name,
        int typeId,
        int qualityId,
        int jobId,
        int cost,
        int attack,
        int hp,
        string fileName,
        string description)
    {
        this.id = id;
        this.name = name;
        this.typeId = typeId;
        this.qualityId = qualityId;
        this.jobId = jobId;
        this.cost = cost;
        this.attack = attack;
        this.hp = hp;
        this.fileName = fileName;
        this.description = description;

        this.cardType = (CardType)typeId;
        this.job = (Job)jobId;
        this.quality = (Quality)qualityId;
    }
}
