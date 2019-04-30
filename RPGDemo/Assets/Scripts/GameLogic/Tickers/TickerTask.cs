using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLogic
{
    public abstract class TickerTask
    {
        private static uint s_globalTickerID = 0;

        private bool m_valid = false;
        private uint m_tickerID = 0;
        private uint m_startTick = 0;


        protected void initTask()
        {
            m_valid = true;
            m_tickerID = ++s_globalTickerID;
        }

        public void SetStartTick(uint startTick)
        {
            m_startTick = startTick;
        }

        public void OnRelease()
        {
            m_valid = false;
            m_tickerID = 0;
            m_startTick = 0;
        }


        public bool Valid
        {
            get { return m_valid; }
        }

        public uint TickerID
        {
            get { return m_tickerID; }
        }

    }
}
