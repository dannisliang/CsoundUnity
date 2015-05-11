/*
 * Send .csd files to the StreamAssets folder where it will be read from
 * Files can be saved directly to streaming Assets folder, but it's better to follow
 * the usual project hierarchy and place them in the scripts folder
 */
#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.IO;

public class SendToStreamingAssets : AssetPostprocessor 
{
	public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets)
		{ 
			if (asset.EndsWith(".csd"))
			{
				//string filePath = asset.Substring(0, asset.Length - Path.GetFileName(asset).Length) + "Generated Assets/";
				string newFileName = Application.streamingAssetsPath+"/"+Path.GetFileNameWithoutExtension(asset)+".csd_";
				StreamReader reader = new StreamReader(asset);
				string fileData = reader.ReadToEnd();
				reader.Close();
				
				FileStream resourceFile = new FileStream(newFileName, FileMode.OpenOrCreate, FileAccess.Write);
				StreamWriter writer = new StreamWriter(resourceFile);
				writer.Write(fileData);
				writer.Close();
				resourceFile.Close();
				
				AssetDatabase.Refresh(ImportAssetOptions.Default);
			}
		}
	}	
}

#endif
