using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IfrouteClick : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if ( eventData.button == PointerEventData.InputButton.Right )
        {
            Debug.Log( "右クリック"+this.transform.parent.name+","+name);
            var basecon = this.transform.parent.gameObject.GetComponent<BaseCon>();
            if (this.name == "upb") basecon.ifroute = 0;
            if (this.name == "uprightb") basecon.ifroute = 1;
            if (this.name == "rightb") basecon.ifroute = 2;
            if (this.name == "downrightb") basecon.ifroute = 3;
            if (this.name == "downb") basecon.ifroute = 4;
            if (this.name == "downleftb") basecon.ifroute = 5;
            if (this.name == "leftb") basecon.ifroute = 6;
            if (this.name == "upleftb") basecon.ifroute = 7;
        }

        if ( eventData.button == PointerEventData.InputButton.Left )
        {
            Debug.Log( "左クリック"+this.transform.parent.name+","+name);
            var basecon = this.transform.parent.gameObject.GetComponent<BaseCon>();
            if (this.name == "upb") basecon.next = 0;
            if (this.name == "uprightb") basecon.next = 1;
            if (this.name == "rightb") basecon.next = 2;
            if (this.name == "downrightb") basecon.next = 3;
            if (this.name == "downb") basecon.next = 4;
            if (this.name == "downleftb") basecon.next = 5;
            if (this.name == "leftb") basecon.next = 6;
            if (this.name == "upleftb") basecon.next = 7;
        }
    }

    void Start()
    {
    }
}
