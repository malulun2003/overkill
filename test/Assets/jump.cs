using UnityEngine;

//StateMachineBehaviourを継承
public class JumpStateMachine : StateMachineBehaviour {

    private bool act = false;

    //状態が変わった時に実行
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        act = false;
    }

    //状態が終わる時(変わる直前)に実行
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
    }
    
    //毎フレーム実行(※最初と最後のフレームを除く)
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        var go = animator.gameObject;
        if (stateInfo.normalizedTime >= 0.2f && !act) {
            // 乱数で飛ぶ方向を変更する
            if (Random.Range(0, 2) == 0) {
                go.GetComponent<Rigidbody>().AddForce(go.transform.right * 1.8f, ForceMode.Impulse);
            } else {
                go.GetComponent<Rigidbody>().AddForce(-go.transform.right * 1.8f, ForceMode.Impulse);
            }
            // GameObject newbullet = Instantiate(bulletPrefab, go.transform.position+go.transform.up*1.65f+go.transform.forward*1.5f, Quaternion.identity); //弾を生成
            // newbullet.transform.rotation = go.transform.rotation;
            // Rigidbody bulletRigidbody = newbullet.GetComponent<Rigidbody>();
            // bulletRigidbody.AddForce(go.transform.forward * bulletSpeed); //キャラクターが向いている方向に弾に力を加える
            // Destroy(newbullet, 10); //10秒後に弾を消す
            act = true;
        }
    }
}
