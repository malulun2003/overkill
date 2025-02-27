using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    public void OnDir(Object button)
    {
        Debug.Log("OnDir>"+this.name);
        Debug.Log("OnDir>"+button.name);
        if (button.name == "up")
        {
            up = !up;
        }
        else if (button.name == "down")
        {
            down = !down;
        }
        else if (button.name == "left")
        {
            left = !left;
        }
        else if (button.name == "right")
        {
            right = !right;
        }
        else if (button.name == "up_left")
        {
            up_left = !up_left;
        }
        else if (button.name == "up_right")
        {
            up_right = !up_right;
        }
        else if (button.name == "down_right")
        {
            down_right = !down_right;
        }
        else if (button.name == "down_left")
        {
            down_left = !down_left;
        }
        OnValidate();
    }

    public void OnValidate()
    {
        transform.Find("up").gameObject.SetActive(up);
        transform.Find("down").gameObject.SetActive(down);
        transform.Find("left").gameObject.SetActive(left);
        transform.Find("right").gameObject.SetActive(right);
        transform.Find("down_right").gameObject.SetActive(down_right);
        transform.Find("down_left").gameObject.SetActive(down_left);
        transform.Find("up_left").gameObject.SetActive(up_left);
        transform.Find("up_right").gameObject.SetActive(up_right);
    }
}
