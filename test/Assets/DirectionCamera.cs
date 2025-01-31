using UnityEngine;

public class DirectionCamera : MonoBehaviour
{
    private GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("unitychan");
    }

    // Update is called once per frame
    void Update()
    {
        // transform.LookAt(player.transform);
    }
}
