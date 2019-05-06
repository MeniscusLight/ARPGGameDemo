﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLogic
{
    public class LogicScene
    {
        private LogicManagerCenter m_logicMgrCenter = null;

        public LogicScene()
        {
            m_logicMgrCenter = new LogicManagerCenter(this);
        }

        public void Init()
        {
            m_logicMgrCenter.Init();
        }

        
        public LogicManagerCenter LogicMgrCenter
        {
            get { return m_logicMgrCenter; }
        }


    }
}
