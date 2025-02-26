using UnityEngine;

public class basecon : MonoBehaviour
{
    public bool up = true;
    public bool down = true;
    public bool left = true;
    public bool right = true;
    public bool up_left = true;
    public bool up_right = true;
    public bool down_right = true;
    public bool down_left = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnValidate()
    {
        GameObject obj = transform.Find("up").gameObject;
        obj.SetActive(up);
        obj = transform.Find("down").gameObject;
        obj.SetActive(down);
        obj = transform.Find("left").gameObject;
        obj.SetActive(left);
        obj = transform.Find("right").gameObject;
        obj.SetActive(right);
        obj = transform.Find("down_right").gameObject;
        obj.SetActive(down_right);
        obj = transform.Find("down_left").gameObject;
        obj.SetActive(down_left);
        obj = transform.Find("up_left").gameObject;
        obj.SetActive(up_right);
        obj = transform.Find("up_right").gameObject;
        obj.SetActive(up_right);
    }
}
