using System;
using System.Collections;

namespace GameLogic
{
    public class Ticker : TickerTask
    {
        private bool m_isStart = false;
        private bool m_isPause = false;
        private bool m_runImmediately = false;
        private bool m_completeNeedRelease = false;

        private uint m_updateTick = 0;
        private uint m_repeatIndex = 0;
        private uint m_delayTick = 0;
        private uint m_repeatCount = 0;

        private ITickerCtrl m_tickerCtrl = null;

        private Action<Ticker> m_onTicker = null;
        private Action<Ticker> m_onTickerComplete = null;

        public void Init(ITickerCtrl tickerCtrl, uint delayTick, uint repeatCount)
        {
            this.Init();
            m_tickerCtrl = tickerCtrl;
            delayTick = delayTick == 0 ? 1 : delayTick;
            m_delayTick = delayTick;
            m_repeatCount = repeatCount;
        }

        public override void Start()
        {
            m_updateTick = 0;
            m_isStart = true;
            m_isPause = false;
            m_repeatIndex = 0;
            if (m_tickerCtrl != null)
            {
                this.StartTick = m_tickerCtrl.UpdateTick;
            }
        }

        public override void Stop()
        {
            m_updateTick = 0;
            m_isStart = false;
            m_isPause = false;
            m_repeatIndex = 0;
        }
        

        internal override void Update()
        {
            if (!m_isStart)
            {
                return;
            }
            if (m_isPause)
            {
                return;
            }
            if (m_updateTick == 0 && m_runImmediately)
            {
                m_runImmediately = false;
                OnRunTicker();
            }
            else
            {
                ++m_updateTick;
                uint delayTick = m_delayTick == 0 ? 1 : m_delayTick;
                if (m_updateTick % delayTick == 0)
                {
                    OnRunTicker();
                }
            }
        }

        private void OnRunTicker()
        {
            if (!this.Valid)
            {
                return;
            }
            if (!m_isStart|| !m_isPause)
            {
                return;
            }
            ++m_repeatIndex;
            OnTicker();
            if (m_repeatCount != 0)
            {
                if (m_repeatIndex == m_repeatCount)
                {
                    Stop();
                    OnTickerComplete();
                    if (m_completeNeedRelease)
                    {
                        Release();
                    }
                }
            }
        }

        private void OnTicker()
        {
            if (m_onTicker != null)
            {
                m_onTicker(this);
            }
        }

        private void OnTickerComplete()
        {
            if (m_onTickerComplete != null)
            {
                m_onTickerComplete(this);
            }
        }

        public void Release()
        {
            if (m_tickerCtrl != null)
            {
                m_tickerCtrl.ReleaseTickerTaskWithID(this.TickerID);
            }
        }

        internal override void OnRelease()
        {
            base.OnRelease();

            m_isStart = false;
            m_isPause = false;
            m_runImmediately = false;
            m_completeNeedRelease = false;

            m_updateTick = 0;
            m_repeatIndex = 0;
            m_delayTick = 0;
            m_repeatCount = 0;

            m_tickerCtrl = null;

            m_onTicker = null;
            m_onTickerComplete = null;
        }

        public bool IsStart
        {
            get { return m_isStart; }
        }

        public bool IsPause
        {
            set { m_isPause = value; }
            get { return m_isPause; }
        }

        public bool CompleteNeedRelease
        {
            set { m_completeNeedRelease = value; }
        }

        public uint RepeatCount
        {
            set { m_repeatCount = value; }
        }

        public uint DelayTick
        {
            set { m_delayTick = value; }
            get { return m_delayTick; }
        }

        public uint RepeatIndex
        {
            get { return m_repeatIndex; }
        }

        public bool RunImmediately
        {
            set { m_runImmediately = value; }
        }

        public void SetOnTickerDelegate(Action<Ticker> onTicker)
        {
            this.m_onTicker = onTicker;
        }
        public void SetOnTickerCompleteDelegate(Action<Ticker> onTickerComplete)
        {
            this.m_onTickerComplete = onTickerComplete;
        }

    }
}