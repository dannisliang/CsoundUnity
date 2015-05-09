using System;
using System.Collections.Generic;

using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using UnityEngine;
using csoundcsharp;

/* 
 * C S O U N D ____ U N I T Y ____ B R I D G E
 * 
 * Simple Csound bridge for Unity 3d. 
 * This file is licensed under the same terms and disclaimers 
 * as Csound.
 * 
 * Copyright (C) 2015 Rory Walsh
 * 
 * Although you can use this interface directly, it might be easier to adapt and modify 
 * the CsoundUnity.cs file for your Unity projects.
*/ 

public class CsoundUnityBridge
{
	public IntPtr csound;
	Thread performanceThread;
	ManualResetEvent manualReset;
	public string baseDir;

	/*
	 * This constructor tries to take the path of the Csound file, which should always be lcoated
	 * in the Scripts folder, and use it to add csound64.dll to the system path, and set up OPCODE6DIR64
	 * but it doens't work. I'm leaving the code here in case anyone can spot the reason why :)
	public CsoundUnityBridge(string csdFile)
	{
		//Environment.SetEnvironmentVariable("Path", varEnv);
		string baseDir = csdFile.Substring(0, csdFile.IndexOf("/Assets"));
		System.Environment.SetEnvironmentVariable("Path", baseDir+"/Assets/Plugins/CsoundUnity");
		Csound6.NativeMethods.csoundSetGlobalEnv("OPCODE6DIR64", baseDir+"/Assets/Plugins/CsoundUnity/CsoundPluginsOpcodes");
		Console.Write(baseDir+"/Assets/Plugins/CsoundUnity/CsoundPluginsOpcodes");
		string envPath = System.IO.Directory.GetCurrentDirectory()+"/Assets/Plugins";
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
	*/

	//constructor sets up the OPCODE6DIR64 directory that holds the Csound plugins. 
  	public CsoundUnityBridge(string opcodeDir)
	{
		//Environment.SetEnvironmentVariable("Path", varEnv);
		Csound6.NativeMethods.csoundSetGlobalEnv("OPCODE6DIR64", opcodeDir);
		baseDir = opcodeDir;
		//string envPath = System.IO.Directory.GetCurrentDirectory()+"/Assets/Plugins";
		manualReset = new ManualResetEvent(false);
		performanceThread = new Thread(new ThreadStart(performCsound));
		manualReset.Reset();
	}

	//called to create an instance of Csound
	public int createCsound(string csdFile)
	{
		csound = Csound6.NativeMethods.csoundCreate(System.IntPtr.Zero);
		Csound6.NativeMethods.csoundCreateMessageBuffer(csound, 0);
		string[] runargs = new string[] { "csound", csdFile };
		int ret = Csound6.NativeMethods.csoundCompile(csound, 2, runargs);	
		if(ret==0)
			performanceThread.Start();


		return ret;
	}

	//starts a performance of Csound
	public void startPerformance()
	{
		manualReset.Set();       
	}
	
	public void stopPerformance()
	{
		manualReset.Reset();
	}
	
	private void performCsound()
	{
		while (true)
		{
			manualReset.WaitOne();
			Csound6.NativeMethods.csoundPerformKsmps(csound);
		}
	}
	
	public void sendScoreEvent(string scoreEvent)
	{
		Csound6.NativeMethods.csoundInputMessage(csound, scoreEvent);
	}
	
	public void setChannel(string channel, float value)
	{
		Csound6.NativeMethods.csoundSetControlChannel(csound, channel, value);
	}

	public void setStringChannel(string channel, string value)
	{
		Csound6.NativeMethods.csoundSetStringChannel(csound, channel, value);
	}

	public double getTable(int table, int index)
	{
		return Csound6.NativeMethods.csoundTableGet(csound, table, index);
	}

	public double getChannel(string channel)
	{
		return Csound6.NativeMethods.csoundGetControlChannel(csound, channel, IntPtr.Zero);
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



