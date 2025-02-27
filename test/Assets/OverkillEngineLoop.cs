using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace UnityChan
{
    public class OverkillEngineLoop : MonoBehaviour
    {
        private float _repeatSpan;    //繰り返す間隔
        private float _timeElapsed;   //経過時間

        private string[] order = {"shot", "search", "backward", "left", "search", "right"};
        // private string[] order = {"shot"};
        private int order_count = 0;

        private GameObject player;
        private UnityChanControlScriptWithRgidBody chan;

        public int save_num = 0;
        private const string path = "save_temp/";

        Panels p = null;

        public void OnLoadPanel()
        {
            var fname = path + save_num.ToString().PadLeft(4, '0') + ".sav";
            Debug.Log("OnLoadPanel"+this.name+", "+fname);
            if (File.Exists(fname))
            {
                // バイナリ形式でデシリアライズ
                BinaryFormatter bf = new BinaryFormatter();
                // 指定したパスのファイルストリームを開く
                FileStream file = File.Open(fname, FileMode.Open);
                try 
                {
                    // 指定したファイルストリームをオブジェクトにデシリアライズ。
                    p = (Panels)bf.Deserialize(file);
                    foreach (PanelData panel in p.panels)
                    {
                        if (panel == null)
                        {
                            continue;
                        }
                        Debug.Log("OnLoadPanel)"+panel.name);
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
            order_count = 0;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            // player = GameObject.Find("unitychan");
            player = this.gameObject;

            chan = player.GetComponent<UnityChanControlScriptWithRgidBody>();
            _repeatSpan = 0;    //実行間隔を設定(second)
            _timeElapsed = 0;   //経過時間をリセット
            // Debug.Log(player.tag);
        }

        // Update is called once per frame
        void Update()
        {
            _timeElapsed += Time.deltaTime;     //時間をカウントする

            //各プレイヤーの状態を取得する
            // players = GameObject.FindWithTag("Player");
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject p in players)
            {
                // 自分の場合
                if (p == player)
                {
                    continue;
                }
                // Debug.Log(p.name+" "+player.name);
            }
            // Debug.Log(players);

            //経過時間が繰り返す間隔を経過したら
            if (p != null && _timeElapsed >= _repeatSpan)
            {
                var pd = p.panels[order_count];
                if (pd == null)
                {
                    order_count = 0;
                    return;
                }
                _repeatSpan = chan.orderExec(pd.name, pd.param, players);
                Debug.Log("span>>"+_repeatSpan);
                // 矢印の方向決め
                if (pd.dir[(int)OKdir.up]) {
                    order_count -= 16;
                }
                else if (pd.dir[(int)OKdir.down]) {
                    order_count += 16;
                }
                else if (pd.dir[(int)OKdir.right]) {
                    order_count += 1;
                }
                else if (pd.dir[(int)OKdir.left]) {
                    order_count -= 1;
                }
                else if (pd.dir[(int)OKdir.up_right]) {
                    order_count -= 15;
                }
                else if (pd.dir[(int)OKdir.up_left]) {
                    order_count -= 17;
                }
                else if (pd.dir[(int)OKdir.down_right]) {
                    order_count += 17;
                }
                else if (pd.dir[(int)OKdir.down_left]) {
                    order_count += 15;
                }
                if (pd.name == "end")
                {
                    order_count = 0;
                }
                Debug.Log("order_count)"+order_count);
                _timeElapsed = 0;   //経過時間をリセットする
            }
        }
    }
}
