using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class UImanager 
{
    private static UImanager instance;
    public static UImanager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new UImanager();
            }
            return instance;
        }
    }

    //获取画布的Transform，之后的panel都会生成在画布下
    private Transform canvasTransform;
    public Transform CanvasTransform
    {
        get
        {
            if (canvasTransform == null)
            {
                canvasTransform = GameObject.Find("Canvas").transform;
            }
            return canvasTransform;
        }
    }

    private Dictionary<UIWndType, string> uiPathDict;//保存ui面板的类型和加载路径
    private Dictionary<UIWndType, BaseWnd> uiDict;//保存ui面板的类型和面板对象的BaseWnd组件，便于实例化
    private Stack<BaseWnd> wndStack;

    private UImanager()
    {
        ParseUIWndTypeJson();
    }

    public void PushWnd(UIWndType type)
    {
        if (wndStack == null)
        {
            wndStack = new Stack<BaseWnd>();
        }

        BaseWnd newWnd = GetWnd(type);
        //Debug.Log(uiDict.Count);
        if(type == UIWndType.Battle)
        {
            GameManager.instance.GetBattle();
        }
        newWnd.Init();
        newWnd.OnShow();
        wndStack.Push(newWnd);
    }

    public void PopWnd()
    {
        //if (wndStack == null)
        //{
        //    wndStack = new Stack<BaseWnd>();
        //}

        //if (wndStack.Count <= 0)
        //{
        //    return;
        //}

        //BaseWnd newWnd = wndStack.Pop();
        //newWnd.OnHide();
    }

    public BaseWnd GetWnd(UIWndType type)
    {
        if(uiDict==null)
        {
            uiDict = new Dictionary<UIWndType, BaseWnd>();
        }

        BaseWnd wnd;
        uiDict.TryGetValue(type, out wnd);

        if (wnd==null)
        {
            GameObject instanceWnd = GameObject.Instantiate(Resources.Load(uiPathDict[type]), CanvasTransform) as GameObject;
            uiDict.Add(type, instanceWnd.GetComponent<BaseWnd>());
        }

        return uiDict[type];
    }

    public void RefreshUIByType(UIWndType type)
    {
        if (uiDict == null)
        {
            uiDict = new Dictionary<UIWndType, BaseWnd>();
        }

        if (!uiDict.ContainsKey(type))
        {
            GameObject instanceWnd = GameObject.Instantiate(Resources.Load(uiPathDict[type]), CanvasTransform) as GameObject;
            uiDict.Add(type, instanceWnd.GetComponent<BaseWnd>());
        }
        uiDict[type].RefreshUI();
    }

    /// <summary>
    /// 解析UIWndTypeJson
    /// </summary>
    private void ParseUIWndTypeJson()
    {
        uiPathDict = new Dictionary<UIWndType, string>();
        TextAsset textAsset = Resources.Load<TextAsset>("UIWnd");
        JsonData jsonData = JsonMapper.ToObject(textAsset.text);
        foreach (JsonData item in jsonData)
        {
            string typeName = item["wndType"].ToString();
            UIWndType wndType = (UIWndType)System.Enum.Parse(typeof(UIWndType), typeName);
            string path = item["path"].ToString();
            uiPathDict.Add(wndType, path);
        }
    }
}
