using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLogic
{
    public class SceneLogicCmdManager : LogicBasicManager
    {
        private IList<SceneLogicCmd> m_logicCmdList = null;

        public SceneLogicCmdManager(LogicScene logicScene) : base(logicScene)
        {
            m_logicCmdList = new List<SceneLogicCmd>();
        }

        public void Init()
        {
            base.BaseInit();
        }

        public void Start()
        {

        }

        public void OnExecuteSceneCmd()
        {
            if (m_logicCmdList == null)
            {
                return;
            }
            SceneLogicCmd sceneLogicCmd = null;
            for(int index = 0; index < m_logicCmdList.Count; ++index)
            {
                sceneLogicCmd = m_logicCmdList[index];
                if (sceneLogicCmd == null)
                {
                    continue;
                }

            }
        }

        public void Release()
        {
            base.OnRelease();
            if (m_logicCmdList != null)
            {
                m_logicCmdList.Clear();
            }
        }
    }
}
