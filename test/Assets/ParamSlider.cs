using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ParamSlider : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnSlider(Slider slider)
    {
        Debug.Log("OnSlider>"+this.name+", "+slider.value);
        // Debug.Log("OnSlider>"+param.name);
        slider.transform.Find("param").GetComponent<TextMeshProUGUI>().text = slider.value.ToString();
        // var param = children[i].transform.Find("param").gameObject.GetComponent<TextMeshProUGUI>();
    }
}
