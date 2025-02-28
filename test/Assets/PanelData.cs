using UnityEngine;

public enum OKtype
{
	move,
	attack,
	detect,
    start,
    end,
    search,
}

// public enum OKdir
// {
//     up = 0,
//     down,
//     left,
//     right,
//     up_left,
//     up_right,
//     down_right,
//     down_left,
// }

[System.Serializable]
public class PanelData
{
    public OKtype type = OKtype.move;
	public string name = "";
    public int param = 0;
    public int next = 4;
    public int ifroute = -1;

    public PanelData() {
        // Debug.Log("PanelData");
        // dir = new bool[8];
        // for (var i = 0; i < 8; i++)
        // {
        //     dir[i] = false;
        // }
    }
}
