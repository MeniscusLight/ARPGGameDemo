using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameLogic
{
    public class Ticker : TickerTask
    {
        private bool m_isStart = false;
        private bool m_isPause = false;
        private bool m_completeNeedRelease = false;

        private uint m_updateTick = 0;
        private uint m_repeatIndex = 0;
        private uint m_delayTick = 0;
        private uint m_repeatCount = 0;

        private ITickerCtrl m_tickerCtrl = null;

        private Action<Ticker> m_onTicker = null;
        private Action<Ticker> m_onTickerComplete = null;
        

    }
}