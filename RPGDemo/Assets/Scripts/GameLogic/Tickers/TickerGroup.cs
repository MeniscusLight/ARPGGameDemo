using System;
using System.Collections.Generic;
using System.Collections;

namespace GameLogic
{
    public class TickerGroup : TickerTask, ITickerCtrl
    {
        private string m_tickerGroupName = null;
        
        private bool m_isStart = false;
        private bool m_isPause = false;
        private bool m_notScaleFrame = false;

        private uint m_tickerScale = GameLogicDefs.GAME_TICKER_FULL_SCALE;
        private uint m_tickerProgress = 0;
        private uint m_updateTick = 0;

        private Func<Ticker> m_tickerCreater = null;
        private Func<TickerGroup> m_tickerGroupCreater = null;
        private Action<Ticker> m_tickerReleaser = null;
        private Action<TickerGroup> m_tickerGroupReleaser = null;

        private IList<TickerTask> m_tickerTaskList = null;
        private IList<TickerTask> m_tickerTaskAssistList = null;

        private ITickerCtrl m_rootTickerCtrl = null;

        public TickerGroup()
        {
            m_tickerTaskList = new List<TickerTask>();
            m_tickerTaskAssistList = new List<TickerTask>();
        }

        public void Init(ITickerCtrl tickerCtrl)
        {
            this.Init();
            m_rootTickerCtrl = tickerCtrl;
            m_tickerScale = GameLogicDefs.GAME_TICKER_FULL_SCALE;
            m_tickerProgress = 0;
        }

        private TickerTask FindTickerTaskWithID(uint tickerTaskID)
        {
            if (tickerTaskID == 0)
            {
                return null;
            }
            if (m_tickerTaskList == null || m_tickerTaskAssistList == null)
            {
                return null;
            }
            TickerTask targetTickerTask = null;
            TickerTask tickerTask = null;
            for (int index = m_tickerTaskList.Count - 1; index >= 0; --index)
            {
                tickerTask = m_tickerTaskList[index];
                if (tickerTask == null)
                {
                    continue;
                }
                if (tickerTask.TickerID == tickerTaskID)
                {
                    targetTickerTask = tickerTask;
                    break;
                }
            }
            if (targetTickerTask == null)
            {
                for (int index = m_tickerTaskAssistList.Count - 1; index >= 0; --index)
                {
                    tickerTask = m_tickerTaskAssistList[index];
                    if (tickerTask == null)
                    {
                        continue;
                    }
                    if (tickerTask.TickerID == tickerTaskID)
                    {
                        targetTickerTask = tickerTask;
                        break;
                    }
                }
            }
            return targetTickerTask;
        }

        public void UpdateGroup()
        {
            if (!this.Valid)
            {
                return;
            }
            if (!m_isStart)
            {
                return;
            }
            if (m_notScaleFrame)
            {
                Update();
            }
            else
            {
                uint currentTickerProgress = m_tickerProgress + m_tickerScale;
                uint updateCount = currentTickerProgress / GameLogicDefs.GAME_TICKER_FULL_SCALE;
                m_tickerProgress = currentTickerProgress % GameLogicDefs.GAME_TICKER_FULL_SCALE;
                updateCount = Math.Max(updateCount, GameLogicDefs.TICKER_GROUP_UPDATE_MAX_COUNT);
                for (int index = 0; index < updateCount; ++index)
                {
                    Update();
                }
            }
        }

        private void ReleaseTickerTask(TickerTask tickerTask)
        {
            if (tickerTask == null)
            {
                return;
            }
            tickerTask.OnRelease();
            if (tickerTask is Ticker)
            {
                if (m_tickerReleaser != null)
                {
                    m_tickerReleaser(tickerTask as Ticker);
                }
            }
            if (tickerTask is TickerGroup)
            {
                if (m_tickerGroupReleaser != null)
                {
                    m_tickerGroupReleaser(tickerTask as TickerGroup);
                }
            }
        }

        internal override void Update()
        {
            if (!this.Valid)
            {
                return;
            }
            if (!m_isStart)
            {
                return;
            }
            if (m_tickerTaskList == null || m_tickerTaskAssistList == null)
            {
                return;
            }
            TickerTask tickerTask = null;
            for (int index = 0; index < m_tickerTaskList.Count; ++index)
            {
                tickerTask = m_tickerTaskList[index];
                if (tickerTask == null)
                {
                    continue;
                }
                if (tickerTask.Valid)
                {
                    m_tickerTaskAssistList.Add(tickerTask);
                    if (tickerTask.StartTick < m_updateTick || m_updateTick == 0)
                    {
                        tickerTask.Update();
                    }
                }
                else
                {
                    this.ReleaseTickerTask(tickerTask);
                }
            }
        }

        public override void Start()
        {
            m_isStart = true;
            if (m_rootTickerCtrl != null)
            {
                this.StartTick = m_rootTickerCtrl.UpdateTick;
            }
        }

        public override void Stop()
        {
            m_isStart = false;
        }

        internal override void OnRelease()
        {
            base.OnRelease();
            
        }

        /* ----- ITickerCtrl ----- */
        public Ticker CreateTicker(uint delayTick, uint repeatCount)
        {
            Ticker ticker = null;
            if (m_tickerCreater != null)
            {
                ticker = m_tickerCreater();
            }
            if (ticker == null)
            {
                ticker = new Ticker();
            }
            ticker.Init(this, delayTick, repeatCount);
            m_tickerTaskList.Add(ticker);
            return ticker;
        }

        public Ticker CreateTickerWithTime(uint delayTime, uint repeatCount)
        {
            uint delayTick = this.TimeToTick(delayTime);
            Ticker ticker = this.CreateTicker(delayTick, repeatCount);
            return ticker;
        }

