using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class ButtonController : MonoBehaviour
{
    Button button;
    
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Save);
    }

    void Save()
    {
        GameObject editScreen = GameObject.Find("editScreen");
        RectTransform con = editScreen.transform.GetComponentsInChildren<RectTransform>(true).Where((_ => _.name == "Content")).FirstOrDefault();
        var children = GetChildren(con.transform);
        // 取得した子オブジェクト名をログ出力
        for (var i = 0; i < children.Length; i++)
        {
            var rect = children[i].GetComponent<RectTransform>();
            Debug.Log(children[i].name+","+rect.position);
        }
    }

    // parent直下の子オブジェクトをforループで取得する
    private static Transform[] GetChildren(Transform parent)
    {
        var children = new Transform[parent.childCount];
        for (var i = 0; i < children.Length; ++i)
        {
            children[i] = parent.GetChild(i);
        }
        return children;
    }
}
