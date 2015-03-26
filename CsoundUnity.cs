using System;
using System.Collections.Generic;

using System.Text;
using System.Threading;
using UnityEngine;
using csoundcsharp;

/* 
 * C S O U N D  for  U N I T Y 3 D
 * 
 * Simple Csound wrapper for Unity 3d. 
 * This file is licensed under the same terms and disclaimers 
 * as Csound.
 * 
 * Copyright (C) 2015 Rory Walsh
*/ 

public class CsoundUnity
{
	public IntPtr csound;
	private volatile bool threadStatus;
	Thread performanceThread;
	ManualResetEvent manualReset;
	
	public CsoundUnity(string csdFile)
	{
		manualReset = new ManualResetEvent(false);
		performanceThread = new Thread(new ThreadStart(performCsound));
		csound = Csound6.NativeMethods.csoundCreate(System.IntPtr.Zero);
		Csound6.NativeMethods.csoundSetMessageCallback(csound, messageCallbackProxy);
		string[] runargs = new string[] { "csound", csdFile };
		int ret = Csound6.NativeMethods.csoundCompile(csound, 2, runargs);
		//Csound6Net.NativeMethods.csoundSetRTAudioModule(csound, "mme");
		performanceThread.Start();
		manualReset.Reset();
	}
	
	
	private void messageCallbackProxy(IntPtr csound, Int32 attr, string format, IntPtr valist)
	{
		Debug.Log(Csound6.NativeMethods.cvsprintf(format, valist));
	}
	
	public void startPerformance()
	{
		manualReset.Set();       
	}
	
	public void stopPerformance()
	{
		manualReset.Reset();
	}
	
	public void performCsound()
	{
		while (true)
		{
			manualReset.WaitOne();
			Csound6.NativeMethods.csoundPerformKsmps(csound);
		}
	}
	
	public void inputMessage(string scoreEvent)
	{
		Csound6.NativeMethods.csoundInputMessage(csound, scoreEvent);
	}
	
	public void setChannel(string channel, float value)
	{
		Csound6.NativeMethods.csoundSetControlChannel(csound, channel, value);
	}
}



