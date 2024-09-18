using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public Fadable panelMenu;
    public GameObject panelPass;
    public GameObject buttonSave;
    public GameObject buttonLoad;
    public Text levelName;
    public Text levelDescription;
    public GameObject pathFailCross;
    public GameObject pathSucceedCircle;
    public bool IsUIBusy => panelMenu.transform.GetChild(0).gameObject.activeSelf;
    private void Awake()
    {
        buttonSave.SetActive(Config.Instance.isEditorMode);
        buttonLoad.SetActive(Config.Instance.isEditorMode);
        levelName.text = LevelManager.Instance.curLevel.levelName;
        levelDescription.text = LevelManager.Instance.curLevel.levelDescription;
    }
    private void Update()
    {
        if (LevelManager.Instance.pass)
            return;
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            panelMenu.StartFade(!panelMenu.transform.GetChild(0).gameObject.activeSelf);
        }
    }
    public void PassCurLevel()
    {
        panelMenu.StartFade(true);
        panelPass.SetActive(true);
    }

    public void ShowSth(string sth, Vector3 screenPos)
    {
        GameObject g2 = null;
        switch (sth)
        {
            case "pathFailCross":
                g2 = pathFailCross;
                break;
            case "pathSucceedCircle":
                g2 = pathSucceedCircle;
                break;
            default:
                break;
        }
        GameObject g = Instantiate(g2, screenPos, g2.transform.rotation, transform);
        g.SetActive(true);
    }
}
