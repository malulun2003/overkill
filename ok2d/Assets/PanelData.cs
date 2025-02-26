using UnityEngine;

public enum OKtype
{
	move,
	attach,
	detect,
}

public enum OKdir
{
    up = 0,
    down,
    left,
    right,
    up_left,
    up_right,
    down_right,
    down_left,
}

[System.Serializable]
public class PanelData
{
    public OKtype type;
	public string name;
    public int pram;
    public bool[] dir;

    public PanelData() {
        Debug.Log("PanelData");
        dir = new bool[8];
        for (var i = 0; i < 8; i++)
        {
            dir[i] = false;
        }
    }
}
