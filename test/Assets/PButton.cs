using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour {

    // ボタンが押された場合、今回呼び出される関数
    public void OnClick()
    {
        // Change the button color to GREEN
        GetComponent<Renderer>().material.color = Color.green;
    
        // Create a new button below the pressed button
        // GameObject newButton = Instantiate(gameObject, transform.position + Vector3.down * 2, Quaternion.identity);
        // newButton.GetComponent<ButtonScript>().OnButtonClick += () => Debug.Log("New button clicked!");
        Debug.Log("New button clicked!");
    }
}