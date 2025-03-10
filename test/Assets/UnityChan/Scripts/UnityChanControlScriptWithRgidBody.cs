//
// Mecanimのアニメーションデータが、原点で移動しない場合の Rigidbody付きコントローラ
// サンプル
// 2014/03/13 N.Kobyasahi
//
using UnityEngine;
using System.Collections;

namespace UnityChan
{
// 必要なコンポーネントの列記
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(CapsuleCollider))]
	[RequireComponent(typeof(Rigidbody))]

	public class UnityChanControlScriptWithRgidBody : MonoBehaviour
	{
		public float animSpeed = 1.5f;				// アニメーション再生速度設定
		public float lookSmoother = 3.0f;			// a smoothing setting for camera motion
		public bool useCurves = true;				// Mecanimでカーブ調整を使うか設定する
		// このスイッチが入っていないとカーブは使われない
		public float useCurvesHeight = 0.5f;		// カーブ補正の有効高さ（地面をすり抜けやすい時には大きくする）

	    public float bulletSpeed; //弾の速度
    	public GameObject bulletPrefab; //弾のPrefabを入れるための変数

		// 以下キャラクターコントローラ用パラメタ
		// 前進速度
		public float forwardSpeed = 0.5f;
		// 後退速度
		public float backwardSpeed = 0.5f;
		// 旋回速度
		public float rotateSpeed = 1.0f;
		// ジャンプ威力
		public float jumpPower = 3.0f; 
		// キャラクターコントローラ（カプセルコライダ）の参照
		private CapsuleCollider col;
		private Rigidbody rb;
		// キャラクターコントローラ（カプセルコライダ）の移動量
		private Vector3 velocity, bv;
		// CapsuleColliderで設定されているコライダのHeiht、Centerの初期値を収める変数
		private float orgColHight;
		private Vector3 orgVectColCenter;
		private Animator anim;							// キャラにアタッチされるアニメーターへの参照
		private AnimatorStateInfo currentBaseState;			// base layerで使われる、アニメーターの現在の状態の参照

		// private GameObject player;
		private GameObject cameraObject;	// メインカメラへの参照

		private float order_v = 0f;
		private float order_h= 0f;

		private float elapsedAngle = -1f;
		
		// アニメーター各ステートへの参照
		static int idleState = Animator.StringToHash ("Base Layer.Idle");
		static int locoState = Animator.StringToHash ("Base Layer.Locomotion");
		static int jumpState = Animator.StringToHash ("Base Layer.Jump");
		static int restState = Animator.StringToHash ("Base Layer.Rest");

		// 初期化
		void Start ()
		{
			// player = this.gameObject;
			// Animatorコンポーネントを取得する
			anim = GetComponent<Animator> ();
			// CapsuleColliderコンポーネントを取得する（カプセル型コリジョン）
			col = GetComponent<CapsuleCollider> ();
			rb = GetComponent<Rigidbody> ();
			//メインカメラを取得する
			cameraObject = GameObject.FindWithTag ("MainCamera");
			// CapsuleColliderコンポーネントのHeight、Centerの初期値を保存する
			orgColHight = col.height;
			orgVectColCenter = col.center;
		}
	
	
		// 以下、メイン処理.リジッドボディと絡めるので、FixedUpdate内で処理を行う.
		void FixedUpdate ()
		{
			float h = Input.GetAxis ("Horizontal");				// 入力デバイスの水平軸をhで定義
			float v = Input.GetAxis ("Vertical");				// 入力デバイスの垂直軸をvで定義
			// anim.SetFloat ("Speed", v);							// Animator側で設定している"Speed"パラメタにvを渡す
			// anim.SetFloat ("Direction", h); 						// Animator側で設定している"Direction"パラメタにhを渡す
			anim.speed = animSpeed;								// Animatorのモーション再生速度に animSpeedを設定する
			currentBaseState = anim.GetCurrentAnimatorStateInfo (0);	// 参照用のステート変数にBase Layer (0)の現在のステートを設定する
			rb.useGravity = true;//ジャンプ中に重力を切るので、それ以外は重力の影響を受けるようにする

			if (order_v != 0) {
				v = order_v;
			}
			if (order_h != 0) {
				h = order_h;
			}
			// Debug.Log(v+", 	"+h);

			// 以下、キャラクターの移動処理
			velocity = new Vector3 (0, 0, v);		// 上下のキー入力からZ軸方向の移動量を取得
			// キャラクターのローカル空間での方向に変換
			velocity = transform.TransformDirection (velocity);
			bv = velocity;
			// Debug.Log("vel="+velocity+", v="+v);
			//以下のvの閾値は、Mecanim側のトランジションと一緒に調整する
			if (v >= 0.1) {
				velocity *= forwardSpeed;		// 移動速度を掛ける
				// Debug.Log("vel2="+bv+", "+velocity+", v="+v+", "+forwardSpeed);
			} else if (v <= -0.1) {
				velocity *= backwardSpeed;	// 移動速度を掛ける
				// Debug.Log("vel2="+bv+", "+velocity+", v="+v+", "+backwardSpeed);
			}

			if (Input.GetButtonDown ("Jump")) {	// スペースキーを入力したら

				//アニメーションのステートがLocomotionの最中のみジャンプできる
				if (currentBaseState.fullPathHash == locoState) {
					//ステート遷移中でなかったらジャンプできる
					if (!anim.IsInTransition (0)) {
						rb.AddForce (Vector3.up * jumpPower, ForceMode.VelocityChange);
						// anim.SetBool ("Jump", true);		// Animatorにジャンプに切り替えるフラグを送る
					}
				}
			}


			// 上下のキー入力でキャラクターを移動させる
			transform.localPosition += velocity * Time.fixedDeltaTime;

			// 左右のキー入力でキャラクタをY軸で旋回させる
			transform.Rotate (0, h * rotateSpeed, 0);
			// 設定角度分旋回したかを計算する
			if (Mathf.Abs(elapsedAngle) > 0 && elapsedAngle != -1)
			{
				elapsedAngle -= Mathf.Abs(h * rotateSpeed);
				if (elapsedAngle < 0) {
					elapsedAngle = 0;
				}
			}

			// 以下、Animatorの各ステート中での処理
			// Locomotion中
			// 現在のベースレイヤーがlocoStateの時
			if (currentBaseState.fullPathHash == locoState) {
				//カーブでコライダ調整をしている時は、念のためにリセットする
				if (useCurves) {
					resetCollider ();
				}
			}
			// JUMP中の処理
			// 現在のベースレイヤーがjumpStateの時
			else if (currentBaseState.fullPathHash == jumpState) {
				cameraObject.SendMessage ("setCameraPositionJumpView");	// ジャンプ中のカメラに変更
				// ステートがトランジション中でない場合
				if (!anim.IsInTransition (0)) {

					// 以下、カーブ調整をする場合の処理
					if (useCurves) {
						// 以下JUMP00アニメーションについているカーブJumpHeightとGravityControl
						// JumpHeight:JUMP00でのジャンプの高さ（0〜1）
						// GravityControl:1⇒ジャンプ中（重力無効）、0⇒重力有効
						float jumpHeight = anim.GetFloat ("JumpHeight");
						float gravityControl = anim.GetFloat ("GravityControl"); 
						if (gravityControl > 0)
							rb.useGravity = false;	//ジャンプ中の重力の影響を切る

						// レイキャストをキャラクターのセンターから落とす
						Ray ray = new Ray (transform.position + Vector3.up, -Vector3.up);
						RaycastHit hitInfo = new RaycastHit ();
						// 高さが useCurvesHeight 以上ある時のみ、コライダーの高さと中心をJUMP00アニメーションについているカーブで調整する
						if (Physics.Raycast (ray, out hitInfo)) {
							if (hitInfo.distance > useCurvesHeight) {
								col.height = orgColHight - jumpHeight;			// 調整されたコライダーの高さ
								float adjCenterY = orgVectColCenter.y + jumpHeight;
								col.center = new Vector3 (0, adjCenterY, 0);	// 調整されたコライダーのセンター
							} else {
								// 閾値よりも低い時には初期値に戻す（念のため）					
								resetCollider ();
							}
						}
					}
					// Jump bool値をリセットする（ループしないようにする）				
					// anim.SetBool ("Jump", false);
				}
			}
			// IDLE中の処理
			// 現在のベースレイヤーがidleStateの時
			else if (currentBaseState.fullPathHash == idleState) {
				//カーブでコライダ調整をしている時は、念のためにリセットする
				if (useCurves) {
					resetCollider ();
				}
				// スペースキーを入力したらRest状態になる
				if (Input.GetButtonDown ("Jump")) {
					// anim.SetBool ("Rest", true);
				}
			}
			// REST中の処理
			// 現在のベースレイヤーがrestStateの時
			else if (currentBaseState.fullPathHash == restState) {
				//cameraObject.SendMessage("setCameraPositionFrontView");		// カメラを正面に切り替える
				// ステートが遷移中でない場合、Rest bool値をリセットする（ループしないようにする）
				if (!anim.IsInTransition (0)) {
					// anim.SetBool ("Rest", false);
				}
			}
		}

		// void OnGUI ()
		// {
		// 	GUI.Box (new Rect (Screen.width - 260, 10, 250, 150), "Interaction");
		// 	GUI.Label (new Rect (Screen.width - 245, 30, 250, 30), "Up/Down Arrow : Go Forwald/Go Back");
		// 	GUI.Label (new Rect (Screen.width - 245, 50, 250, 30), "Left/Right Arrow : Turn Left/Turn Right");
		// 	GUI.Label (new Rect (Screen.width - 245, 70, 250, 30), "Hit Space key while Running : Jump");
		// 	GUI.Label (new Rect (Screen.width - 245, 90, 250, 30), "Hit Spase key while Stopping : Rest");
		// 	GUI.Label (new Rect (Screen.width - 245, 110, 250, 30), "Left Control : Front Camera");
		// 	GUI.Label (new Rect (Screen.width - 245, 130, 250, 30), "Alt : LookAt Camera");
		// }


		// キャラクターのコライダーサイズのリセット関数
		void resetCollider ()
		{
			// コンポーネントのHeight、Centerの初期値を戻す
			col.height = orgColHight;
			col.center = orgVectColCenter;
		}

		private float enemy_distance = 0f;
		private float enemy_relative_angle = 0f;
		private bool enemy_flag = false;

		int target_lockon = 0;

		public (float, int) orderExec(string order, int param, GameObject[] players, GameObject player)
		{
			int res = 0;
			float elapsedTime = 0f;

			// foreach (GameObject p in players)
            // {
            //     // 自分の場合
            //     if (p == player)
            //     {
            //         continue;
            //     }
            //     // Debug.Log("2) "+p.name+" "+player.name);
	 	    //    	Vector3 targetPos = p.transform.position;
    	    // 	Vector3 playerPos = player.transform.position;
		    //     /* ターゲットとプレイヤーの距離を取得 */
        	// 	enemy_distance = Vector3.Distance(targetPos, playerPos);
            //     // Debug.Log("DIS) "+p.name+" > "+enemy_distance);
			// 	/* ターゲットとプレイヤーの相対角度を計算する */
			// 	enemy_relative_angle = Vector3.Angle(targetPos - playerPos, player.transform.forward);
			// 	// Debug.Log("ANGLE) "+p.name+" > "+enemy_relative_angle);
            // }

			enemy_distance = this.transform.Find("sight").GetComponent<FunSearch>().enemy_distance;
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
				// anim.SetBool("walk", true);

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
			else if (order == "search")
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
				// GameObject newbullet = Instantiate(bulletPrefab, this.transform.position+this.transform.up+this.transform.forward*0.8f, Quaternion.identity); //弾を生成
    		    // Rigidbody bulletRigidbody = newbullet.GetComponent<Rigidbody>();
				// bulletRigidbody.AddForce(this.transform.forward * bulletSpeed); //キャラクターが向いている方向に弾に力を加える
				// Destroy(newbullet, 10); //10秒後に弾を消す
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
