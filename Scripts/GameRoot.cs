using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //实际入口
        UImanager.Instance.PushWnd(UIWndType.Welcome);

        //测试代码
        //UImanager.Instance.PushWnd(UIWndType.MainMenu);
        //GameManager.instance.Init();
    }

    
}
