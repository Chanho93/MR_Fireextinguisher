/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Vuforia;

namespace Vuforia
{
    /// <summary>
    /// A custom handler that implements the ITrackableEventHandler interface.
    /// </summary>
    public class DefaultTrackableEventHandler : MonoBehaviour,
                                                ITrackableEventHandler
    {
        #region PRIVATE_MEMBER_VARIABLES
        public GameObject blackShadow;
        private TrackableBehaviour mTrackableBehaviour;
        bool enable = false;
        private AudioSource audio_source;
        private Animator anim;
        private bool clouth = true;
        private Text NarrationText;
        private Text AriText;
        public AudioClip audio_clip1;
        public AudioClip audio_clip2;
        public GameObject Fire;
        public GameObject Ari;
        public GameObject[] Clouth;
        public GameObject Fire_Extin;
        public GameObject AriUI;
        public GameObject Narration;


        #endregion // PRIVATE_MEMBER_VARIABLES



        #region UNTIY_MONOBEHAVIOUR_METHODS

        void Start()
        {
            mTrackableBehaviour = GetComponent<TrackableBehaviour>();
            if (mTrackableBehaviour)
            {
                mTrackableBehaviour.RegisterTrackableEventHandler(this);
            }
            audio_source = GetComponent<AudioSource>();
            anim = Ari.GetComponent<Animator>();
            NarrationText = Narration.GetComponentInChildren<Text>();
            AriText = AriUI.GetComponentInChildren<Text>();
        }

        #endregion // UNTIY_MONOBEHAVIOUR_METHODS

        void Update()
        {
            if (clouth)
            {
                Clouth[0].SetActive(true);
                Clouth[1].SetActive(false);
            }
            else
            {
                Clouth[1].SetActive(true);
                Clouth[0].SetActive(false);
            }
        }

        #region PUBLIC_METHODS

        /// <summary>
        /// Implementation of the ITrackableEventHandler function called when the
        /// tracking state changes.
        /// </summary>
        public void OnTrackableStateChanged(
                                        TrackableBehaviour.Status previousStatus,
                                        TrackableBehaviour.Status newStatus)
        {
            if (newStatus == TrackableBehaviour.Status.DETECTED ||
                newStatus == TrackableBehaviour.Status.TRACKED ||
                newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {

                OnTrackingFound();
                if (enable == false)
                {
                    enable = true;
                    //이 구역에 만약 이미지타겟, 멀티타겟이 대상을 처음으로 찾았을 경우의 행동을 작성하시면 됩니다.
                    StartCoroutine(FireStart());
                }

            }
            else
            {
                OnTrackingLost();
            }
        }

        #endregion // PUBLIC_METHODS



        #region PRIVATE_METHODS

        private void OnTrackingFound()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            // Enable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = true;
            }

            // Enable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = true;
            }

            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
        }


        private void OnTrackingLost()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            // Disable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = false;
            }

            // Disable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = false;
            }

            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
        }

        #endregion // PRIVATE_METHODS
        IEnumerator FireStart()
        {
            AriUI.SetActive(true);
            AriText.DOText("빨래가 잘 안 마르네.", 3);
            yield return new WaitForSeconds(5.0f);
            AriText.text = "";
            AriText.DOText("라지에타 앞에 널어놔야겠다.", 3);
            anim.SetBool("StartAnim", true);
            yield return new WaitForSeconds(3.0f);
            clouth = false;
            Narration.SetActive(true);
            NarrationText.DOText("난로 앞에 옷을 널으면 불이 붙을 수 있어요.", 3);
            AriUI.SetActive(false);
            anim.SetBool("StartAnim", false);
            yield return new WaitForSeconds(5.0f);
            Narration.SetActive(false);
            anim.SetBool("GoBack", true);
            yield return new WaitForSeconds(2.0f);
            Ari.transform.DOMove(new Vector3(-20.0f, 0.5f, -0.5f), 3.0f);
            yield return new WaitForSeconds(3.0f);
            Ari.SetActive(false);
            Ari.transform.Rotate(new Vector3(Ari.transform.rotation.x, 180.0f, Ari.transform.rotation.z));
            yield return new WaitForSeconds(5.0f); // **초 후에 불 발화. Fire스크립트와 연계
            Fire.SetActive(true);

            audio_source.Play();
            StartCoroutine(narration_audio11());
        }

        IEnumerator narration_audio11()
        {
            yield return new WaitForSeconds(1.5f);
            audio_source.clip = audio_clip1;
            audio_source.Play();
            yield return new WaitForSeconds(7.5f);
            audio_source.clip = audio_clip2;
            audio_source.Play();
            yield return new WaitForSeconds(4.0f);
            Ari.SetActive(true);
            AriUI.SetActive(true);
            AriText.text = "";
            AriText.DOText("으악! 불이야!!", 3.0f);
            Fire_Extin.SetActive(true);
            anim.SetBool("Run", true);
            Ari.transform.DOMove(new Vector3(-5.0f, 0.5f, -0.5f), 1.0f);
            yield return new WaitForSeconds(1.0f);
            anim.SetBool("Fire", true);
            yield return new WaitForSeconds(2.0f);
            AriUI.SetActive(false);
            Ari.SetActive(false);
            Fire_Extin.SetActive(false);
        }

    }

}
