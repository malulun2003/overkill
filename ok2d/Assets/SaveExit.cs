using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveExit : MonoBehaviour
{
    Button button;
    
    void Start()
    {
        // button = GetComponent<Button>();
        // button.onClick.AddListener(OnSave);
    }

    private string path = "save_temp/panel_data";

    public void OnLoad()
    {
        Debug.Log("Load hoge");
        if (File.Exists(path))
        {
            // バイナリ形式でデシリアライズ
            BinaryFormatter bf = new BinaryFormatter();
            // 指定したパスのファイルストリームを開く
            FileStream file = File.Open(path, FileMode.Open);
            try 
            {
                // 指定したファイルストリームをオブジェクトにデシリアライズ。
                PanelData p = (PanelData)bf.Deserialize(file);
                Debug.Log(p.color);
            }
            finally 
            {
                // ファイル操作には明示的な破棄が必要です。Closeを忘れないように。
                if (file != null) 
                    file.Close();
            }
        }
        else
        {
            Debug.Log("no load file");
        }
    }

    public void OnSave()
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

        PanelData p = new PanelData();
        p.name = "hoge";
        p.age = 10;;
        p.color = "red";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(path);
        try
        {
            // 指定したオブジェクトを上で作成したストリームにシリアル化する
            bf.Serialize(file, p);
        }
        finally
        {
            // ファイル操作には明示的な破棄が必要です。Closeを忘れないように。
            if (file != null) 
                file.Close();
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
