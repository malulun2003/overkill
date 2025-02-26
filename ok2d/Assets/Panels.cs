using UnityEngine;

[System.Serializable]
public class Panels
{
    public PanelData[] panels;

    public Panels() {
        panels = new PanelData[256];
        for (var i = 0; i < panels.Length; i++)
        {
            panels[i] = null;
        }
    }
}
