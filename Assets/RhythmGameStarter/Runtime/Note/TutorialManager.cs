using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RhythmGameStarter
{
    [DefaultExecutionOrder(-1)]
    public class TutorialManager : MonoBehaviour
    {
        private static TutorialManager m_Instance;
        public static TutorialManager Instance
        {
            get
            {
                return m_Instance;
            }
        }

        public GameObject go_TutPop;

        public Transform tf_1stClickFreeplay;
        public Transform tf_1stClickFreeplayTutorial;
        public Transform tf_1stClickFreeplayTutorialBtnPos;
        public Transform tf_Hand;

        public static string m_1stClickFreeplay = "m_1stClickFreeplay";
        public static string m_1stClickFreeplayTutorial = "m_1stClickFreeplayTutorial";

        private void Awake()
        {
            if (m_Instance != null)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                m_Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public void CheckTut(string _key, UnityAction _callback = null)
        {
            switch (_key)
            {
                case "m_1stClickFreeplay":
                    if (!PlayerPrefs.HasKey(_key))
                    {
                        go_TutPop.SetActive(true);
                        tf_Hand.position = tf_1stClickFreeplay.position;
                        if (_callback != null)
                        {
                            _callback();
                        }
                    }
                    break;
                case "m_1stClickFreeplayTutorial":
                    if (!PlayerPrefs.HasKey(_key))
                    {
                        go_TutPop.SetActive(true);
                        tf_Hand.position = tf_1stClickFreeplayTutorial.position;
                        if (_callback != null)
                        {
                            _callback();
                        }
                    }
                    break;
            }
        }

        public void SetTut(string _key, UnityAction _callback = null)
        {
            switch (_key)
            {
                case "m_1stClickFreeplay":
                    if (!PlayerPrefs.HasKey(_key))
                    {
                        PlayerPrefs.SetInt(m_1stClickFreeplay, 1);
                        go_TutPop.SetActive(false);
                        if (_callback != null)
                        {
                            _callback();
                        }
                    }
                    break;
                case "m_1stClickFreeplayTutorial":
                    if (!PlayerPrefs.HasKey(_key))
                    {
                        PlayerPrefs.SetInt(m_1stClickFreeplayTutorial, 1);
                        go_TutPop.SetActive(false);
                        if (_callback != null)
                        {
                            _callback();
                        }
                    }
                    break;
            }
        }
    }
}