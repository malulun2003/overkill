using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.IO;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.SceneManagement;

public class SaveExit : MonoBehaviour
{
    Button button;
    GameObject editScreen;
    RectTransform con;

    public int save_num = 0;

    private const string path = "save_temp/";
    private string fname;

    void Start()
    {
        editScreen = GameObject.Find("editScreen");
        con = editScreen.transform.GetComponentsInChildren<RectTransform>(true).Where((_ => _.name == "Content")).FirstOrDefault();

        // private string path = "save_temp/";
        fname = path + save_num.ToString().PadLeft(4, '0') + ".sav";
        Debug.Log("start)" + fname);
    }

    public void OnUnLoadScene()
    {
        transform.parent.gameObject.SetActive(false);
    }

    public async void OnLoad()
    {
        Debug.Log("OnLoad)" + fname);
        if (File.Exists(fname))
        {
            RectTransform con = editScreen.transform.GetComponentsInChildren<RectTransform>(true).Where(_ => _.name == "Content").FirstOrDefault();

            //自分の子供を全て削除
            foreach (Transform child in con)
            {
                Destroy(child.gameObject);
            }

            // バイナリ形式でデシリアライズ
            BinaryFormatter bf = new BinaryFormatter();
            // 指定したパスのファイルストリームを開く
            FileStream file = File.Open(fname, FileMode.Open);
            try
            {
                // 指定したファイルストリームをオブジェクトにデシリアライズ。
                var p = (Panels)bf.Deserialize(file);
                AsyncOperationHandle<GameObject> prefabHandle = Addressables.LoadAssetAsync<GameObject>("Assets/BaseCon.prefab");
                await prefabHandle.Task;
                if (prefabHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    for (var i = 0; i < p.panels.Length; i++)
                    {
                        var pd = p.panels[i];
                        if (pd == null)
                        {
                            continue;
                        }
                        GameObject prefabInstance = Instantiate(prefabHandle.Result, con);
                        prefabInstance.GetComponent<RectTransform>().anchoredPosition = new Vector2((i % 16) * 128 + 64, -i / 16 * 128 - 64);
                        prefabInstance.name = pd.name;
                        var tmp = prefabInstance.transform.Find("title").gameObject.GetComponent<TextMeshProUGUI>();
                        tmp.text = prefabInstance.name;
                        BaseCon bc = prefabInstance.GetComponent<BaseCon>();
                        bc.next = pd.next;
                        bc.ifroute = pd.ifroute;
                        bc.transform.Find("Slider/param").GetComponent<TextMeshProUGUI>().text = pd.param.ToString();
                        bc.OnValidate();
                    }
                }
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
        Debug.Log("OnSave)" + this.name);
        Panels p = new Panels();
        // Debug.Log(p.Length);
        GameObject editScreen = GameObject.Find("editScreen");
        RectTransform con = editScreen.transform.GetComponentsInChildren<RectTransform>(true).Where((_ => _.name == "Content")).FirstOrDefault();
        var children = GetChildren(con.transform);
        // 取得した子オブジェクト名をログ出力
        for (var i = 0; i < children.Length; i++)
        {
            var rect = children[i].GetComponent<RectTransform>();
            var bc = rect.GetComponent<BaseCon>();
            // Debug.Log(children[i].name+","+rect.position+","+Mathf.FloorToInt(rect.anchoredPosition.x/128)+","+Mathf.FloorToInt(Mathf.Abs(rect.anchoredPosition.y)/128));
            // Debug.Log(bc+","+bc.down);
            var px = Mathf.FloorToInt(rect.anchoredPosition.x / 128);
            var py = Mathf.FloorToInt(Mathf.Abs(rect.anchoredPosition.y) / 128);
            var array_pos = px + py * 16;
            var panel = new PanelData();
            panel.type = OKtype.move;
            panel.next = bc.next;
            panel.ifroute = bc.ifroute;
            var tmp = children[i].transform.Find("title").gameObject.GetComponent<TextMeshProUGUI>();
            panel.name = tmp.text;
            var param = children[i].transform.Find("Slider/param").gameObject.GetComponent<TextMeshProUGUI>();
            panel.param = int.Parse(param.text);
            p.panels[px + py * 16] = panel;
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(fname);
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

        // JSON形式でデータを保存（確認用）
        DataContractJsonSerializer jf = new DataContractJsonSerializer(typeof(Panels));
        FileStream jfile = File.Create(fname + ".json");
        try
        {
            jf.WriteObject(jfile, p);
        }
        finally
        {
            if (jfile != null)
                jfile.Close();
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
