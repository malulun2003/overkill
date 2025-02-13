using UnityEngine;

public class baecon : MonoBehaviour
{
    public bool up = true;
    public bool down = true;
    public bool left = true;
    public bool right = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnValidate()
    {
        GameObject obj = transform.Find("up").gameObject;
        obj.SetActive(up);
        obj = transform.Find("down").gameObject;
        obj.SetActive(down);
        obj = transform.Find("left").gameObject;
        obj.SetActive(left);
        obj = transform.Find("right").gameObject;
        obj.SetActive(right);
    }
}
