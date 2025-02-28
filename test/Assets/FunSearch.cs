using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunSearch : MonoBehaviour
{
    public float angle = 45f;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player") //視界の範囲内の当たり判定
        {
            Debug.Log("Colider name="+other.gameObject.name);
            //視界の角度内に収まっているか
            Vector3 posDelta = other.transform.position - this.transform.position;
            float target_angle = Vector3.Angle(this.transform.forward, posDelta);

            if (target_angle < angle) //target_angleがangleに収まっているかどうか
            {
                // if(Physics.Raycast(this.transform.position, posDelta, out RaycastHit hit)) //Rayを使用してtargetに当たっているか判別
                // {
                //     if (hit.collider==other)
                //     {
                //         Debug.Log("range of view)"+target_angle);
                //     }
                // }
                Debug.Log("range of view)"+target_angle);
            }
        }
    }
}