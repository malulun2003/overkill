using UnityEngine;

//StateMachineBehaviourを継承
public class StateMachineBehaviourSample : StateMachineBehaviour {

    private bool act = false;

    //状態が変わった時に実行
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        // Debug.Log($"Idle_Bに変更");
        act = false;
    }

    //状態が終わる時(変わる直前)に実行
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        // Debug.Log($"Idle_B終了");
    }
    
    //毎フレーム実行(※最初と最後のフレームを除く)
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        // Debug.Log($"Idle_B再生中"+animator.gameObject.name);
        // Debug.Log("Idle_ AnimateStateInfo : " + stateInfo.normalizedTime.ToString());
        var go = animator.gameObject;
        var bulletPrefab = go.GetComponent<OverKillEngine.OverkillEngineLoop>().bulletPrefab;
        var bulletSpeed = go.GetComponent<OverKillEngine.OverkillEngineLoop>().bulletSpeed;
        if (stateInfo.normalizedTime >= 0.65f && !act) {
            GameObject newbullet = Instantiate(bulletPrefab, go.transform.position+go.transform.up*1.65f+go.transform.forward*1.5f, Quaternion.identity); //弾を生成
            Rigidbody bulletRigidbody = newbullet.GetComponent<Rigidbody>();
            bulletRigidbody.AddForce(go.transform.forward * bulletSpeed); //キャラクターが向いている方向に弾に力を加える
            Destroy(newbullet, 10); //10秒後に弾を消す
            act = true;
        }
    }
}
