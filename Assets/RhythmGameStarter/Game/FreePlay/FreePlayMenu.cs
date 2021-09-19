using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnhancedUI;
using EnhancedUI.EnhancedScroller;
using RhythmGameStarter;

public class FreePlayMenu : MonoBehaviour, IEnhancedScrollerDelegate
{
    private SmallList<SongData> m_Data;
    public EnhancedScrollerCellView g_SongViewPrefab;
    public EnhancedScroller m_Scroller;

    private void Start()
    {
        m_Scroller.Delegate = this;

        m_Data = new SmallList<SongData>();

        List<SongConfig> configs = GameData.Instance.GetSongConfigs();

        for (int i = 0; i < configs.Count; i++)
        {
            m_Data.Add(new SongData()
            {
                m_ID = configs[i].m_Id,
            });
            // Debug.Log("Song: " + configs[i].m_Name);
        }

        m_Scroller.ReloadData();
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return m_Data.Count;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return 650f;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        // first, we get a cell from the scroller by passing a prefab.
        // if the scroller finds one it can recycle it will do so, otherwise
        // it will create a new cell.
        SongPlaylist cellView = scroller.GetCellView(g_SongViewPrefab) as SongPlaylist;

        // set the name of the game object to the cell's data index.
        // this is optional, but it helps up debug the objects in 
        // the scene hierarchy.
        cellView.name = "Cell " + dataIndex.ToString();

        // in this example, we just pass the data to our cell's view which will update its UI
        cellView.SetData(m_Data[dataIndex]);

        // return the cell to the scroller
        return cellView;
    }
}

public class SongData
{
    public int m_ID;
}