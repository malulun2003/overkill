using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunSearch : MonoBehaviour
{
    public float angle = 90f;
    public bool found = false;
    public float enemy_distance = 0f;
    public float enemy_angle = 0f;
  
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player") //視界の範囲内の当たり判定
        {
            // Debug.Log("Colider name="+other.gameObject.name);
            //視界の角度内に収まっているか
            Vector3 posDelta = other.transform.position - this.transform.position;
            float target_angle = Vector3.SignedAngle(this.transform.forward, posDelta, Vector3.up);
            // Debug.Log("target_angle="+target_angle+", "+angle);

            if (Mathf.Abs(target_angle) < angle/2) //target_angleがangleに収まっているかどうか
            {
                // if(Physics.Raycast(this.transform.position, posDelta, out RaycastHit hit)) //Rayを使用してtargetに当たっているか判別
                // {
                //     if (hit.collider==other)
                //     {
                //         Debug.Log("range of view)"+target_angle);
                //     }
                // }
                // Debug.Log("range of view)"+target_angle);

                // if (!found)
                // {
                //     Debug.Log("sight angle enter");
                // }
                enemy_distance = posDelta.magnitude;
                enemy_angle = target_angle;
                Debug.Log("enemy_angle="+enemy_angle+", enemy_distance="+enemy_distance);
                found = true;
            } else {
                // if (found)
                // {
                //     Debug.Log("sight angle exit");
                // }
                found = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        found = false;
        // Debug.Log("sight dis exit");
    }
}
