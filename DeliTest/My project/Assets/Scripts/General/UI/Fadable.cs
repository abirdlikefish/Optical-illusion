using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
//显现消失UI
//需要第0个子物体是背景、第1个子物体是内容
public class Fadable : MonoBehaviour
{
    public Image panelBack => transform.GetChild(0).GetComponent<Image>();
    public GameObject panelContent => transform.GetChild(1).gameObject;
    [HelpBox("第0个子物体是背景、第1个子物体是内容", HelpBoxType.Warning/*enum selection*/, 0/*shown order*/)]
    //淡入时长
    public float inDuration = 0.3f;
    //淡出时长
    public float outDuration = 3f;
    bool isFading = false;
    public void StartFade(bool isIn, Action callbackBefore = null, Action callbackAfter = null)
    {
        if (isFading && isIn)
            return;
        StopFade();
        StartCoroutine(Fade(isIn, callbackBefore, callbackAfter));
    }
    public void StopFade()
    {
        StopAllCoroutines();
    }
    IEnumerator Fade(bool isIn, Action callbackBefore = null,Action callbackAfter = null)
    {
        isFading = true;
        panelBack.gameObject.SetActive(true);
        if (!isIn)
        {
            panelContent.SetActive(false);
        }
        callbackBefore?.Invoke();
        float time,timer;
        time = timer = isIn ? inDuration : outDuration;
        while (timer > 0)
        {
            panelBack.color = new Color(panelBack.color.r, panelBack.color.g, panelBack.color.b, (isIn ? (time - timer) : timer) / time);
            timer -= Time.deltaTime;
            yield return null;
        }
        if (!isIn)
        {
            panelBack.gameObject.SetActive(false);
        }
        if (isIn)
        {
            panelContent.SetActive(true);
        }
        callbackAfter?.Invoke();
        isFading = false;
        yield break;
    }
}
