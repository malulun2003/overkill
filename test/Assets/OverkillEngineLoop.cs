using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;

namespace UnityChan
{
    public class OverkillEngineLoop : MonoBehaviour
    {
        private float _repeatSpan;    //繰り返す間隔
        private float _timeElapsed;   //経過時間
        private int order_count = 0;

        private float life = 100f;
        private float heat = 0f;

        private GameObject player;
        private UnityChanControlScriptWithRgidBody chan;

        public int save_num = 0;
        private const string path = "save_temp/";

        [SerializeField] GameObject explosionPrefab;

        Panels p = null;

        int target_lockon = 0;
        private float _interval;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "bullet")
            {
                Debug.Log("BulletLanding!!!"+gameObject.name+", "+other.name);
                Destroy(other.gameObject);
                // 爆発エフェクト
                GameObject explosion =Instantiate(explosionPrefab, other.gameObject.transform.position, Quaternion.identity);
                Destroy(explosion, 2.0f);

                this.life -= 10f;
                this.heat += 10f;
                Debug.Log("life="+this.life);
                if (this.life <= 0f)
                {
                    Destroy(this.gameObject);
                }
            }
        }

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

        private Animator anim = null;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            // player = GameObject.Find("unitychan");
            player = this.gameObject;

            chan = player.GetComponent<UnityChanControlScriptWithRgidBody>();
            _repeatSpan = 0;    //実行間隔を設定(second)
            _timeElapsed = 0;   //経過時間をリセット
            // Debug.Log(player.tag);

            // SphereColider の　Radius を変える
            this.transform.Find("sight").GetComponent<SphereCollider>().radius = 4f;

            anim = GetComponent<Animator>();
            Debug.Log("anim="+anim.name);
        }

        // Update is called once per frame
        void Update()
        {
            anim.SetBool("start", true);
            anim.SetBool("start2", true);
            _timeElapsed += Time.deltaTime;     //時間をカウントする

            //各プレイヤーの状態を取得する
            // players = GameObject.FindWithTag("Player");
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            var own = this.gameObject;

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
                int ifcon;
                Debug.Log(player.name+"> order>>"+pd.name+", "+pd.param);
                (_repeatSpan, ifcon) = chan.orderExec(pd.name, pd.param, players, own);
                Debug.Log(player.name+"> span>>"+_repeatSpan+", if="+ifcon+", ("+pd.next+", "+pd.ifroute);
                // 矢印の方向決め
                if (ifcon == 0) {
                    if (pd.next == 0) {
                        order_count -= 16;
                    }
                    else if (pd.next == 1) {
                        order_count -= 15;
                    }
                    else if (pd.next == 2) {
                        order_count += 1;
                    }
                    else if (pd.next == 3) {
                        order_count += 17;
                    }
                    else if (pd.next == 4) {
                        order_count += 16;
                    }
                    else if (pd.next == 5) {
                        order_count += 15;
                    }
                    else if (pd.next == 6) {
                        order_count -= 1;
                    }
                    else if (pd.next == 7) {
                        order_count -= 17;
                    }
                } else if (ifcon == 1) {
                    if (pd.ifroute == 0) {
                        order_count -= 16;
                    }
                    else if (pd.ifroute == 1) {
                        order_count -= 15;
                    }
                    else if (pd.ifroute == 2) {
                        order_count += 1;
                    }
                    else if (pd.ifroute == 3) {
                        order_count += 17;
                    }
                    else if (pd.ifroute == 4) {
                        order_count += 16;
                    }
                    else if (pd.ifroute == 5) {
                        order_count += 15;
                    }
                    else if (pd.ifroute == 6) {
                        order_count -= 1;
                    }
                    else if (pd.ifroute == 7) {
                        order_count -= 17;
                    }
                }
                if (pd.name == "end")
                {
                    order_count = 0;
                }
                // Debug.Log("order_count)"+order_count);
                _timeElapsed = 0;   //経過時間をリセットする
                

            }
        }
    }
}
