using UnityEngine;
using UnityEngine.EventSystems;

public class BaseCon : MonoBehaviour
{
    int _next = 4;
    public int next
    {
        get { return _next; }
        set
        {
            if (value == _ifroute) return;
            if (_next == value) return;
            _next = value;
            OnValidate();
        }
    }

    int _ifroute = -1;
    public int ifroute
    {
        get { return _ifroute; }
        set
        {
            if (value == next) return;
            if (_ifroute == value) {
                _ifroute = -1;
            } else {
                _ifroute = value;
            }
            OnValidate();
        }
    }

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
        transform.Find("up").gameObject.SetActive(_next == 0);
        transform.Find("down").gameObject.SetActive(_next == 4);
        transform.Find("left").gameObject.SetActive(_next == 6);
        transform.Find("right").gameObject.SetActive(_next == 2);
        transform.Find("down_right").gameObject.SetActive(_next == 3);
        transform.Find("down_left").gameObject.SetActive(_next == 5);
        transform.Find("up_left").gameObject.SetActive(_next == 7);
        transform.Find("up_right").gameObject.SetActive(_next == 1);

        transform.Find("ifup").gameObject.SetActive(_ifroute == 0);
        transform.Find("ifdown").gameObject.SetActive(_ifroute == 4);
        transform.Find("ifleft").gameObject.SetActive(_ifroute == 6);
        transform.Find("ifright").gameObject.SetActive(_ifroute == 2);
        transform.Find("ifdown_right").gameObject.SetActive(_ifroute == 3);
        transform.Find("ifdown_left").gameObject.SetActive(_ifroute == 5);
        transform.Find("ifup_left").gameObject.SetActive(_ifroute == 7);
        transform.Find("ifup_right").gameObject.SetActive(_ifroute == 1);
    }
}
