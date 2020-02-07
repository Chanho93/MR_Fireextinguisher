# MR_Fireextinguisher
 
혼합현실을 이용한 소화기 분사 가상체험 


1. 불 생성을 위한 이미지 타켓

![image](https://user-images.githubusercontent.com/48191157/71572765-13400080-2b24-11ea-97cd-47fc17e3e85f.png)

2. 마그네틱 스위치를 이용한 음성인식(불 크기 조절)

![image](https://user-images.githubusercontent.com/48191157/71572810-42567200-2b24-11ea-8ad8-cc4f9fa7e4ba.png)


    public class CardboardTriggerControlMono : MonoBehaviour
    {
    public bool magnetDetectionEnabled = true;
    public Transform vrCamera;    
    public float toggleAngle = 10.0f;
    public float RightAngle = 165.0f;
    public float LeftAngle = 10.0f;
    public float speed = 6.0f;
    public static bool moveforward;
    private CharacterController cc; //기본적으로 제공되는 character controller
    private bool StartRecord = true;
    public Fire fireScript;
    public GameObject REC;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        CardboardMagnetSensor.SetEnabled(magnetDetectionEnabled);
        // Disable screen dimming:
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void Update()
    {
        if (!magnetDetectionEnabled) return;
        if (CardboardMagnetSensor.CheckIfWasClicked())
        {
           
            CardboardMagnetSensor.ResetClick();
            //moveforward = true;     
            //이 주석 밑으로 다음 주석까지의 구역이 마스네틱을 움직였을때의 행동을 처리함.
            if (StartRecord)
            {
                fireScript.StartRecord();
                REC.SetActive(true);
                StartRecord = false;
            }
            else
            {
                fireScript.EndRecord();
                REC.SetActive(false);
                StartRecord = true;
            }      
        }
        else if (!CardboardMagnetSensor.CheckIfWasClicked())
        {
            moveforward = false;
        }
        else
        {
            moveforward = false;
        }
        MoveTg();
    }   
    }

3. 녹음 표시

![image](https://user-images.githubusercontent.com/48191157/71572819-46828f80-2b24-11ea-8680-2e4bde806056.png)

4. 1단계 불

![image](https://user-images.githubusercontent.com/48191157/71572821-497d8000-2b24-11ea-804f-19af9f26517f.png)

   
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

        Atext.DOText("여기 회사에 큰 불이 났어요!! 빨리 와주세요!!", 3);
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

5. 2단계 불

![image](https://user-images.githubusercontent.com/48191157/71572823-4d110700-2b24-11ea-9690-da8416143a0a.png)

6. 3단계 불

![image](https://user-images.githubusercontent.com/48191157/71572829-50a48e00-2b24-11ea-9c39-6ca44340d0e1.png)

7. 소화기 생성을 위한 멀티타겟 이미지

![image](https://user-images.githubusercontent.com/48191157/71572863-75006a80-2b24-11ea-9407-47563de681e5.png)

8. 소화기 분사

![image](https://user-images.githubusercontent.com/48191157/71572893-919ca280-2b24-11ea-952c-30351e320286.png)

9. 불 끄는 장면

![image](https://user-images.githubusercontent.com/48191157/71572917-ad07ad80-2b24-11ea-9323-17ee4c737fbd.png)

10. 불꺼짐

![image](https://user-images.githubusercontent.com/48191157/71572940-ca3c7c00-2b24-11ea-9caf-389eaf32d639.png)
