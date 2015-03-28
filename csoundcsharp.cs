using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;


/*
 * C S O U N D for C#
 * Simple wrapper building C# hosts for Csound 6 via the Csound API
 * and is licensed under the same terms and disclaimers as Csound described below.
 * Copyright (C) 2013 Richard Henninger, Rory Walsh
 *
 * C S O U N D
 *
 * An auto-extensible system for making music on computers
 * by means of software alone.
 *
 * Copyright (C) 2001-2013 Michael Gogins, Matt Ingalls, John D. Ramsdell,
 *                         John P. ffitch, Istvan Varga, Victor Lazzarini,
 *                         Andres Cabrera and Steven Yi
 *
 * This software is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This software is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this software; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 *
 */

namespace csoundcsharp
{
	// This simple wrapper is based on Richard Henninger's Csound6Net .NET wrapper. If you wish to 
	// use the Csound API in a model that is idiomatic to .net please use his wrapper instead. 
	// http://csound6net.codeplex.com

	// This lightweight wrapper was created to provide an interface to the Unity3d game engine


	public partial class Csound6 {
		
		// You will need to comment/uncomment one of these depending on you OS.
		internal const string _dllVersion ="csound64.dll";
		//internal const string _dllVersion ="/Library/Frameworks/CsoundLib64.framework/CsoundLib64"; //change this to point to a different dll for csound functions: currently csound6RC2
		
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal delegate void MessageCallbackProxy(IntPtr csound, Int32 attr, string format, IntPtr valist);
		
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal delegate void FileOpenCallbackProxy(IntPtr csound, string pathname, int csFileType, int writing, int temporary);
		
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal delegate void RtcloseCallbackProxy(IntPtr csound);

