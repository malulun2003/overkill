using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;

namespace OverKillEngine
{
    public class OverkillEngineLoop : MonoBehaviour
    {

        private float _repeatSpan;    //繰り返す間隔
        private float _timeElapsed;   //経過時間
        private int order_count = 0;

        private float life = 1f;
        private float heat = 0f;

        private GameObject player;
        // private UnityChanControlScriptWithRgidBody chan;

        public int save_num = 1;
        private const string path = "save_temp/";

        public GameObject hitPrefab;
        public GameObject deadPrefab;
        public GameObject bulletPrefab;
        public float bulletSpeed;

        Panels p = null;

        // int target_lockon = 0;
        // private float _interval;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "bullet" && this.life > 0f)
            {
                // Debug.Log("BulletLanding!!!"+gameObject.name+", "+other.name);
                Destroy(other.gameObject);
                // 爆発エフェクト
                GameObject explosion =Instantiate(hitPrefab, other.gameObject.transform.position, Quaternion.identity);
                Destroy(explosion, 2.0f);

                this.life -= 0.5f;
                this.heat += 0.1f;
                // Debug.Log("life="+this.life);
                UpdateGage(life, heat);
                if (this.life <= 0f)
                {
                    anim.SetTrigger("dead_t");
                    Destroy(this.gameObject, 2f);
                    // Invoke(nameof(DeadMethod), 2.5f);
                    Instantiate(deadPrefab, gameObject.transform.position, Quaternion.identity);
                }
            }
        }

        // void DeadMethod()
        // {
        //     Debug.Log("Delay call");
        //     Instantiate(deadPrefab, gameObject.transform.position, Quaternion.identity);
        // }

        public GameObject gage;

        private void UpdateGage(float life, float heat)
        {
            var lifegage = gage.transform.Find("damage");
            // var lifegage = gage.GetComponent<UnityEngine.UI.Image>();
            // Debug.Log("lifegage="+lifegage.GetComponent<UnityEngine.UI.Image>().rectTransform.sizeDelta);
            // Debug.Log("name="+lifegage.name+", life="+life);
            lifegage.GetComponent<UnityEngine.UI.Image>().rectTransform.sizeDelta = new Vector2((int)(life*100), 20);
        }

        public void OnLoadPanel()
        {            
            var fname = path + save_num.ToString().PadLeft(4, '0') + ".sav";
            // Debug.Log("OnLoadPanel"+this.name+", "+fname+", num="+save_num);            
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
                        // Debug.Log("OnLoadPanel)"+panel.name);
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

            // chan = player.GetComponent<UnityChanControlScriptWithRgidBody>();
            _repeatSpan = 0;    //実行間隔を設定(second)
            _timeElapsed = 0;   //経過時間をリセット
            // Debug.Log(player.tag);

            // SphereColider の　Radius を変える
            // this.transform.Find("sight").GetComponent<SphereCollider>().radius = 4f;

            anim = GetComponent<Animator>();
            // Debug.Log("anim="+anim.name);
            // anim.SetBool("walk", true);

            gage = GameObject.Find("LifeGage"+gameObject.name.Substring(gameObject.name.Length - 1));
            UpdateGage(life, heat);
        }

		// 前進速度
		public float forwardSpeed = 0.5f;
		// 後退速度
		public float backwardSpeed = 0.5f;
        // 旋回速度
		public float rotateSpeed = 1.0f;

        private Vector3 velocity;
        private float order_v = 0;
        private float order_h = 0;
        private float elapsedAngle = -1f;
        private bool caution = false;

        public void Caution()
        {
            Debug.Log("Caution");
            caution = true;
        }

        void FixedUpdate()
        {
            float v = 0f;
            float h = 0f;

            if (order_v != 0) {
                v = order_v;
            }
            if (order_h!= 0) {
                h = order_h;
            }
            velocity = transform.TransformDirection (new Vector3 (0, 0, v));
            if (v > 0) {
                velocity *= forwardSpeed;
            } else if (v < 0) {
                velocity *= backwardSpeed;
            }
            transform.localPosition += velocity * Time.fixedDeltaTime;
            transform.Rotate (0, h * rotateSpeed, 0);
            // 設定角度分旋回したかを計算する
			if (Mathf.Abs(elapsedAngle) > 0 && elapsedAngle != -1)
			{
				elapsedAngle -= Mathf.Abs(h * rotateSpeed);
				if (elapsedAngle < 0) {
					elapsedAngle = 0;
				}
			}
        }