        public TickerGroup CreateTickerGroup()
        {
            TickerGroup tickerGroup = null;
            if (m_tickerGroupCreater != null)
            {
                tickerGroup = m_tickerGroupCreater();
            }
            if (tickerGroup == null)
            {
                tickerGroup = new TickerGroup();
            }
            tickerGroup.Init(this);
            tickerGroup.m_tickerCreater = m_tickerCreater;
            tickerGroup.m_tickerGroupCreater = m_tickerGroupCreater;
            tickerGroup.m_tickerReleaser = m_tickerReleaser;
            tickerGroup.m_tickerGroupReleaser = m_tickerGroupReleaser;
            m_tickerTaskList.Add(tickerGroup);
            return tickerGroup;
        }

        public Ticker FindTickerWithID(uint tickerTaskID)
        {
            TickerTask tickerTask = FindTickerTaskWithID(tickerTaskID);
            Ticker targetTicker = tickerTask as Ticker;
            return targetTicker;
        }

        public TickerGroup FindTickerGroupWithID(uint tickerTaskID)
        {
            TickerTask tickerTask = FindTickerTaskWithID(tickerTaskID);
            TickerGroup targetTickerGroup = tickerTask as TickerGroup;
            return targetTickerGroup;
        }

        public void FastForward(uint forwardTickNum)
        {
            for (int index = 0; index < forwardTickNum; ++index)
            {
                this.UpdateGroup();
            }
        }

        public uint TickToTime(uint tick)
        {
            uint time = 0;
            if (m_rootTickerCtrl != null)
            {
                time = m_rootTickerCtrl.TickToTime(tick);
            }
            return time;
        }

        public uint TimeToTick(uint time)
        {
            uint tick = 0;
            if (m_rootTickerCtrl != null)
            {
                time = m_rootTickerCtrl.TickToTime(time);
            }
            return tick;
        }

        public void Clear()
        {
            if (m_tickerTaskList != null)
            {
                TickerTask tickerTask = null;
                for (int index = 0; index < m_tickerTaskList.Count; ++index)
                {
                    tickerTask = m_tickerTaskList[index];
                    if (tickerTask == null)
                    {
                        continue;
                    }
                    ReleaseTickerTask(tickerTask);
                }
                m_tickerTaskList.Clear();
            }
            if (m_tickerTaskAssistList != null)
            {
                TickerTask tickerTask = null;
                for (int index = 0; index < m_tickerTaskAssistList.Count; ++index)
                {
                    tickerTask = m_tickerTaskAssistList[index];
                    if (tickerTask == null)
                    {
                        continue;
                    }
                    ReleaseTickerTask(tickerTask);
                }
                m_tickerTaskAssistList.Clear();
            }
        }

        public void ReleaseTickerTaskWithID(uint tickerTaskID)
        {
            if (tickerTaskID == 0)
            {
                return;
            }
            if (m_tickerTaskList == null || m_tickerTaskAssistList == null)
            {
                return;
            }
            TickerTask tickerTask = null;
            for (int index = m_tickerTaskList.Count - 1; index >= 0; --index)
            {
                tickerTask = m_tickerTaskList[index];
                if (tickerTask == null)
                {
                    continue;
                }
                if (tickerTask.TickerID == tickerTaskID)
                {
                    tickerTask.WaitForRelease();
                    break;
                }
            }
        }

        public void Release()
        {
            if (m_rootTickerCtrl != null)
            {
                m_rootTickerCtrl.ReleaseTickerTaskWithID(this.TickerID);
            }
        }

        public Func<Ticker> TickerCreater
        {
            set { m_tickerCreater = value; }
        }

        public Func<TickerGroup> TickerGroupCreater
        {
            set { m_tickerGroupCreater = value; }
        }

        public Action<Ticker> TickerReleaser
        {
            set { m_tickerReleaser = value; }
        }

        public Action<TickerGroup> TickerGroupReleaser
        {
            set { m_tickerGroupReleaser = value; }
        }

        public string Name
        {
            set { m_tickerGroupName = value; }
            get { return m_tickerGroupName; }
        }

        public bool Pause
        {
            set
            {
                m_isPause = true;
            }
            get
            {
                bool isPause = !m_isStart;
                if (m_rootTickerCtrl != null)
                {
                    isPause = isPause || m_rootTickerCtrl.Pause;
                }
                return isPause;
            }
        }

        public bool IsSystem
        {
            set
            {
                if (m_rootTickerCtrl != null)
                {
                    m_rootTickerCtrl.IsSystem = value;
                }
            }
        }

        public bool IsOnline
        {
            set
            {
                if (m_rootTickerCtrl != null)
                {
                    m_rootTickerCtrl.IsOnline = value;
                }
            }
        }

        public bool IsServer
        {
            set
            {
                if (m_rootTickerCtrl != null)
                {
                    m_rootTickerCtrl.IsServer = value;
                }
            }
        }

        public bool IsLogic
        {
            set
            {
                if (m_rootTickerCtrl != null)
                {
                    m_rootTickerCtrl.IsLogic = value;
                }
            }
        }

        public uint LogicTick
        {
            get
            {
                uint logicTick = 0;
                if (m_rootTickerCtrl != null)
                {
                    logicTick = m_rootTickerCtrl.LogicTick;
                }
                return logicTick;
            }
        }

        public uint TickFrameRate
        {
            get
            {
                uint frameRate = 0;
                if (m_rootTickerCtrl != null)
                {
                    frameRate = m_rootTickerCtrl.TickFrameRate;
                }
                return frameRate;
            }
        }

        public uint TickerScale
        {
            set { m_tickerScale = value; }
            get { return m_tickerScale; }
        }

        public uint UpdateTick
        {
            get { return m_updateTick; }
        }
    }
}
