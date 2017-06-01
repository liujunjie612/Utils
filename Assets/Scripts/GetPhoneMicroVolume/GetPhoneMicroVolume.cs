using UnityEngine;
using System.Collections;

public class GetPhoneMicroVolume : MonoBehaviour {

    public AudioSource audio;
    private const int timeLimit = 3600;
    
    private string selectedDevice = null;//即为使用系统默认麦克风

    public TextMesh txtMesh;


	void Start () 
    {
        StartRecord();
	}
	

	void Update () 
    {
        txtMesh.text = Volume.ToString() ;
	}

    public float Volume
    {
        get
        {
            if (Microphone.IsRecording(selectedDevice))
            {
                int sampleSize = 128;
                float[] samples = new float[sampleSize];
                int startPosition = Microphone.GetPosition(selectedDevice) - (sampleSize + 1);
                this.audio.clip.GetData(samples, startPosition);
                float levelMax = 0;
                for (int i = 0; i < sampleSize; ++i)
                {
                    float wavePeak = samples[i];
                    if (levelMax < wavePeak)
                        levelMax = wavePeak;
                }
                return levelMax * 100;
            }
            return 0;
        }
    }
    public void StartRecord()
    {
        audio.Stop();
        audio.loop = false;
        audio.mute = false;
        audio.clip = Microphone.Start(selectedDevice, true, timeLimit, 128);
    }
    public void StopRecord()
    {
        Microphone.End(selectedDevice);
        audio.Stop();
    }
}
