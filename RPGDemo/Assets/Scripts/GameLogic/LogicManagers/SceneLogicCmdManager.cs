using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLogic
{
    public class SceneLogicCmdManager : LogicBasicManager
    {
        private IList<SceneLogicCmd> m_logicCmdList = null;
        private IList<SceneLogicCmd> m_logicCmdAssistList = null;

        public SceneLogicCmdManager(LogicScene logicScene) : base(logicScene)
        {
            m_logicCmdList = new List<SceneLogicCmd>();
            m_logicCmdAssistList = new List<SceneLogicCmd>();
        }

        public void Init()
        {
            base.BaseInit();
        }

        public void Start()
        {
            var logicTimerMgr = this.LogicScene.LogicMgrCenter.SceneLogicTimerMgr;
            if (logicTimerMgr == null)
            {
                return;
            }
            Ticker ticker = logicTimerMgr.LogicCmdTickerMgr.CreateTicker(1, 0);
            ticker.SetOnTickerDelegate(this.OnExecuteSceneCmd);
            ticker.Start();
        }

        public void OnExecuteSceneCmd(Ticker ticker)
        {
            if (m_logicCmdList == null)
            {
                return;
            }
            uint currentUpdateTick = 0;
            SceneLogicCmd sceneLogicCmd = null;
            for(int index = 0; index < m_logicCmdList.Count; ++index)
            {
                sceneLogicCmd = m_logicCmdList[index];
                if (sceneLogicCmd == null)
                {
                    continue;
                }
                if (sceneLogicCmd.LogicUpdateTick >= currentUpdateTick)
                {
                    sceneLogicCmd.ExecuteCmd();
                }
                else
                {
                    m_logicCmdAssistList.Add(sceneLogicCmd);
                }
            }
            LogicUtil.CopyListContent<SceneLogicCmd>(m_logicCmdAssistList, m_logicCmdList);
            m_logicCmdAssistList.Clear();
        }




        public void Release()
        {
            base.OnRelease();
            if (m_logicCmdList != null)
            {
                m_logicCmdList.Clear();
            }
            if (m_logicCmdAssistList != null)
            {
                m_logicCmdAssistList.Clear();
            }
        }
    }
}