        // Update is called once per frame
        void Update()
        {
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
                // Debug.Log(player.name+"> order>>"+pd.name+", "+pd.param);
                (_repeatSpan, ifcon) = orderExec(pd.name, pd.param, players, own);
                // Debug.Log(player.name+"> span>>"+_repeatSpan+", if="+ifcon+", ("+pd.next+", "+pd.ifroute);
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
            caution = false;
        }

		private float enemy_relative_angle = 0f;
		private bool enemy_flag = false;
		int target_lockon = 0;

        public (float, int) orderExec(string order, int param, GameObject[] players, GameObject player)
		{
			int res = 0;
			float elapsedTime = 0f;

			// enemy_distance = this.transform.Find("sight").GetComponent<FunSearch>().enemy_distance;
			enemy_relative_angle = this.transform.Find("sight").GetComponent<FunSearch>().enemy_angle;
			enemy_flag = this.transform.Find("sight").GetComponent<FunSearch>().found;

			if (!this.transform.Find("sight").GetComponent<FunSearch>().found) {
				target_lockon = 0;
			}

			// Debug.Log(order+", "+param);
			if (order == "start")
			{
				elapsedTime = 0f;
			}
			else if (order == "end")
			{
				elapsedTime = 0f;
			}
			else if (order == "forward" || order == "move")
			{
				if (param < 0)
				{
					order_v = -1;
					anim.SetTrigger("back_t");
				} else
				{
					order_v = 1;
					anim.SetTrigger("forward_t");
				}
				order_h = 0;
				elapsedTime = Mathf.Abs(param);
			}
			else if (order == "rotation")
			{
				if (param < 0)
				{
					order_h = -1;
				} else
				{
					order_h = 1;
				}
				anim.SetTrigger("rot_t");
				order_v = 0;
				elapsedTime = Mathf.Abs(param);
			}
            else if (order == "avoidance")
            {
                Debug.Log("jump!!!!!!!!!!!!!!!!!!!");
                anim.SetTrigger("jump");
                // this.GetComponent<Rigidbody>().AddForce(transform.right * 20, ForceMode.Impulse);
                elapsedTime = 2f;
            }
			else if (order == "rot_enemy")
			{
				// Debug.Log("rot_enemy) dist="+enemy_distance+", angle="+enemy_relative_angle+", "+target_lockon+", "+elapsedAngle);
				if (target_lockon == 1 && elapsedAngle < 0)
				{
					// ターゲットの方向に回転する
					// Debug.Log("rot_enemy2) dist="+enemy_distance+", angle="+enemy_relative_angle);
					if (enemy_relative_angle < 0) {
						order_h = -1;
					} else {
						order_h = 1;
					}
					elapsedTime = 0f;
					elapsedAngle = Mathf.Abs(enemy_relative_angle);
					res = -1;
				} else if (target_lockon == 0) {
					res = 0;
				} else if (elapsedAngle > 0) {
					res = -1;
				} else if (elapsedAngle == 0) {
					res = 0;
					elapsedAngle = -1f;
				}
				anim.SetTrigger("rot_t");
			}
			else if (order == "s_enemy")
			{
				// Debug.Log("search) dist="+enemy_distance+", angle="+enemy_relative_angle+", "+enemy_flag);
				if (enemy_flag) {
					elapsedTime = 0f;
					res = 1;
				} else {
					target_lockon = 0;
				}
				order_v = 0;
				order_h = 0;
				// Debug.Log("search found >"+this.transform.Find("sight").GetComponent<FunSearch>().found);
			}
			else if (order == "s_bullet")
			{
                // 弾が飛来中！
                if (caution) {
                    res = 1;
                    Debug.Log("LLLLLLLLLLLLLLLLLLLLLLLLL");
                }
			}
			else if (order == "rockon")
			{
				if (enemy_flag) {
					target_lockon = 1;
					Debug.Log("target_lockon >"+target_lockon);
				}
				elapsedTime = 0f;
				order_v = 0;
				order_h = 0;
			}
			else if (order == "shot")
			{
				elapsedTime = 1f;
				order_v = 0;
				order_h = 0;
				anim.SetTrigger("shoot_t");
			}
			else if (order == "rand")
			{
				elapsedTime = 0f;
				res = 0;
				if (Random.Range(0, 2) == 0) {
					res = 1;
				}
			}
			
			return (elapsedTime, res);
		}

    }
}
