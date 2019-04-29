using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameUIContrl
{
    public class GameFrameUIContrl : MonoBehaviour
    {
        Text m_textObj = null;
        private string m_frameString = null;
        void Start()
        {
            m_textObj = this.transform.GetComponent<Text>();
        }

        void Update()
        {
            m_textObj.text = GameGlobalData.GameFrameRate.ToString();
        }
    }

}