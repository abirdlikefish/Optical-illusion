using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//音频管理器
public class AudioManager : Singleton<AudioManager>
{
    public GameObject emptyObject;
    // 整个游戏中，总的音源数量
    private const int AUDIO_CHANNEL_NUM = 10;
    // 淡进淡出时长
    public float fadeDuration = 1.5f;
    private struct CHANNEL
    {
        public AudioSource channel;
        public float keyOnTime; //记录最近一次播放音乐的时刻
        public float startT;
        public float endT;
    };
    private CHANNEL[] m_channels;
    void Awake()
    {
        m_channels = new CHANNEL[AUDIO_CHANNEL_NUM];
        for (int i = 0; i < AUDIO_CHANNEL_NUM; i++)
        {
            //每个频道对应一个音源
            m_channels[i].channel = Instantiate(emptyObject, gameObject.transform).AddComponent<AudioSource>();
            m_channels[i].channel.spatialBlend = 1;//3d立体声
            m_channels[i].keyOnTime = 0;
        }
    }
    private void Update()
    {
        SetFade();
    }
    int fadeId = -1;
    //根据channel的开始时间startT和结束时间endT淡进淡出
    void SetFade()
    {
        if (fadeId == -1)
            return;
        if (!m_channels[fadeId].channel.isPlaying)
            return;
        if (m_channels[fadeId].endT == float.MaxValue)
            return;
        float curT = m_channels[fadeId].channel.time;
        float deltaT = Mathf.Min(curT - m_channels[fadeId].startT,m_channels[fadeId].endT-curT);
        deltaT = Mathf.Clamp(deltaT,0f,fadeDuration);
        m_channels[fadeId].channel.volume = deltaT / fadeDuration;
        if (m_channels[fadeId].channel.time >= m_channels[fadeId].endT)
            m_channels[fadeId].channel.time = m_channels[fadeId].startT;
    }
    //公开方法：播放一次，参数为音频片段、音量、左右声道、速度
    //这个方法主要用于音效，因此考虑了音效顶替的逻辑
    public int PlayOneShot(AudioClip clip, float volume = 1f, float pan = 1f, float pitch = 1.0f)
    {
        for (int i = 0; i < m_channels.Length; i++)
        {
            if (!m_channels[i].channel.isPlaying)
            {
                m_channels[i].channel.transform.localPosition = Vector3.zero;
                m_channels[i].channel.clip = clip;
                m_channels[i].channel.volume = volume;
                m_channels[i].channel.pitch = pitch;
                m_channels[i].channel.panStereo = pan;
                m_channels[i].channel.loop = false;
                m_channels[i].channel.Play();
                m_channels[i].keyOnTime = Time.time;
                return i;
            }
        }
        return -1;
    }
    //公开方法：循环播放，用于播放长时间的背景音乐，处理方式相对简单一些
    public int PlayFadeLoop(AudioClip clip, float volume = 1f, float pan = 1f, float pitch = 1.0f,float startT = 0f,float endT = float.MaxValue)
    {
        for (int i = 0; i < m_channels.Length; i++)
        {
            if (!m_channels[i].channel.isPlaying)
            {
                //m_channels[i].channel.transform.parent = PlayerStateController.Instance.transform;
                m_channels[i].channel.transform.localPosition = Vector3.zero;
                m_channels[i].channel.clip = clip;
                m_channels[i].channel.volume = volume;
                m_channels[i].channel.pitch = pitch;
                m_channels[i].channel.panStereo = pan;
                m_channels[i].startT = m_channels[i].channel.time = startT;
                m_channels[i].endT = endT;
                m_channels[i].channel.loop = true;
                m_channels[i].channel.Play();
                m_channels[i].keyOnTime = Time.time;
                return fadeId = i;
            }
        }
        return -1;
    }
    //公开方法：停止所有音频
    public void StopAll()
    {
        foreach (CHANNEL channel in m_channels)
            channel.channel.Stop();
    }
    //公开方法：根据频道ID停止音频
    public void Stop(int id)
    {
        if (id >= 0 && id < m_channels.Length)
        {
            m_channels[id].channel.Stop();
        }
    }
    public void Stop(AudioClip ac)
    {
        foreach (CHANNEL channel in m_channels)
        {
            if(channel.channel.isPlaying && channel.channel.clip == ac)
                channel.channel.Stop();
        }
    }
    public AudioSource GetSource(int id)
    {
        return m_channels[id].channel;
    }
    public bool IsPlayingClip(AudioClip tarClip)
    {
        for(int i = 0;i < m_channels.Length; i++)
        {
            if (m_channels[i].channel.isPlaying && m_channels[i].channel.clip == tarClip)
                return true;
        }
        return false;
    }
}