		public class NativeMethods
		{
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern Int32 csoundInitialize([In] int flags);

			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern IntPtr csoundCreate(IntPtr hostdata);

			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void csoundDestroy([In] IntPtr csound);
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern Int32 csoundGetVersion();

			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern Int32 csoundGetAPIVersion();
						
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
			internal static extern IntPtr csoundParseOrc([In] IntPtr csound, [In] String str);
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern Int32 csoundCompileTree([In] IntPtr csound, [In] IntPtr root);
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void csoundDeleteTree([In] IntPtr csound, [In] IntPtr root);
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
			internal static extern Int32 csoundCompileOrc([In] IntPtr csound, [In, MarshalAs(UnmanagedType.LPStr)] String orchStr);
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
			internal static extern double csoundEvalCode([In] IntPtr csound, [In, MarshalAs(UnmanagedType.LPStr)] String orchStr);
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
			internal static extern Int32 csoundCompileArgs([In] IntPtr csound, [In] Int32 argc, [In] string[] argv);
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern Int32 csoundStart([In] IntPtr csound);
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
			internal static extern IntPtr csoundInputMessage([In] IntPtr csound, [In] String str);
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
			internal static extern IntPtr csoundSetControlChannel([In] IntPtr csound, [In] String str, [In] double value);            
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern Int32 csoundCompile([In] IntPtr csound, [In] Int32 argc, [In] string[] argv);

			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern int csoundPerform([In] IntPtr csound);

			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern Int32 csoundPerformKsmps([In] IntPtr csound);

			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern Int32 csoundPerformBuffer([In] IntPtr csound);
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void csoundStop([In] IntPtr csound);

			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern Int32 csoundCleanup([In] IntPtr csound);

			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void csoundReset([In] IntPtr csound);

			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern Double csoundGetSr([In] IntPtr csound);
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern Double csoundGetKr([In] IntPtr csound);
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern UInt32 csoundGetKsmps([In] IntPtr csound);

			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern UInt32 csoundGetNchnls([In] IntPtr csound);
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern UInt32 csoundGetNchnlsInput([In] IntPtr csound);

			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern Double csoundGet0dBFS([In] IntPtr csound);
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern Int64 csoundGetCurrentTimeSamples([In] IntPtr csound);
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern Int32 csoundGetSizeOfMYFLT();
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern IntPtr csoundGetHostData([In] IntPtr csound);
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void csoundSetHostData([In] IntPtr csound, IntPtr hostData);

			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern Int32 csoundGetDebug([In] IntPtr csound);

			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void csoundSetDebug([In] IntPtr csound, [In] Int32 debug);

			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern IntPtr csoundGetOutputName([In] IntPtr csound);
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
			internal static extern void csoundSetOutput([In] IntPtr csound, [In, MarshalAs(UnmanagedType.LPStr)] string name, [In, MarshalAs(UnmanagedType.LPStr)] string type, [In, MarshalAs(UnmanagedType.LPStr)] string format);
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
			internal static extern void csoundSetInput([In] IntPtr csound, [In, MarshalAs(UnmanagedType.LPStr)] string name);
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
			internal static extern void csoundSetMIDIFileOutput([In] IntPtr csound, [In, MarshalAs(UnmanagedType.LPStr)] string name);
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
			internal static extern void csoundSetMIDIFileInput([In] IntPtr csound, [In, MarshalAs(UnmanagedType.LPStr)] string name);
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void csoundSetFileOpenCallback([In] IntPtr csound, FileOpenCallbackProxy processMessage);

			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
			internal static extern void csoundSetRTAudioModule([In] IntPtr csound, [In, MarshalAs(UnmanagedType.LPStr)] string module);
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
			internal static extern int csoundGetModule([In] IntPtr csound, int number, ref IntPtr name, ref IntPtr type);
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern Int32 csoundGetAudioDevList([In] IntPtr csound, [Out] IntPtr list, [In] Int32 isOutput);
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern Int32 csoundGetInputBufferSize([In] IntPtr csound);
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern Int32 csoundGetOutputBufferSize([In] IntPtr csound);

			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
			internal static extern void csoundSetMIDIModule([In] IntPtr csound, [In, MarshalAs(UnmanagedType.LPStr)] string module);
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern Int32 csoundGetMIDIDevList([In] IntPtr csound, [Out] IntPtr list, [In] Int32 isOutput);

			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
			internal static extern Int32 csoundReadScore([In] IntPtr csound, [In, MarshalAs(UnmanagedType.LPStr)] string score);

			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern Double csoundGetScoreTime([In] IntPtr csound);
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern int csoundIsScorePending([In] IntPtr csound);

			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void csoundSetScorePending([In] IntPtr csound, [In] Int32 pending);

			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern Double csoundGetScoreOffsetSeconds([In] IntPtr csound);

			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void csoundSetScoreOffsetSeconds([In] IntPtr csound, [In] Double time);

			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void csoundRewindScore([In] IntPtr csound);

			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern int csoundScoreSort([In] IntPtr csound, [In] IntPtr inFile, [In] IntPtr outFile);

			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern int csoundScoreExtract(IntPtr csound, IntPtr inFile, IntPtr outFile, IntPtr extractFile);

			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern void csoundSetDefaultMessageCallback(MessageCallbackProxy processMessage);
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void csoundSetRtcloseCallback([In] IntPtr csound, RtcloseCallbackProxy processRtclose);

			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern void csoundSetMessageCallback([In] IntPtr csound, MessageCallbackProxy processMessage);
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern Int32 csoundGetMessageLevel([In] IntPtr csound);

			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void csoundSetMessageLevel([In] IntPtr csound, [In] Int32 messageLevel);
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void csoundCreateMessageBuffer([In] IntPtr csound, [In] int toStdOut);
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]	
			internal static extern void csoundDestroyMessageBuffer([In] IntPtr csound);
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern IntPtr csoundGetFirstMessage([In] IntPtr csound);
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void csoundPopFirstMessage([In] IntPtr csound);
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern int csoundGetMessageCnt([In] IntPtr csound);	

			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern IntPtr csoundGetNamedGens([In] IntPtr csound);
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern Int32 csoundNewOpcodeList([In] IntPtr csound, [Out] out IntPtr ppOpcodeList);
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern void csoundDisposeOpcodeList([In] IntPtr csound, [In] IntPtr ppOpcodeList);

			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
			internal static extern IntPtr csoundGetEnv([In] IntPtr csound, [In, MarshalAs(UnmanagedType.LPStr)] String key);

			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
			internal static extern Int32 csoundSetGlobalEnv([In, MarshalAs(UnmanagedType.LPStr)] string name, [In, MarshalAs(UnmanagedType.LPStr)] string value);

			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern UInt32 csoundGetRandomSeedFromTime();
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern long csoundRunCommand([In] string[] argv, [In] int nowait);
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl)]
			internal static extern void csoundSleep(uint milleseconds);
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern IntPtr csoundListUtilities([In] IntPtr csound);
			
			[DllImport(_dllVersion, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			internal static extern void csoundDeleteUtilityList([In] IntPtr csound, IntPtr list);
		}

	}
}

