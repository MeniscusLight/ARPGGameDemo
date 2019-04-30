using System;

namespace GameLogic
{
    public interface ITickerCtrl
    {
        Ticker CreateTicker(uint delayTick, uint repeatCount);
        Ticker CreateTickerWithTime(uint delayTime, uint repeatCount);
        TickerGroup CreateTickerGroup();

        Ticker FindTickerWithID(uint tickerID);
        TickerGroup FindTickerGroupWithID(uint tickerID);
        
        void Start();
        void Stop();
        void Clear();

        void FastForward(uint forwardTickNum);
        
        uint TickToTime(uint tick);
        uint TimeToTick(uint time);

        void ReleaseTickerTaskWithID(uint tickerTaskID);
        void Release();

        Func<Ticker> TickerCreater
        {
            set;
        }

        Func<TickerGroup> TickerGroupCreater
        {
            set;
        }

        Action<Ticker> TickerReleaser
        {
            set;
        }

        Action<TickerGroup> TickerGroupReleaser
        {
            set;
        }

        string Name
        {
            set;
            get;
        }

        bool Pause
        {
            set;
            get;
        }

        bool IsSystem
        {
            set;
        }

        bool IsOnline
        {
            set;
        }

        bool IsServer
        {
            set;
        }

        bool IsLogic
        {
            set;
        }

        uint LogicTick
        {
            get;
        }

        uint TickFrameRate
        {
            get;
        }

        uint TickerScale
        {
            set;
            get;
        }

        uint UpdateTick
        {
            get;
        }
    }
}
