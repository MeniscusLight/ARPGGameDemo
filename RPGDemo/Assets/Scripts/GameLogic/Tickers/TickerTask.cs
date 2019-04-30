using System;

namespace GameLogic
{
    public abstract class TickerTask
    {
        private static uint s_globalTickerID = 0;

        private bool m_valid = false;
        private uint m_tickerID = 0;
        private uint m_startTick = 0;

        internal void Init()
        {
            m_valid = true;
            ++s_globalTickerID;
            m_tickerID = s_globalTickerID;
        }

        internal void WaitForRelease()
        {
            m_valid = false;
        }

        internal virtual void OnRelease()
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

        public uint StartTick
        {
            set { m_startTick = value; }
            get { return m_startTick; }
        }

        abstract public void Start();
        abstract public void Stop();
        abstract internal void Update();

    }
}
