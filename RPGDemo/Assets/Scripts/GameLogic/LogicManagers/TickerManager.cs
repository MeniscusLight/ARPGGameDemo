using System;
using System.Collections;
using System.Diagnostics;

namespace GameLogic
{
    public class TickerManager : ITickerCtrl
    {
        public static ulong s_globalTick = 0;

        private TickerGroup m_rootTickerGroup = null;        
        private Stopwatch m_stopWatch = null;

        private bool m_isNativeLogic = false;
        private bool m_isSystem = false;
        private bool m_isOnline = false;
        private bool m_isServer = false;
        private bool m_isLogic = false;

        private uint m_updateTick = 0;
        private uint m_serverLogicTick = 0;

        private long m_logicTickFrameTime = 0;

        private long m_logicTickerTimeRemain = 0;
        private long m_prevUpdateLogicTickerTime = 0;

        private uint m_logicTickFrameRate = 0;
        private uint m_viewTickFrameRate = 0;

        public TickerManager()
        {
            m_rootTickerGroup = new TickerGroup();
            m_stopWatch = new Stopwatch();
        }

        public void Init()
        {
            m_updateTick = 0;
            m_stopWatch.Start();
            m_rootTickerGroup.Init(this);
        }

        public long GetRunningTime()
        {
            if (m_stopWatch == null)
            {
                return 0;
            }
            long elapsedMilliseconds = m_stopWatch.ElapsedMilliseconds * 1000;
            return elapsedMilliseconds;
        }

        public void SetLogicFrameRate(uint logicFrameRate)
        {
            if (logicFrameRate == 0)
            {
                return;
            }
            m_logicTickFrameRate = logicFrameRate;
            m_logicTickFrameTime = 1000000 / m_logicTickFrameRate;
        }

        public void SetViewFrameRate(uint viewFrameRate)
        {
            m_viewTickFrameRate = viewFrameRate;
        }

        public static void UpdateGlobalTick()
        {
            ++s_globalTick;
        }

        public void UpdateTicker()
        {
            if (m_isSystem)
            {
                UpdateSystemTicker();
            }
            else
            {
                if (m_isServer)
                {
                    UpdateServerTick();
                }
                else
                {
                    UpdateClientTick();
                }
            }
        }

        private void UpdateSystemTicker()
        {
            if (!m_isSystem)
            {
                return;
            }
            RunTickerCtrl();
        }

        private void UpdateServerTick()
        {
            if (!m_isLogic)
            {
                return;
            }
            if (m_isNativeLogic)
            {
                UpdateNativeLogicTick();
            }
            else
            {
                UpdateLogicTick();
            }
        }

        private void UpdateClientTick()
        {
            if (m_isOnline)
            {
                UpdateOnlineTick();
            }
            else
            {
                RunTickerCtrl();
            }
        }

        private void UpdateNativeLogicTick()
        {
            long currentTime = this.GetRunningTime();
            long timeDistance = currentTime - m_prevUpdateLogicTickerTime + m_logicTickerTimeRemain;
            uint nativeUpdateCount =(uint)(timeDistance / m_logicTickFrameTime);
            m_prevUpdateLogicTickerTime = currentTime;
            m_logicTickerTimeRemain = timeDistance % m_logicTickFrameTime;

            uint nativeUpdateMaxCount = Math.Max(nativeUpdateCount, GameLogicDefs.TICKER_GROUP_UPDATE_MAX_COUNT);
            if (nativeUpdateCount > 0)
            {
                for (int index = 0; index < nativeUpdateCount; ++index)
                {
                    // update ticker tree
                    m_rootTickerGroup.UpdateGroup();
                }
            }
        }

        private void UpdateLogicTick()
        {
            ++m_serverLogicTick;
            // waiting for complete code...
        }

        private void UpdateOnlineTick()
        {
            if (m_isLogic)
            {
                if (m_isNativeLogic)
                {
                    UpdateNativeLogicTick();
                }
                else
                {
                    UpdateLogicTick();
                }
            }
            else
            {
                RunTickerCtrl();
            }
        }
        

        private void RunTickerCtrl()
        {
            ++m_updateTick;
            this.UpdateTickerCtrl();
        }

        private void UpdateTickerCtrl()
        {
            if (m_rootTickerGroup != null)
            {
                m_rootTickerGroup.UpdateGroup();
            }
        }

        public uint GetTickFrameRate()
        {
            if (m_isSystem)
            {
                return m_viewTickFrameRate;
            }
            if (!m_isOnline && !m_isServer)
            {
                return m_viewTickFrameRate;
            }
            uint frameRate = 0;
            if (m_isLogic)
            {
                frameRate = m_logicTickFrameRate;
            }
            else
            {
                frameRate = m_viewTickFrameRate;
            }
            return frameRate;
        }

        /* ---------- ITickCtrl ---------- */

        public uint TickToTime(uint tick)
        {
            uint frameRate = GetTickFrameRate();
            uint time = tick * 1000 / frameRate;
            return time;
        }

        public uint TimeToTick(uint time)
        {
            uint frameRate = GetTickFrameRate();
            uint totalTickValue = time * frameRate;
            uint tick = totalTickValue / 1000;
            if (totalTickValue % 1000 != 0)
            {
                ++tick;
            }
            return tick;
        }

