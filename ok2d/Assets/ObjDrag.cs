using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UI;

public class ObjDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private GameObject dragobj;
    private GameObject editScreen;

    private void Awake()
    {
        editScreen = GameObject.Find("editScreen");
    }

    // ドラッグ開始時の処理
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBegingDrag");

        // オブジェクトを複製する
        dragobj = Instantiate(gameObject, editScreen.GetComponent<RectTransform>());

        // インスタンス時になぜかアンカーが変化するため、再設定
        dragobj.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
        dragobj.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
    }

    // ドラッグ中の処理
    public void OnDrag(PointerEventData eventData)
    {
        if (dragobj != null)
        {
            dragobj.transform.position = eventData.position;
        }
        // Debug.Log("OnDrag" + eventData.position + ", " + rectTransform.anchoredPosition);
    }

    Vector2 MultipleFloorPos(Vector2 value, Vector2 multiple)
    {
        return new Vector2(Mathf.Floor(value.x / multiple.x) * multiple.x, Mathf.Floor(value.y / multiple.y) * multiple.y);
    }

    // ドラッグ終了時の処理
    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragobj != null)
        {
            // Destroy(dragobj); // 複製されたオブジェクトを破棄する
            RectTransform con = editScreen.transform.GetComponentsInChildren<RectTransform>(true).Where((_ => _.name == "Content")).FirstOrDefault();
            dragobj.transform.SetParent(con);
            var objsize = dragobj.GetComponent<RectTransform>().sizeDelta;
            dragobj.GetComponent<RectTransform>().anchoredPosition = MultipleFloorPos(dragobj.GetComponent<RectTransform>().anchoredPosition, objsize) + objsize/2;

            // editScreen サイズの調整
            if (con.sizeDelta.x < dragobj.GetComponent<RectTransform>().localPosition.x + objsize.x*2) {
                con.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, dragobj.GetComponent<RectTransform>().localPosition.x + objsize.x*2);
            }
            if (con.sizeDelta.y < Mathf.Abs(dragobj.GetComponent<RectTransform>().localPosition.y) + objsize.y*2) {
                con.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Abs(dragobj.GetComponent<RectTransform>().localPosition.y) + objsize.y*2);
            }
        }
        Debug.Log("OnEndDrag");
    }
}
