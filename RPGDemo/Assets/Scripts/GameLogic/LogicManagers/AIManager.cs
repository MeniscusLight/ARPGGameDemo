using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLogic
{
    public class AIManager : LogicBasicManager
    {

        public AIManager(LogicScene logicScene) : base(logicScene)
        {
            
        }
        
        public void Init()
        {
            base.BaseInit();
        }

        public void Release()
        {
            base.OnRelease();
        }
    }
}
