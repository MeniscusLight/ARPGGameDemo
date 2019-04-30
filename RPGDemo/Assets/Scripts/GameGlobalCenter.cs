using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using GameLogic;
using GameView;
using GameUIContrl;


public class GameGlobalCenter : MonoBehaviour
{
    private uint m_globalUpdateFrame = 0;
    private long m_prevGlobalUpdateTime = 0;
    private uint m_gameUpdateFrame = 0;

    private float m_frameTime = 0;
    private uint m_logicTickerTimeRemain = 0;
    private long m_prevUpdateLogicTickerTime = 0;
    

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
        this.GameUpdateFrame(thisTime);
        this.GameLogicUpdate(thisTime);
	}

    void GameUpdateFrame(long currentTime)
    {
        if (m_prevGlobalUpdateTime == 0)
        {
            m_prevGlobalUpdateTime = currentTime;
        }
        ++m_gameUpdateFrame;
        if (currentTime - m_prevGlobalUpdateTime >= 1000)
        {
            m_prevGlobalUpdateTime = currentTime;
            GameGlobalData.GameFrameRate = m_gameUpdateFrame;
            m_gameUpdateFrame = 0;
        }

    }

    void GameLogicUpdate(long currentTime)
    {
        bool isOnline = false;
        if (!isOnline)
        {
            long timeDistance = currentTime - m_prevUpdateLogicTickerTime + m_logicTickerTimeRemain;
            if (timeDistance >= m_frameTime)
            {
                m_logicTickerTimeRemain = (uint)(timeDistance - m_frameTime);
                m_prevUpdateLogicTickerTime = currentTime;
                // update logic tickerMgr
            }
        }
        else
        {
            //
        }
    }
    

    void Destroy()
    {
        m_stopWatch.Stop();
        m_stopWatch = null;
    }
}
