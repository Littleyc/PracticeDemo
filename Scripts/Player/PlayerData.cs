using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//储存玩家所有数据
public class PlayerData
{
    public int HP;
    public int totalCrystal;//总水晶数
    public int currentCrystal;//当前水晶数

    public PlayerData(int hp,int _totalCrystal,int _currentCrystal)
    {
        HP = hp;
        totalCrystal = _totalCrystal;
        currentCrystal = _currentCrystal;
    }
}
