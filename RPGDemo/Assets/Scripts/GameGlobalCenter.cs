using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using GameLogic;
using GameView;
using GameUIContrl;


public class GameGlobalCenter : MonoBehaviour
{
    private ulong m_globalUpdateFrame = 0;
    private ulong m_prevGlobalUpdateTime = 0;
    private uint m_gameUpdateFrame = 0;

    private float m_frameTime = 0;
    private uint m_logicTickerTimeLeft = 0;
    private uint m_prevUpdateLogicTickerTime = 0;
    

    private Stopwatch m_stopWatch = null;

	void Start ()
    {
        m_stopWatch = new Stopwatch();
        m_stopWatch.Start();
        m_frameTime = 1000.0f / GameLogicDefs.GAME_VIEW_FRAME_RATE;

        // set game frameRate is 60
        Application.targetFrameRate = 60;
    }
	
	void Update ()
    {
        ++m_globalUpdateFrame;

        long thisTime = m_stopWatch.ElapsedMilliseconds;
        this.GameGlobalUpdate(thisTime);
        this.GameLogicUpdate(thisTime);
	}

    void GameGlobalUpdate(long currentTime)
    {
        if (m_prevGlobalUpdateTime == 0)
        {
            m_prevGlobalUpdateTime = (ulong)currentTime;
        }
        ++m_gameUpdateFrame;
        if ((ulong)currentTime - m_prevGlobalUpdateTime >= 1000)
        {
            m_prevGlobalUpdateTime = (ulong)currentTime;
            GameGlobalData.GameFrameRate = m_gameUpdateFrame;
            m_gameUpdateFrame = 0;
        }

    }

    void GameLogicUpdate(long currentTime)
    {

    }





    void Destroy()
    {
        m_stopWatch.Stop();
        m_stopWatch = null;
    }
}
