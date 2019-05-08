using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLogic
{
    public class CharacterManager : LogicBasicManager
    {
        public CharacterManager(LogicScene logicScene) : base(logicScene)
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
