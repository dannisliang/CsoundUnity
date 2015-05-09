using UnityEngine;
using System.Collections;

/* 
 * C S O U N D ____ U N I T Y 
 * 
 * Simple Csound bridge for Unity 3d. 
 * This file is licensed under the same terms and disclaimers 
 * as Csound.
 * 
 * Copyright (C) 2015 Rory Walsh
 * 
 * This script provides some simple utility functions to start and control
 * an instance of Csound in Unity file. If you need more Csound API functions 
 * you should look at the CsoundUnityBridge.cs file.
*/

public class CsoundUnity : MonoBehaviour {

	// Use this for initialization
	public CsoundUnityBridge csound;
	public string csoundFile;

	void Start () 
	{
		 
		 /* I M P O R T A N T
		 * 
		 * Please ensure that all csd files reside in your Assets/Scripts directory
		 *
		 */

		string csoundFilePath = Application.streamingAssetsPath+"/"+csoundFile+"_";
		/*
		 * the CsoundUnity constructor takes a path to the Csound Plugin Opcodes directory.
		 * After this has been set we call createCsound(string csdFile) to create an instance of
		 * Csound and compile the 'csdFile'. After this we start the performance of Csound. 
		 */
		System.Environment.SetEnvironmentVariable("Path", Application.streamingAssetsPath);
		string opcodeDirPath = Application.streamingAssetsPath;
		csound = new CsoundUnityBridge(opcodeDirPath);
		csound.createCsound(csoundFilePath);
		csound.startPerformance();
		csound.setStringChannel("baseDir", opcodeDirPath);

		/*
		 * This method prints the Csound output to the Unity console
		 */
		InvokeRepeating("logCsoundMessages", 0, 1f);	

	}

	void Update () 
	{
		//we don't need to do anything in here...
	}


	/*
	 * Called when the game stops. Needed so that Csound stops when 
	 * your game does 
	 */ 
	void OnApplicationQuit()
	{
		csound.stopPerformance();
	}


	/*
	 * Sets a Csound channel. Used in connection with a chnget opcode
	 * in your Csound instrument.
	 */ 
	public void setChannel(string channel, float val)
	{
		csound.setChannel(channel, val);
	}

	public void setStringChannel(string channel, string val)
	{
		csound.setStringChannel(channel, val);
	}
	/*
	 * Gets a Csound channel. Used in connection with a chnset opcode
	 * in your Csound instrument.
	 */ 
	public double getChannel(string channel)
	{
		return csound.getChannel(channel);
	}

	/*
	 * Send a score event to Csound in the form of "i1 0 10 ...."
	 */
	public void sendScoreEvent(string scoreEvent)
	{
		csound.sendScoreEvent(scoreEvent);
	}


	/*
	 * Print the Csound output to the Unity message console
	 */
	void logCsoundMessages()
	{
		//print Csound message to Unity console....
		for(int i=0;i<csound.getCsoundMessageCount();i++)
			Debug.Log(csound.getCsoundMessage());
	}
}
