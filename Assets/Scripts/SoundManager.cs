using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BGM�ESE���Ǘ�����N���X
/// </summary>
public class SoundManager : MonoBehaviour
{
    /// <summary>
    /// �����f�[�^�ݒ�p�N���X
    /// </summary>
    [System.Serializable]
    public class SoundData
    {
        // �T�E���h��
        public string name;
        // �����f�[�^
        public AudioClip audioClip;
        // ��������
        [Range(0.0f, 1.0f)]
        public float volume = 1.0f;
        // ���[�v�Đ����邩�ǂ���
        public bool isLoop = false;
    }
    // name ���L�[�������Ǘ��p Dictionary
    private Dictionary<string, SoundData> soundDictionary = new Dictionary<string, SoundData>();
    // �g�p����AudioSource���i�[����List
    private List<AudioSource> audioSourceList;
    // soundData�̍ő吔
    public int maxSoundData = 5;
    [SerializeField]
    private SoundData[] soundDatas;

    [SerializeField]
    private AudioClip[] thunderSE;

    public static SoundManager instance;

    private void Awake()
    {
        audioSourceList = new List<AudioSource>();
        // maxSoundData�̕�����AudioSource��p�ӂ��Ă���
        for (int i = 0; i < maxSoundData; i++)
        {
            AddAudioSource();
        }
        // soundDictionary�ɒǉ�
        foreach (var soundData in soundDatas)
        {
            soundDictionary.Add(soundData.name, soundData);
        }

        CheckInstance();
    }
    /// <summary>
    /// AudioSource��ǉ�����
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
    /// ���g�p��AudioSource���擾����
    /// </summary>
    /// <returns></returns>
    private AudioSource GetUnUsedAudioSource()
    {
        // ���g�p��AudioSource�����݂���ꍇ
        foreach (var audioSource in audioSourceList)
        {
            if (!audioSource.isPlaying)
            {
                return audioSource;
            }
        }
        // ���g�p��AudioSource�����݂��Ȃ��ꍇ�A�V��������
        return AddAudioSource();
    }
    /// <summary>
    /// �w�肳�ꂽAudioClip���Đ�����
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
        // AudioSource�ɐݒ�𔽉f����
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = isLoop;
        audioSource.Play();
    }
    /// <summary>
    /// �w�肳�ꂽ�T�E���h���Đ�����
    /// </summary>
    /// <param name="name"></param>
    public void PlaySound(string name)
    {
        if (soundDictionary.TryGetValue(name, out var soundData))
        {
            // �w��̉��������łɍĐ�����Ă���ꍇ
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
    /// �T�E���h��x���Đ�����
    /// </summary>
    /// <param name="name"></param>
    /// <param name="delayTime"></param>
    /// <returns></returns>
    public IEnumerator PlayDelaySound(string name, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        if (soundDictionary.TryGetValue(name, out var soundData))
        {
            // �w��̉��������łɍĐ�����Ă���ꍇ
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
    /// �z����̉����i���j�����C���_���ň�Đ�����
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
    /// �Đ����̃T�E���h���~����
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
    /// �w�肳�ꂽ�T�E���h���~����
    /// </summary>
    /// <param name="name"></param>
    public void StopSound(string name)
    {
        foreach (var audioSource in audioSourceList)
        {
            // �Đ������T�E���h������v����ꍇ
            if (audioSource.isPlaying
                && soundDictionary.ContainsKey(name)
                && audioSource.clip == soundDictionary[name].audioClip)
            {
                audioSource.Stop();
            }
        }
    }
    /// <summary>
    /// �T�E���h���t�F�[�h�A�E�g������
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
    /// �w��̃T�E���h���t�F�[�h�A�E�g������
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
    /// isLoop��true�̃T�E���h���t�F�[�h�A�E�g������
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
                audioSource.volume = 0;  // ���ʂ����Z�b�g���Ȃ�
            }
        }
    }

    /// <summary>
    /// �Đ����̃T�E���h�̉��ʂ�ݒ肷��
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
    /// �w�肳�ꂽ�T�E���h�̉��ʂ�ݒ肷��
    /// </summary>
    /// <param name="name"></param>
    public void SetSoundVolume(string name, float volume)
    {
        foreach (var audioSource in audioSourceList)
        {
            // �Đ�������������v����ꍇ
            if (audioSource.isPlaying
                && soundDictionary.ContainsKey(name)
                && audioSource.clip == soundDictionary[name].audioClip)
            {
                audioSource.volume = volume;
            }
        }
    }

    /// <summary>
    /// ���̃Q�[���I�u�W�F�N�g�ɕt�^����Ă��邩���ׂ�
    /// �t�^����Ă���ꍇ�͔j������
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
