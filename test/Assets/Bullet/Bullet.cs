using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Bullet
{
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    private Collider objectCollider;
    private Rigidbody rb;

    void Start()
    {
        GetComponent<Renderer>().material.color = new Color32(0, 0, 0, 1); //弾の色を黒にする
        objectCollider = GetComponent<SphereCollider>();
        objectCollider.isTrigger = true; //Triggerとして扱う
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; //重力を無効にする
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Block")) //タグがBlockのオブジェクトと衝突した場合
        {
            Debug.Log("Block Hit");
            Destroy(this.gameObject); //弾を消す
        }

        if (collision.gameObject.CompareTag("Player")) //タグがEnemyのオブジェクトと衝突した場合
        {
            Debug.Log("Player Hit");
            // Destroy(collision.gameObject); //衝突した相手を消す
            // Destroy(this.gameObject); //弾を消す
        }
    }
}
}