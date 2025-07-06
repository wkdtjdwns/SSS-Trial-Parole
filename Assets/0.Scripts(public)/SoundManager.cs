using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioMixer mixer;

    public AudioSource bgSound; //�����
    public AudioClip[] bgList;  //����� ����Ʈ

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;    //���� �ε�� �� ȣ��Ǵ� �̺�Ʈ
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// ���� �ε�� �� ȣ��Ǵ� �޼���
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="arg1"></param>
    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        for (int i = 0; i < bgList.Length; i++)
        {
            if (arg0.name == bgList[i].name)    //�� �̸��� ����� �̸��� ������
                BGMPlay(bgList[i]);
        }
    }

    /// <summary>
    /// ����� ���� ����
    /// </summary>
    /// <param name="val">���� ��</param>
    public void BGMSoundVolume(float val)
    {
        mixer.SetFloat("BGMSound", Mathf.Log10(val) * 20);  //mixer�� ������ �α׽�����?�� ����Ѵٳ׿�.
    }

    /// <summary>
    /// SFX ���� ����
    /// </summary>
    /// <param name="val">���� ��</param>
    public void SFXSoundVolume(float val)
    {
        mixer.SetFloat("SFXSound", Mathf.Log10(val) * 20);  //mixer�� ������ �α׽�����?�� ����Ѵٳ׿�.
    }

    /// <summary>
    /// SFX ���
    /// </summary>
    /// <param name="sfxName">�Ҹ� �̸�</param>
    /// <param name="clip">Ŭ��</param>
    public void SFXPlay(string sfxName, AudioClip clip)
    {
        GameObject obj = new GameObject(sfxName + "Souund");
        AudioSource audioSource = obj.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0]; // SFX �ͼ� �׷쿡 ����
        audioSource.clip = clip;
        audioSource.Play();

        Destroy(obj, clip.length); // ����� ������ ������Ʈ ����
    }

    /// <summary>
    /// BGM ���
    /// </summary>
    /// <param name="clip">Ŭ��</param>
    public void BGMPlay(AudioClip clip)
    {
        bgSound.outputAudioMixerGroup = mixer.FindMatchingGroups("BGM")[0]; // BGM �ͼ� �׷쿡 ����
        bgSound.clip = clip;
        bgSound.loop = true; // �ݺ� ���
        bgSound.volume = 0.15f;
        bgSound.Play();
    }
}
