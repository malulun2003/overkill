using UnityEngine;

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

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            // player = GameObject.Find("unitychan");
            player = this.gameObject;

            chan = player.GetComponent<UnityChanControlScriptWithRgidBody>();
            _repeatSpan = 1;    //実行間隔を設定
            _timeElapsed = 0;   //経過時間をリセット
            Debug.Log(player.tag);
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
            if (_timeElapsed >= _repeatSpan)
            {
                // Debug.Log("Update "+order[order_count%order.Length]);
                // Debug.Log("chan="+chan);
                chan.orderExec(order[order_count%order.Length], players);
                order_count += 1;
                //ここで処理を実行
                _timeElapsed = 0;   //経過時間をリセットする
            }
        }
    }
}
