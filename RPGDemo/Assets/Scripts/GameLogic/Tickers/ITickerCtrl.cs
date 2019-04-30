using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLogic
{
    public interface ITickerCtrl
    {
        Ticker CreateTicker(uint delayTick, uint repeatCount);
        Ticker CreateTickerWithTime(uint delayTick, uint repeatCount);
        TickerGroup CreateTickerGroup();

        Ticker FindTickerWithID(uint tickerID);
        TickerGroup FindTickerGroupWithID(uint tickerID);
        
        void Start();
        void Stop();
        
        uint TickToTime(uint tick);
        uint TimeToTick(uint time);

        void ReleaseTickerTaskWithID(uint tickerTaskID);
        void Release();
    }
}
