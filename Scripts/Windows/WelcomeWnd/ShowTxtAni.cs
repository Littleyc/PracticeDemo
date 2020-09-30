using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowTxtAni : MonoBehaviour
{
    public AnimationClip txtStartGameClip;
    public Text txtStartGame;

    public void ShowTxtOnTitleAnimationFinished()
    {
        txtStartGame.gameObject.SetActive(true);
        txtStartGame.GetComponent<Animator>().Play(txtStartGameClip.name);
    }

}
