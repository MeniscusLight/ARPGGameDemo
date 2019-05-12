using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLogic;

namespace GameCtrls
{
    public class SceneCtrl
    {
        private LogicScene m_logicScene = null;


        public SceneCtrl()
        {
            
        }
                
        public void CreateScene()
        {
            if (m_logicScene != null)
            {
                this.ExitScene();
            }
            m_logicScene = new LogicScene();
        }

        private void InitScene()
        {
            if (m_logicScene == null)
            {
                return;
            }
            m_logicScene.Init();
        }

        public void StartScene()
        {
            if (m_logicScene == null)
            {
                return;
            }
            this.InitScene();
            m_logicScene.Start();
        }

        public void ExitScene()
        {
            if (m_logicScene != null)
            {
                m_logicScene.Release();
            }
            m_logicScene = null;
        }


        public LogicScene LogicScene
        {
            get { return m_logicScene; }
        }

    }
}
