using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLogic
{
    public class SceneLogicTimerManager : LogicBasicManager
    {
        TickerManager m_logicCmdTickerMgr = null;
        TickerManager m_logicTickerMgr = null;
        TickerManager m_viewTickerMgr = null;

        public SceneLogicTimerManager(LogicScene logicScene) : base(logicScene)
        {
            m_logicCmdTickerMgr = new TickerManager();
            m_logicTickerMgr = new TickerManager();
            m_viewTickerMgr = new TickerManager();
        }

        public void Init()
        {
            base.BaseInit();
            m_logicTickerMgr.Init();
            m_logicCmdTickerMgr.Start();

            m_logicTickerMgr.Init();
            m_logicTickerMgr.Start();

            m_viewTickerMgr.Init();
            m_viewTickerMgr.Start();
        }

        public void UpdateTickMgr()
        {
            if (m_logicCmdTickerMgr != null)
            {
                m_logicCmdTickerMgr.UpdateTicker();
            }
            if (m_logicTickerMgr != null)
            {
                m_logicTickerMgr.UpdateTicker();
            }
            if (m_viewTickerMgr != null)
            {
                m_viewTickerMgr.UpdateTicker();
            }
        }



        public void Release()
        {
            base.OnRelease();
        }

        public TickerManager LogicCmdTickerMgr
        {
            get { return m_logicCmdTickerMgr; }
        }

        public TickerManager LogicTickerMgr
        {
            get { return m_logicTickerMgr; }
        }

        public TickerManager ViewTickerMgr
        {
            get { return m_viewTickerMgr; }
        }

    }
}
