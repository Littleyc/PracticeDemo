using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//处理战斗内核心逻辑
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public int roundNum;//当前回合数        总的回合数等于roundNum/2
    public PlayerData player;
    public PlayerData enemy;
    public bool isPlayerTurn;//记录是谁的回合
    public bool isFightStart;//判断战斗是否开始
    public float turnTime;//一个回合的时间
    public float burnRopeTime;//烧绳子的时间
    public float currentTime;//当前的时间
    public bool isRopeShow;//是否显示绳子

    private Battle battle;//拿到Battle界面的脚本组件，目前的用处是调用里面的更换回合方法

    private void Awake()
    {
        instance = this;
    }

    public void Init()
    {
        player = new PlayerData(30 ,1,1);
        //测试，默认是玩家起手
        isPlayerTurn = true;
        turnTime = 15f;
        burnRopeTime = 10f;
        roundNum = 1;
        isFightStart = true;
        isRopeShow = false;
       
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && player.currentCrystal <player.totalCrystal)
        {
            player.currentCrystal += 1;
            CalculateCrystal();
        }
        else if (Input.GetKeyDown(KeyCode.S) && player.totalCrystal<10)
        {
            player.currentCrystal += 1;
            player.totalCrystal += 1;
            CalculateCrystal();
        }
        else if (Input.GetKeyDown(KeyCode.D) && player.currentCrystal >0)
        {
            player.currentCrystal -= 1;
            CalculateCrystal();
        }


        if(isFightStart)
        {
            //战斗开始，进行计时，管理双方玩家的回合
            CalculateTurnTime();
        }
    }

    //调用刷新ui数据
    public void CalculateCrystal()
    {
        //每次水晶更新时会进行一次刷新
        UImanager.Instance.RefreshUIByType(UIWndType.Battle);
    }

    //计算回合时间
    public void CalculateTurnTime()
    {
        
        currentTime += Time.deltaTime;
        if(turnTime- currentTime < burnRopeTime)
        {
            //剩余时间不足开始烧绳子

            //TODO
            if (!isRopeShow)
            {
                battle.BurnRopeState(true);
                isRopeShow = true;
            }

            battle.BurnRopeProgress((turnTime - currentTime) / burnRopeTime);


        }

        if (turnTime - currentTime < 0)
        {
            //时间到了,交换回合

            //把绳子状态重置

            //把按钮状态进行切换
            
            battle.ChangeTurn();
            battle.BurnRopeState(false);
            isRopeShow = false;
        }
    }


    public void ResetTurnInfo()
    {
        //交换回合

        isPlayerTurn = !isPlayerTurn;
        currentTime = 0;
        roundNum += 1;

        //如果绳子正在烧， 就把把绳子状态重置
        battle.BurnRopeState(false);
        isRopeShow = false;

        if (isPlayerTurn)
        {
            Debug.Log("玩家回合");
            player.currentCrystal += 1;
            player.totalCrystal += 1;
            CalculateCrystal();
        }
        else
        {
            Debug.Log("对方回合");
        }
    }

    //处理战斗结束时的数据
    public void EndFight()
    {
        isFightStart = false;
    }

    //获取battle面板的脚本
    public void GetBattle()
    {
        battle = transform.GetComponentInChildren<Battle>();
    }
}
