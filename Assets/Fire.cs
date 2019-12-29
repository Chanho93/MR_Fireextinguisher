using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables; //타임라인을 쓰기 위해 필요
using DG.Tweening; //나레이션 텍스트를 동작하기 위해 필요
using UnityEngine.SceneManagement;
using FrostweepGames.Plugins.GoogleCloud.SpeechRecognition; //Google Speech를 쓰기 위해 필요

public class Fire : MonoBehaviour
{
    private GCSpeechRecognition GC; //Google Speech
    private bool BoxBool = true;
    public string ResultText = "";
    public Text Rtext;
    private Text Atext;
    private AudioSource audio;
    private Animator Arianim;
    public ParticleSystem fire;
    public ParticleSystem grow;
    public ParticleSystem smoke;
    public GameObject SmallFire;
    public GameObject Ari;
    public GameObject AriUIImage;
    public GameObject Smartphone;
    public GameObject FireTruck;
    public PlayableDirector FadeOut; //타임라인을 넣는 공간
    public ParticleSystem WaterCannon;

    public AudioClip clip0;
    public AudioClip clip1;
    public AudioClip clip2;
    public AudioClip clip3;
    public AudioClip clip_narration;
    public AudioClip clip_narration2_1;
    public AudioClip clip_narration2;

    // Start is called before the first frame update
    void Start()
    {
        GC = GCSpeechRecognition.Instance;
        GC.RecognitionSuccessEvent += SuccessEventHandler;
        GC.NetworkRequestFailedEvent += FaildEventHandler;
        //음성인식을 하기위해 위 3개가 필요함

        audio = this.GetComponent<AudioSource>();
        audio.clip = clip0;
        fire.startSize = 0f;
        fire.startLifetime = 0f;
        smoke.startSize = 0f;
        smoke.startLifetime = 0f;
        Arianim = Ari.GetComponent<Animator>();
        Atext = AriUIImage.GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            StartCoroutine(LoadDevice("Vuforia", false));
        }
        if (SmallFire.activeSelf && BoxBool)
        {
            StartCoroutine(IncreaseFire());
            BoxBool = false;
            Rtext.text = "1단계 불";
        }

        if (ResultText.Equals("1단계"))
        {
            audio.clip = clip0;
            fire.startSize = 3f;
            fire.startLifetime = 3f;
            smoke.startSize = 8f;
            smoke.startLifetime = 8f;

            Rtext.text = "1단계 불";
            audio.PlayOneShot(audio.clip);
            ResultText = "";
        }
        else if (ResultText.Equals("2단계"))
        {
            audio.clip = clip1;
            fire.startSize = 5f;
            fire.startLifetime = 4f;
            smoke.startSize = 10f;
            smoke.startLifetime = 9f;

            Rtext.text = "2단계 불";
            audio.PlayOneShot(audio.clip);
            ResultText = "";
        }
        else if (ResultText.Equals("3단계"))
        {
            audio.clip = clip2;
            fire.startSize = 7f;
            fire.startLifetime = 5f;
            smoke.startSize = 12f;
            smoke.startLifetime = 10f;

            Rtext.text = "3단계 불";
            audio.PlayOneShot(audio.clip);
            ResultText = "";
        }
        else if (ResultText.Equals("4단계"))
        {
            SmallFire.GetComponent<BoxCollider>().enabled = false;
            audio.clip = clip3;
            fire.startSize = 9f;
            fire.startLifetime = 6f;
            smoke.startSize = 14f;
            smoke.startLifetime = 11f;

            Rtext.text = "4단계 불";
            audio.PlayOneShot(audio.clip);
            StartCoroutine(Narr());
            ResultText = "";
        }
        else if (ResultText.Equals("다시"))
        {
            SmallFire.SetActive(true);
            audio.clip = clip0;
            fire.startSize = 3f;
            fire.startLifetime = 3f;
            smoke.startSize = 8f;
            smoke.startLifetime = 8f;
            Rtext.text = "1단계 불";
            audio.clip = clip0;
            audio.PlayOneShot(audio.clip);
            ResultText = "";
        }
    }

    IEnumerator LoadDevice(string newDevice, bool enable) //만약 VR씬(화제 진압 씬)에서 일반 씬(메인 메뉴씬)으로 넘어갈때 VR이 유지된 상태로 넘어가게 됨
                                                          //이를 방지하기 위해 필요
    {
        //! 디바이스 불러오기
        UnityEngine.XR.XRSettings.LoadDeviceByName(newDevice);
        yield return null;
        UnityEngine.XR.XRSettings.enabled = enable;
        SceneManager.LoadScene("MainMenuUI"); //설정을 바꾼 후 씬 전환
    }

    IEnumerator IncreaseFire()
    {
        while (fire.startSize < 3.0f)
        {
            yield return new WaitForSeconds(0.2f);
            fire.startSize += 0.075f;
            fire.startLifetime += 0.075f;
            smoke.startSize += 0.2f;
            smoke.startLifetime += 0.2f;
        }
    }

    IEnumerator Narr()
    {
        yield return new WaitForSeconds(2.0f);
        audio.clip = clip_narration2;
        audio.PlayOneShot(audio.clip);
        yield return new WaitForSeconds(7.0f);
        Ari.transform.DOMove(new Vector3(-6.0f, -1.75f, -1.75f), 0.0f);
        Ari.SetActive(true);
        Arianim.SetBool("Call", true);
        yield return new WaitForSeconds(1.0f);
        Smartphone.SetActive(true);
        AriUIImage.SetActive(true);
        Atext.text = "";

        Atext.DOText("여기 제튼회사에 큰 불이 났어요!! 빨리 와주세요!!", 3);
        yield return new WaitForSeconds(4.0f);


        AriUIImage.SetActive(false);
        FireTruck.SetActive(true);
        FireTruck.transform.DOMove(new Vector3(16.0f, -1.75f, -1.75f), 3.0f);
        yield return new WaitForSeconds(3.0f);
        WaterCannon.Play();
        yield return new WaitForSeconds(3.0f);
        SmallFire.SetActive(false);
        FadeOut.Play();
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(LoadDevice("Vuforia", false)); //Player Setting에서 Vuforia를 비활성화 시킴으로써 VR모드가 풀리면서 메인화면으로 씬전환이 됨
                                                      //만약 자신이 (비)활성화 시키고자 하는 모드의 이름을 모를 경우 Debug로 UnityEngine.XR.XRSettings.loadedDeviceName를 확인가능
    }
    public void StartRecord()
    {
        GC.StartRecord(false); //녹음 시작 코드
    }

    public void EndRecord()
    {
        GC.StopRecord(); //녹음 종료 코드
    }

    private void FaildEventHandler(string obj, long requestIndex) //만약 음성인식을 실패한 경우
    {
        ResultText = "Record Faild";
    }

    private void SuccessEventHandler(RecognitionResponse obj, long requestIndex) //만약 음성인식을 성공한 경우
    {

        if (obj != null && obj.results.Length > 0)
        {
            ResultText = obj.results[0].alternatives[0].transcript; //녹음된 내용을 ResultText에 저장
            Debug.Log(obj.results[0].alternatives[0].transcript);
        }
        else
        {
            ResultText = "잘 못 들었어요. 천천히 말해주세요";
        }
    }

    private void OnDestroy() //녹음을 키고 앱을 종료할 경우 녹음이 종료되지 않고 앱이 종료됨으로 아래와 같이 이벤트헨들러를 지워줌
    {
        GC.RecognitionSuccessEvent -= SuccessEventHandler;
        GC.NetworkRequestFailedEvent -= FaildEventHandler;
    }

}
