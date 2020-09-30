using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : BaseWnd
{
    public override void Init()
    {
        Debug.Log("Battle init...");
    }
    public override void OnHide()
    {
        this.gameObject.SetActive(false);
    }

    public override void OnShow()
    {
        this.gameObject.SetActive(true);
    }

    public void OnClickStartButton()
    {
        UImanager.Instance.PushWnd(UIWndType.SelectHero);
        OnHide();
    }
}
