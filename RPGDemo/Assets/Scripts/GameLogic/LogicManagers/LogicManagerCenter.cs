using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLogic
{
    public class LogicManagerCenter
    {
        private TickerManager m_testTickerMgr = null;

        public LogicManagerCenter(LogicScene logicScene)
        {
            
        }

        public void Init()
        {

        }

        public void Release()
        {
            m_testTickerMgr = null;
        }

        // get all of logicManager...
    }
}
