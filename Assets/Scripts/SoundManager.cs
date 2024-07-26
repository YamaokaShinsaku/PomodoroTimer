using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BGM・SEを管理するクラス
/// </summary>
public class SoundManager : MonoBehaviour
{
    /// <summary>
    /// 音源データ設定用クラス
    /// </summary>
    [System.Serializable]
    public class SoundData
    {
        // サウンド名
        public string name;
        // 音源データ
        public AudioClip audioClip;
        // 初期音量
        [Range(0.0f, 1.0f)]
        public float volume = 1.0f;
        // ループ再生するかどうか
        public bool isLoop = false;
    }
    // name をキーをした管理用 Dictionary
    private Dictionary<string, SoundData> soundDictionary = new Dictionary<string, SoundData>();
    // 使用するAudioSourceを格納するList
    private List<AudioSource> audioSourceList;
    // soundDataの最大数
    public int maxSoundData = 5;
    [SerializeField]
    private SoundData[] soundDatas;

    [SerializeField]
    private AudioClip[] thunderSE;

    public static SoundManager instance;

    private void Awake()
    {
        audioSourceList = new List<AudioSource>();
        // maxSoundDataの分だけAudioSourceを用意しておく
        for (int i = 0; i < maxSoundData; i++)
        {
            AddAudioSource();
        }
        // soundDictionaryに追加
        foreach (var soundData in soundDatas)
        {
            soundDictionary.Add(soundData.name, soundData);
        }

        CheckInstance();
    }
    /// <summary>
    /// AudioSourceを追加する
    /// </summary>
    /// <returns></returns>
    private AudioSource AddAudioSource()
    {
        var newAudioSource = gameObject.AddComponent<AudioSource>();
        newAudioSource.playOnAwake = false;
        audioSourceList.Add(newAudioSource);

        return newAudioSource;
    }
    /// <summary>
    /// 未使用のAudioSourceを取得する
    /// </summary>
    /// <returns></returns>
    private AudioSource GetUnUsedAudioSource()
    {
        // 未使用のAudioSourceが存在する場合
        foreach (var audioSource in audioSourceList)
        {
            if (!audioSource.isPlaying)
            {
                return audioSource;
            }
        }
        // 未使用のAudioSourceが存在しない場合、新しく生成
        return AddAudioSource();
    }
    /// <summary>
    /// 指定されたAudioClipを再生する
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="volume"></param>
    /// <param name="isLoop"></param>
    public void PlaySound(AudioClip clip, float volume, bool isLoop)
    {
        var audioSource = GetUnUsedAudioSource();

        if (!audioSource)
        {
            return;
        }
        // AudioSourceに設定を反映する
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = isLoop;
        audioSource.Play();
    }
    /// <summary>
    /// 指定されたサウンドを再生する
    /// </summary>
    /// <param name="name"></param>
    public void PlaySound(string name)
    {
        if (soundDictionary.TryGetValue(name, out var soundData))
        {
            // 指定の音源がすでに再生されている場合
            foreach (var audioSource in audioSourceList)
            {
                if (audioSource.isPlaying && audioSource.clip == soundData.audioClip)
                {
                    return;
                }
            }

            PlaySound(soundData.audioClip, soundData.volume, soundData.isLoop);
        }
    }
    /// <summary>
    /// サウンドを遅延再生する
    /// </summary>
    /// <param name="name"></param>
    /// <param name="delayTime"></param>
    /// <returns></returns>
    public IEnumerator PlayDelaySound(string name, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        if (soundDictionary.TryGetValue(name, out var soundData))
        {
            // 指定の音源がすでに再生されている場合
            foreach (var audioSource in audioSourceList)
            {
                if (audioSource.isPlaying && audioSource.clip == soundData.audioClip)
                {
                    yield return null;
                }
            }

            PlaySound(soundData.audioClip, soundData.volume, soundData.isLoop);
        }
    }
    /// <summary>
    /// 配列内の音声（雷）をラインダムで一つ再生する
    /// </summary>
    public void PlayRandomSound()
    {
        if (thunderSE == null || thunderSE.Length == 0)
        {
            return;
        }

        int randomIndex = Random.Range(0, thunderSE.Length);
        PlaySound(thunderSE[randomIndex], 0.25f, false);
    }

