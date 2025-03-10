using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour
{
    public GameObject gage;
    int life = 100;

    void Start()
    {
    }

    public void OnDamage()
    {
        life--;
        UpdateGage();
    }

    public void OnCare()
    {
        life += 20;
        if (life > 100) life = 100;
        UpdateGage();
    }

    void UpdateGage() {
        var lifegage = gage.GetComponent<UnityEngine.UI.Image>();
        Debug.Log("name="+lifegage.name+", life="+life);
        // lifegage = (float)life / 100;  
    }
}
