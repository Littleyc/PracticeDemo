using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class Startmenu : BaseWnd,IPointerClickHandler
{
    #region 视频资源
    private RawImage rawImage;
    private VideoPlayer video;
    public VideoClip clip;
    private bool isPlayingVideo;
    private Text txtNotice;
    private bool isClickedOnce;
    #endregion

    private Image imgBG;
    private Image imgTitle;
    private Text txtStartGame;
    public AnimationClip imgTitleClip;

    public override void Init()
    {
        //视频资源处理
        rawImage = transform.GetComponentInChildren<RawImage>();
        video = rawImage.GetComponent<VideoPlayer>();
        video.clip = clip;
        video.targetTexture = (RenderTexture)rawImage.texture;
        //video.Play();
        //Debug.Log(video.clip.name);
        isPlayingVideo = true;
        isClickedOnce = false;

        txtNotice = rawImage.transform.Find("txtNotice").GetComponent<Text>();
        txtNotice.gameObject.SetActive(false);

        //登录界面处理
        imgBG = transform.Find("imgBg").GetComponent<Image>();
        imgTitle = imgBG.transform.Find("imgTitle").GetComponent<Image>();
        txtStartGame = imgBG.transform.Find("txtStartGame").GetComponent<Text>();

        imgBG.gameObject.SetActive(false);
    }

    private void Update()
    {

        //Debug.Log(video.isPlaying);
        

        if (isPlayingVideo)
        {
            if (Input.GetMouseButtonDown(0) && isClickedOnce == false)
            {
                //第一次按下时显示提示文本
                isClickedOnce = !isClickedOnce;
                //Debug.Log(isClickedOnce);
                txtNotice.gameObject.SetActive(true);

            }
            else if (Input.GetMouseButtonDown(0) && isClickedOnce == true)
            {
                //第二次按下时关闭动画
                //Debug.Log(isClickedOnce);
                isPlayingVideo = false;
                video.Stop();
                rawImage.gameObject.SetActive(false);
                //关闭时开启登录界面
                //TODO
                ShowStartWnd();
            }
        }

        //视频播放完自动关闭
        if (isPlayingVideo != video.isPlaying)
        {
            isPlayingVideo = false;
            video.Stop();
            rawImage.gameObject.SetActive(false);
            //关闭时开启登录界面
            //TODO
            ShowStartWnd();
        }
    }

    public void ShowStartWnd()
    {
        imgBG.gameObject.SetActive(true);
        txtStartGame.gameObject.SetActive(false);
        imgTitle.GetComponent<Animator>().Play(imgTitleClip.name);
    }

    public override void OnShow()
    {
        
    }

    public override void OnHide()
    {
        this.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(imgTitle.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            //当前logo变化的动画已完成，此时点击屏幕将进入下一场景；
            UImanager.Instance.PushWnd(UIWndType.MainMenu);
            OnHide();
        }
    }
}
