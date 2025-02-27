using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.IO;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.SceneManagement;

public class EditButton : MonoBehaviour
{
    Button button;
    GameObject editScreen;
    RectTransform con;

    public int save_num = 0;

    void Start()
    {
    }

    public void OnLoadScene()
    {
        // Debug.Log("OnLoadScene");
        // SceneManager.LoadScene("okedit", LoadSceneMode.Additive);
        Debug.Log(transform.root.name);
        transform.root.Find("okedit").gameObject.SetActive(true);
    }
}
