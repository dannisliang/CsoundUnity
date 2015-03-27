# Csound wrapper for the Unity3d game engine.

This simple wrapper is based on Richard Henninger's Csound6Net .NET wrapper. If you wish to use the Csound API in a model that is idiomatic to .net please use his wrapper instead.
http://csound6net.codeplex.com

This work is licensed under the same terms and conditions as Csound. csound.github.io

## Information on using this wrapper
---------------------------------------------
To build this library open the CsoundUnity.sln file in MonoDevelop or MS Visual Studio. Edit the _dllVersion string in the csoundcsharp namespace so that it points to a valid csound64 library. You will then need to add a reference to UnityEngine.dll (http://docs.unity3d.com/Manual/UsingDLL.html). Hit run and a dynamic library will be built.

The following code it typical of how you might create and start a performance of Csound. This startup code is normally placed in the Start() or Awake() functions tied to the main camera, but you can start Csound from any object you wish. Note however that you should stick to a single instance of Csound. Multiple instances might work, but it's completely untested, and should not be necessary. 

```
void Start()
{
        csound = new CsoundUnity("/Users/walshr/Desktop/BasicUnityExample/Assets/Plugins/test.csd");
        csound.startPerformance();
        InvokeRepeating("logCsoundMessages", 0, 1f);
}

void logCsoundMessages()
{
        //print Csound message to Unity console....
        for(int i=0;i<csound.getCsoundMessageCount();i++)
            Debug.Log(csound.getCsoundMessage());
}
```

Once you have created an instance of Csound you can then send messages to it from any other gameObject you wish using the getComponent() method. The code below for example might appear in a main player's controller script. CameraController is the class attached to the main camera. When the Start() method gets called cameraScript is assigned an instance of the main camera class. Once we have an instance of this class we can access our Csound object. When the OnTriggerEvent is called Unity checks to see if the main player has collided with an enemy. If so, it will send a control message to Csound which will trigger some audio. 

```
public CameraController cameraScript;

void Start ()
{
        cameraScript = Camera.main.GetComponent<CameraController>();
}

void OnTriggerEnter(Collider other)
 {
     if (other.gameObject.tag == "Enemy") 
        {
            cameraScript.csound.setChannel("enemySound", Random.Range(0, 100));
        }
}
```
