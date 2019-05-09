using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLogic
{
    public class LogicManagerCenter
    {
        private AIManager m_aiMgr = null;
        private CharacterManager m_characterMgr = null;
        private SceneDataManager m_sceneDataMgr = null;
        private SceneLogicCmdManager m_logicCmdMgr = null;
        private SceneLogicTimerManager m_logicTimerMgr = null;

        public LogicManagerCenter(LogicScene logicScene)
        {
            m_aiMgr = new AIManager(logicScene);
            m_characterMgr = new CharacterManager(logicScene);
            m_sceneDataMgr = new SceneDataManager(logicScene);
            m_logicCmdMgr = new SceneLogicCmdManager(logicScene);
            m_logicTimerMgr = new SceneLogicTimerManager(logicScene);
        }

        public void Init()
        {
            m_aiMgr.Init();
            m_characterMgr.Init();
            m_sceneDataMgr.Init();
            m_logicCmdMgr.Init();
            m_logicTimerMgr.Init();
        }

        public void Start()
        {

        }


        public void Release()
        {
            m_aiMgr.Release();
            m_characterMgr.Release();
            m_sceneDataMgr.Release();
            m_logicCmdMgr.Release();
            m_logicTimerMgr.Release();
        }

        public AIManager AIMgr
        {
            get { return m_aiMgr; }
        }

        public CharacterManager CharacterMgr
        {
            get { return m_characterMgr; }
        }

        public SceneDataManager SceneDataMgr
        {
            get { return m_sceneDataMgr; }
        }

        public SceneLogicCmdManager SceneLogicCmdMgr
        {
            get { return m_logicCmdMgr; }
        }

        public SceneLogicTimerManager SceneLogicTimerMgr
        {
            get { return m_logicTimerMgr; }
        }
    }
}