    /// <summary>
    /// 再生中のサウンドを停止する
    /// </summary>
    public void StopAllSound()
    {
        foreach (var audioSource in audioSourceList)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
    /// <summary>
    /// 指定されたサウンドを停止する
    /// </summary>
    /// <param name="name"></param>
    public void StopSound(string name)
    {
        foreach (var audioSource in audioSourceList)
        {
            // 再生中かつサウンド名が一致する場合
            if (audioSource.isPlaying
                && soundDictionary.ContainsKey(name)
                && audioSource.clip == soundDictionary[name].audioClip)
            {
                audioSource.Stop();
            }
        }
    }
    /// <summary>
    /// サウンドをフェードアウトさせる
    /// </summary>
    public void StopFadeSound(float duration)
    {
        StartCoroutine(FadeOutSound(duration));
    }
    private IEnumerator FadeOutSound(float duration)
    {
        foreach (var audioSource in audioSourceList)
        {
            if (audioSource.isPlaying)
            {
                float startVolume = audioSource.volume;
                float timer = 0.0f;

                while (timer < duration)
                {
                    audioSource.volume = Mathf.Lerp(startVolume, 0, timer / duration);
                    timer += Time.deltaTime;
                    yield return null;
                }
                audioSource.Stop();
                audioSource.volume = startVolume;
            }
        }
    }
    /// <summary>
    /// 指定のサウンドをフェードアウトさせる
    /// </summary>
    public void StopFadeSound(string name, float duration)
    {
        StartCoroutine(FadeOutSound(name, duration));
    }
    private IEnumerator FadeOutSound(string name, float duration)
    {
        foreach (var audioSource in audioSourceList)
        {
            if (audioSource.isPlaying
                && soundDictionary.ContainsKey(name)
                && audioSource.clip == soundDictionary[name].audioClip)
            {
                float startVolume = audioSource.volume;
                float timer = 0.0f;

                while (timer < duration)
                {
                    audioSource.volume = Mathf.Lerp(startVolume, 0, timer / duration);
                    timer += Time.deltaTime;
                    yield return null;
                }

                audioSource.Stop();
                audioSource.volume = startVolume;
            }
        }
    }
    /// <summary>
    /// isLoopがtrueのサウンドをフェードアウトさせる
    /// </summary>
    public void StopFadeLoopingSounds(float duration)
    {
        StartCoroutine(FadeOutLoopingSounds(duration));
    }
    private IEnumerator FadeOutLoopingSounds(float duration)
    {
        foreach (var audioSource in audioSourceList)
        {
            if (audioSource.isPlaying
                && audioSource.loop)
            {
                float startVolume = audioSource.volume;
                float timer = 0.0f;

                while (timer < duration)
                {
                    audioSource.volume = Mathf.Lerp(startVolume, 0, timer / duration);
                    timer += Time.deltaTime;
                    yield return null;
                }

                audioSource.Stop();
                audioSource.volume = 0;  // 音量をリセットしない
            }
        }
    }

    /// <summary>
    /// 再生中のサウンドの音量を設定する
    /// </summary>
    public void SetAllSoundVolume(float volume)
    {
        foreach (var audioSource in audioSourceList)
        {
            if (audioSource.isPlaying)
            {
                audioSource.volume = volume;
            }
        }
    }
    /// <summary>
    /// 指定されたサウンドの音量を設定する
    /// </summary>
    /// <param name="name"></param>
    public void SetSoundVolume(string name, float volume)
    {
        foreach (var audioSource in audioSourceList)
        {
            // 再生中かつ音源が一致する場合
            if (audioSource.isPlaying
                && soundDictionary.ContainsKey(name)
                && audioSource.clip == soundDictionary[name].audioClip)
            {
                audioSource.volume = volume;
            }
        }
    }

    /// <summary>
    /// 他のゲームオブジェクトに付与されているか調べる
    /// 付与されている場合は破棄する
    /// </summary>
    private void CheckInstance()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }
}
