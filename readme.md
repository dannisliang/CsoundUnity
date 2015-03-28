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

One shot samples can easily be triggered by sending a score statement to an instance of Csound using the sendScoreEvent() method. The following Csound instrument will play the file explosion.wav once when started. It's best to ensure that your sound files reside in the same folder as your main .csd file:

```
instr 1
  aL, aR diskin2 "explosion.wav", 1, 0, 0
  outs aL, aR
endin
```

In order to trigger this sample from Unity, we can send a score event to Csound:

```
void OnTriggerEnter(Collider other)
{
     if (other.gameObject.tag == "Enemy") 
        {
            cameraScript.csound.sendScoreEvent("i1 0 5");
        }
}
```

## Documentation

public CsoundUnity(string csdFile):
Main constructor. Compiles 'csdFile' and prepares Csound for performance.

public void startPerformance():
Start a performance of Csound. Many approaches may be taken here but, it is often best to start Csound with a dummy score statement, i.e., f0 9999999 This will tell Csound to perform for 99999999 seconds, but not to start any instruments until told to do so. Different events in the game can then trigger different instruments to start.  

public void stopPerformance():
Stops a performance of Csound. It is important that you call this each time you move back from game mode to scene editor mode in Unity. To do this you can use the OnApplicationQuit() method which gets call whenever the game is stopped. If you fail to stop Csound it will simply continue to run in the background. 

public void sendScoreEvent(string scoreEvent):
Send a score event to Csound. This can be used to start an instrument, or any number of instances of an instrument. You should use this for one-shot sounds. 

public void setChannel(string channel, float value):
Send a channel message to Csound. Use this if you need to send information from Unity to an instrument running in Csound. For example, one could send the player's current position to a Csound instrument, which could then use that information to apply some spatialisation to the player's sound. 

public int getCsoundMessageCount():
Retreives the number of messages in the Csound message buffer. 

public string getCsoundMessage():
Returns a message from the Csound message buffer. You will need to use this if you wish to view Csound's console output. See the example provided above. 