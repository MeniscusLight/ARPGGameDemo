using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLogic
{
    public class SceneLogicTimerManager : LogicBasicManager
    {

        TickerManager m_logicTickerMgr = null;
        TickerManager m_viewTickerMgr = null;
        public SceneLogicTimerManager(LogicScene logicScene) : base(logicScene)
        {
            m_logicTickerMgr = new TickerManager();
            m_viewTickerMgr = new TickerManager();
        }

        public void Init()
        {
            base.BaseInit();
        }

        public void Release()
        {
            base.OnRelease();
        }

    }
}
