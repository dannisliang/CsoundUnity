using System;
using System.Collections.Generic;

using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
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
	Thread performanceThread;
	ManualResetEvent manualReset;
	
	public CsoundUnity(string csdFile)
	{
		manualReset = new ManualResetEvent(false);
		performanceThread = new Thread(new ThreadStart(performCsound));
		csound = Csound6.NativeMethods.csoundCreate(System.IntPtr.Zero);
		Csound6.NativeMethods.csoundCreateMessageBuffer(csound, 0);
		string[] runargs = new string[] { "csound", csdFile };
		int ret = Csound6.NativeMethods.csoundCompile(csound, 2, runargs);		
		//Csound6.NativeMethods.csoundSetRTAudioModule(csound, "coreaudio");
		performanceThread.Start();
		manualReset.Reset();
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
	
	public int getCsoundMessageCount()
	{
		return Csound6.NativeMethods.csoundGetMessageCnt(csound);
	}
	
	public string getCsoundMessage()
	{
		string message = getMessageText(Csound6.NativeMethods.csoundGetFirstMessage(csound));
		Csound6.NativeMethods.csoundPopFirstMessage(csound);
		return message;
	}

	public static string getMessageText(IntPtr message) {
		int len = 0;
		while (Marshal.ReadByte(message, len) != 0) ++len;
		byte[] buffer = new byte[len];
		Marshal.Copy(message, buffer, 0, buffer.Length);
		return Encoding.UTF8.GetString(buffer);
	}
	
}



