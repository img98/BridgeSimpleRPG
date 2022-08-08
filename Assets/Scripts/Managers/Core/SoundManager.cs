using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount]; //Bgm, Effect용 소스가 각각 필요한데, 그냥 배열로 만들어서 코드를 한번만 썻다.

    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>(); //오디오클립들을 담아놓을 딕셔너리, 여기서 string은 경로(path)를 담을거고, AudioClip은 말그대로 클립을 담을것이다.

    // MP3 출력장치 => AudioSource
    // MP3 음원 => AudioClup
    // 관객(귀) => AudioListener
    
    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");
        if(root==null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);

            string[] soundNames = System.Enum.GetNames(typeof(Define.Sound)); //enum Sound에 들어있는 이름들을 배열에 담는다.
            for(int i=0;i<soundNames.Length-1;i++) //이름의 갯수만큼(MaxCount는 소스타입을 위한 이름이 아니니, 하나는 취급안한다 -1)
            {
                GameObject go = new GameObject { name = soundNames[i] }; //새로운 오브젝트go를 만들고 이름은 enum Sound에 있는 이름으로 지어준다.
                _audioSources[i] = go.AddComponent<AudioSource>(); //위에서 만든 오브젝트go에 audioSource 컴포넌트를 달아서, 얘네를 호출하기 쉽게, 기존에 준비해둔 _audioSources 배열에 연결한다.
                go.transform.parent = root.transform; //go와 root(@Sound)를 산하관계로 만들어줌.
            }

            _audioSources[(int)Define.Sound.Bgm].loop = true; //Bgm을 담당하는 소스는 loop를 true로
        }
    }

    public void Clear() //@Sound는 이제 씬이 넘어가도 삭제 되지않으니, 메모리 낭비되지않게, 다음씬에서 안쓰는건 싹다 날려줘야됨
    {
        foreach(AudioSource audioSource in _audioSources)
        {
            audioSource.Stop();
            audioSource.clip = null;
        }

        _audioClips.Clear();
    }


    public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        Play(audioClip, type, pitch); // Play내에서 다른 버전의Play 불러오기
    }
    public void Play(AudioClip audioClip, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f) //나중에 서버연동하면 path로 경로설정해주는게 편한데, 그때까지는 경로 계속 써주는게 힘드니, 직접 클립을 넣어주는 형식의 버전을 새로만들어두자.
    {
        if (audioClip == null)
            return;

        if (type == Define.Sound.Bgm)
        {
            AudioSource audioSource = _audioSources[(int)Define.Sound.Bgm];
            if (audioSource.isPlaying) //이미 다른 bgm이 돌고있다면 멈추게한뒤 틀어야지
                audioSource.Stop();

            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {
            AudioSource audioSource = _audioSources[(int)Define.Sound.Effect];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);

        }
    }

    AudioClip GetOrAddAudioClip(string path, Define.Sound type = Define.Sound.Effect) //매번 Managers.Resource.Load를 사용하면 메모리 부담되니, 이미 찾았던거는 담아뒀다가 다시 호출할땐 꺼내오게하는 함수
    {
        if (path.Contains("Sounds/") == false) //path는 언제나 Sounds/ 산하에 존재하는데, 만약 입력할때 써주는거 깜빡했다면 추가해주는코드
            path = $"Sounds/{path}";

        AudioClip audioClip = null;

        if (type == Define.Sound.Bgm)
        {
            AudioSource audioSource = _audioSources[(int)Define.Sound.Bgm];
        }
        else 
        {
            if (_audioClips.TryGetValue(path, out audioClip) == false)
            {
                audioClip = Managers.Resource.Load<AudioClip>(path);
                _audioClips.Add(path, audioClip);
            }
        }
        if (audioClip == null)
            Debug.Log($"AudioClip Missing ! {path}");

        return audioClip;
    }

}