        public Ticker CreateTicker(uint delayTick, uint repeatCount)
        {
            Ticker ticker = null;
            if (m_rootTickerGroup != null)
            {
                ticker = m_rootTickerGroup.CreateTicker(delayTick, repeatCount);
            }
            return ticker;
        }

        public Ticker CreateTickerWithTime(uint delayTime, uint repeatCount)
        {
            Ticker ticker = null;
            if (m_rootTickerGroup != null)
            {
                ticker = m_rootTickerGroup.CreateTickerWithTime(delayTime, repeatCount);
            }
            return ticker;
        }

        public TickerGroup CreateTickerGroup()
        {
            TickerGroup tickerGroup = null;
            if (m_rootTickerGroup != null)
            {
                tickerGroup = m_rootTickerGroup.CreateTickerGroup();
            }
            return tickerGroup;
        }

        public Ticker FindTickerWithID(uint tickerID)
        {
            Ticker ticker = null;
            if (m_rootTickerGroup != null)
            {
                ticker = m_rootTickerGroup.FindTickerWithID(tickerID);
            }
            return ticker;
        }

        public TickerGroup FindTickerGroupWithID(uint tickerGroupID)
        {
            TickerGroup tickerGroup = null;
            if (m_rootTickerGroup != null)
            {
                tickerGroup = m_rootTickerGroup.FindTickerGroupWithID(tickerGroupID);
            }
            return tickerGroup;
        }

        public void FastForward(uint forwardTickNum)
        {
            if (m_rootTickerGroup != null)
            {
                m_rootTickerGroup.FastForward(forwardTickNum);
            }
        }

        public void Start()
        {
            if (m_rootTickerGroup != null)
            {
                m_rootTickerGroup.Start();
            }
        }

        public void Stop()
        {
            if (m_rootTickerGroup != null)
            {
                m_rootTickerGroup.Stop();
            }
        }

        public void Clear()
        {
            if (m_rootTickerGroup != null)
            {
                m_rootTickerGroup.Clear();
            }
        }

        public void ReleaseTickerTaskWithID(uint tickerTaskID)
        {
            if (m_rootTickerGroup != null)
            {
                m_rootTickerGroup.ReleaseTickerTaskWithID(tickerTaskID);
            }
        }

        public void Release()
        {
            if (m_rootTickerGroup != null)
            {
                m_rootTickerGroup.OnRelease();
            }
            m_stopWatch.Stop();

            m_rootTickerGroup = null;
            m_stopWatch = null;

            m_isNativeLogic = false;
            m_isSystem = false;
            m_isOnline = false;
            m_isServer = false;
            m_isLogic = false;

            m_updateTick = 0;
            m_serverLogicTick = 0;

            m_logicTickFrameTime = 0;

            m_logicTickFrameRate = 0;
            m_viewTickFrameRate = 0;
    }

        public Func<Ticker> TickerCreater
        {
            set
            {
                if (m_rootTickerGroup != null)
                {
                    m_rootTickerGroup.TickerCreater = value;
                }
            }
        }

        public Func<TickerGroup> TickerGroupCreater
        {
            set
            {
                if (m_rootTickerGroup != null)
                {
                    m_rootTickerGroup.TickerGroupCreater = value;
                }
            }
        }

        public Action<Ticker> TickerReleaser
        {
            set
            {
                if (m_rootTickerGroup != null)
                {
                    m_rootTickerGroup.TickerReleaser = value;
                }
            }
        }

        public Action<TickerGroup> TickerGroupReleaser
        {
            set
            {
                if (m_rootTickerGroup != null)
                {
                    m_rootTickerGroup.TickerGroupReleaser = value;
                }
            }
        }

        public string Name
        {
            set
            {
                if (m_rootTickerGroup != null)
                {
                    m_rootTickerGroup.Name = value;
                }
            }
            get
            {
                string name = null;
                if (m_rootTickerGroup != null)
                {
                    name = m_rootTickerGroup.Name;
                }
                return name;
            }
        }

        public bool Pause
        {
            set
            {
                // debuger.log("setting pause state is : {0}", value);
            }
            get
            {
                bool isPause = false;
                if (m_rootTickerGroup != null)
                {
                    isPause = isPause || m_rootTickerGroup.Pause;
                }
                return isPause;
            }
        }

        public bool IsNativeLogic
        {
            set { m_isNativeLogic = value; }
        }

        public bool IsSystem
        {
            set { m_isSystem = value; }
        }

        public bool IsOnline
        {
            set { m_isOnline = value; }
        }

        public bool IsServer
        {
            set { m_isServer = value; }
        }

        public bool IsLogic
        {
            set { m_isLogic = value; }
        }

        public uint LogicTick
        {
            get { return m_updateTick; }
        }

        public uint TickFrameRate
        {
            get
            {
                uint frameRate = 0;
                if (m_rootTickerGroup != null)
                {
                    frameRate = m_rootTickerGroup.TickFrameRate;
                }
                return frameRate;
            }
        }

        public uint TickerScale
        {
            set { m_rootTickerGroup.TickerScale = value; }
            get { return m_rootTickerGroup.TickerScale; }
        }

        public uint UpdateTick
        {
            get { return m_updateTick; }
        }

    }
}