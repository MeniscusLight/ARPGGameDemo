using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


public class GameGlobalCenter : MonoBehaviour
{
    private uint m_prevUpdateLogicTickerTime = 0;
    private uint m_globalUpdateFrame = 0;

    private Stopwatch m_stopWatch = null;

	void Start ()
    {
        m_stopWatch = new Stopwatch();
        m_stopWatch.Start();
	}
	
	void Update ()
    {
		
	}
}
