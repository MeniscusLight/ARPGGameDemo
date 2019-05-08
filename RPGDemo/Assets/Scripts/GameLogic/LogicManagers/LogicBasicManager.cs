using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLogic
{
    public abstract class LogicBasicManager
    {
        private bool m_isValid = false;
        private LogicScene m_logicScene = null;

        public LogicBasicManager(LogicScene logicScene)
        {
            this.m_logicScene = logicScene;
        }

        internal void BaseInit()
        {
            m_isValid = true;
        }

        public void OnRelease()
        {
            m_logicScene = null;
            m_isValid = false;
        }

        public bool Valid
        {
            get { return m_isValid; }
        }

        public LogicScene LogicScene
        {
            get { return m_logicScene; }
        }
    }
}
