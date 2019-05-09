using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLogic
{
    public abstract class SceneLogicCmd
    {
        private bool m_isC2B = false;
        private uint m_logicUpdateTick = 0;
        
        internal void BaseInit(uint logicUpdateTick)
        {
            m_logicUpdateTick = logicUpdateTick;
        }
        
        public void ExecuteCmd()
        {
            if (m_isC2B)
            {
                ExecuteC2BCmd();
            }
            else
            {
                ExecuteB2CCmd();
            }
        }

        internal abstract void ExecuteC2BCmd();
        internal abstract void ExecuteB2CCmd();

        internal void Release()
        {
            m_isC2B = false;
            m_logicUpdateTick = 0;
        }

        public uint LogicUpdateTick
        {
            get { return m_logicUpdateTick; }
        }
    }
}
