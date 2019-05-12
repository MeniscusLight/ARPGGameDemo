using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using GameLogic;
using GameView;
using GameUIContrl;
using GameCtrls;


public class GameGlobalCenter : MonoBehaviour
{
    private uint m_globalUpdateFrame = 0;
    private long m_prevGlobalUpdateTime = 0;
    private uint m_gameGlobalUpdateFrame = 0;
        
    private Stopwatch m_stopWatch = null;

    private TickerManager m_logicTickerMgrTest = null;

	void Start ()
    {
        m_stopWatch = new Stopwatch();
        m_stopWatch.Start();
                
        Application.targetFrameRate = 60;

        m_logicTickerMgrTest = new TickerManager();
        m_logicTickerMgrTest.IsLogic = true;
        m_logicTickerMgrTest.IsNativeLogic = true;
        m_logicTickerMgrTest.IsServer = true;
        m_logicTickerMgrTest.SetLogicFrameRate(GameLogicDefs.GAME_LOGIC_FRAME_RATE);
        m_logicTickerMgrTest.Init();
        m_logicTickerMgrTest.Start();
    }
	
	void Update ()
    {
        ++m_globalUpdateFrame;

        long thisTime = m_stopWatch.ElapsedMilliseconds;
        this.GameGlobalUpdateFrame(thisTime);
        this.GameLogicUpdate(thisTime);
	}

    void GameGlobalUpdateFrame(long currentTime)
    {
        if (m_prevGlobalUpdateTime == 0)
        {
            m_prevGlobalUpdateTime = currentTime;
        }
        ++m_gameGlobalUpdateFrame;
        if (currentTime - m_prevGlobalUpdateTime >= 1000)
        {
            m_prevGlobalUpdateTime = currentTime;
            GameGlobalData.GameFrameRate = m_gameGlobalUpdateFrame;
            m_gameGlobalUpdateFrame = 0;
        }

    }

    void GameLogicUpdate(long currentTime)
    {
        bool isOnline = GameGlobalData.GameTestOnline;
        if (!isOnline)
        {
            if (m_logicTickerMgrTest != null)
            {
                m_logicTickerMgrTest.UpdateTicker();
            }
        }
        else
        {
            if (m_logicTickerMgrTest != null)
            {
                m_logicTickerMgrTest.UpdateTicker();
            }
        }
        TickerManager.UpdateGlobalTick();
    }
    

    void Destroy()
    {
        m_stopWatch.Stop();
        m_logicTickerMgrTest.Stop();
        m_logicTickerMgrTest.Release();
        m_stopWatch = null;
        m_logicTickerMgrTest = null;
    }
}
