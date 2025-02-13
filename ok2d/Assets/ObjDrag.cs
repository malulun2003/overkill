using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UI;

public class ObjDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Vector2 prevPos; //保存しておく初期position
    private RectTransform rectTransform; // 移動したいオブジェクトのRectTransform
    private RectTransform parentRectTransform; // 移動したいオブジェクトの親(Panel)のRectTransform
    private GameObject dragobj;
    private GameObject parent;
    private GameObject editScreen;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentRectTransform = rectTransform.parent as RectTransform;
        parent = transform.parent.gameObject;
        editScreen = GameObject.Find("editScreen");
    }

    // ドラッグ開始時の処理
    public void OnBeginDrag(PointerEventData eventData)
    {
        // ドラッグ前の位置を記憶しておく
        // RectTransformの場合はpositionではなくanchoredPositionを使う
        // prevPos = rectTransform.anchoredPosition;
        // Debug.Log("OnBegingDrag");

        // オブジェクトを複製する
        dragobj = Instantiate(gameObject, editScreen.GetComponent<RectTransform>()/* parentRectTransform*/);
        // dragobj = Instantiate(gameObject, parentRectTransform);
        dragobj.GetComponent<ObjDrag>().enabled = false; // 複製されたオブジェクトのドラッグハンドリングを無効にする
    }

    // ドラッグ中の処理
    public void OnDrag(PointerEventData eventData)
    {
        if (dragobj != null)
        {
            Vector2 localPosition = GetLocalPosition(eventData.position);
            dragobj.transform.position = eventData.position;
        }
        // Debug.Log("OnDrag" + eventData.position + ", " + rectTransform.anchoredPosition);
    }

    // ドラッグ終了時の処理
    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragobj != null)
        {
            // Destroy(dragobj); // 複製されたオブジェクトを破棄する
            RectTransform con = editScreen.transform.GetComponentsInChildren<RectTransform>(true).Where((_ => _.name == "Content")).FirstOrDefault();
            dragobj.transform.parent = con;
            Vector2 p = new Vector2(
                eventData.position.x - editScreen.GetComponent<RectTransform>().position.x,
                editScreen.GetComponent<RectTransform>().position.y - eventData.position.y);
            Vector2 p2 = new Vector2(Mathf.Floor(p.x/128), Mathf.Floor(p.y/128));
            Vector3 pos = dragobj.transform.position;
            pos.x = p2.x*128+64/*+editScreen.transform.position.x*/;
            pos.y = -(p2.y*128)-64/*+editScreen.transform.position.y*/;
            Debug.Log(pos+"==="+editScreen.transform.position);
            dragobj.transform.position = pos + editScreen.transform.position;  // 座標を設定

            // Debug.Log(con);
            


            // editScreen サイズの調整
            // Debug.Log("OnEndDrag>> "+editScreen.transform);
            LayoutElement layoutElement = editScreen.transform.GetComponentsInChildren<LayoutElement>(true)[0];
            Debug.Log(layoutElement.preferredWidth+"<-->"+pos.x+64);
            if (layoutElement.preferredWidth < dragobj.transform.position.x+64) {
                layoutElement.preferredWidth = dragobj.transform.position.x+64;
                Debug.Log(layoutElement.preferredWidth+", "+pos.x);
            }
            // layoutElement.Preferred Width
            // var match = allTransform.Where((_ => _.name == "Content"));
            // layoutElement = match.FirstOrDefault().transform.GetComponent<LayoutElement>();
            // Transform con = editScreen.transform.Find("Content");
            // GameObject con = editScreen.GetComponentsInChildren<Text>();
            // editScreen.GetComponentInChild<Text>();
            // editScreen.transform.Find("Scroll View").GetComponent<ScrollRect>();
            // con.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1024);
            // con.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1024);
            // if (pos.y + 64 > editScreen.transform.size.x) {
            //     Debug.Log("OnEndDrag");
            // }
        }
        Debug.Log("OnEndDrag");
    }

    // ScreenPositionからlocalPositionへの変換関数
    private Vector2 GetLocalPosition(Vector2 screenPosition)
    {
        Vector2 result = Vector2.zero;

        // screenPositionを親の座標系(parentRectTransform)に対応するよう変換する.
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, screenPosition, Camera.main, out result);

        return result;
    }
}
