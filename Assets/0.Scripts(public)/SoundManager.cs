using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioMixer mixer;

    public AudioSource bgSound; //배경음
    public AudioClip[] bgList;  //배경음 리스트

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;    //씬이 로드될 때 호출되는 이벤트
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 씬이 로드될 때 호출되는 메서드
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="arg1"></param>
    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        for (int i = 0; i < bgList.Length; i++)
        {
            if (arg0.name == bgList[i].name)    //씬 이름과 배경음 이름이 같으면
                BGMPlay(bgList[i]);
        }
    }

    /// <summary>
    /// 배경음 볼륨 조절
    /// </summary>
    /// <param name="val">볼륨 값</param>
    public void BGMSoundVolume(float val)
    {
        mixer.SetFloat("BGMSound", Mathf.Log10(val) * 20);  //mixer의 볼륨은 로그스케일?을 사용한다네요.
    }

    /// <summary>
    /// SFX 볼륨 조절
    /// </summary>
    /// <param name="val">볼륨 값</param>
    public void SFXSoundVolume(float val)
    {
        mixer.SetFloat("SFXSound", Mathf.Log10(val) * 20);  //mixer의 볼륨은 로그스케일?을 사용한다네요.
    }

    /// <summary>
    /// SFX 재생
    /// </summary>
    /// <param name="sfxName">소리 이름</param>
    /// <param name="clip">클립</param>
    public void SFXPlay(string sfxName, AudioClip clip)
    {
        GameObject obj = new GameObject(sfxName + "Souund");
        AudioSource audioSource = obj.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0]; // SFX 믹서 그룹에 연결
        audioSource.clip = clip;
        audioSource.Play();

        Destroy(obj, clip.length); // 재생이 끝나면 오브젝트 삭제
    }

    /// <summary>
    /// BGM 재생
    /// </summary>
    /// <param name="clip">클립</param>
    public void BGMPlay(AudioClip clip)
    {
        bgSound.outputAudioMixerGroup = mixer.FindMatchingGroups("BGM")[0]; // BGM 믹서 그룹에 연결
        bgSound.clip = clip;
        bgSound.loop = true; // 반복 재생
        bgSound.volume = 0.15f;
        bgSound.Play();
    }
}
