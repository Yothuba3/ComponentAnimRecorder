using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Yothuba.MotionRecord
{
    
    public class MotionRecordSetting
    {
        public MotionRecordSetting(GameObject character,
            string takeName,
            List<Component> type
        )
        {
            _character = character;
            _takeName = takeName;
            _type = type;
        }

        public GameObject Character => _character;
        public string TakeName => _takeName;
        [CanBeNull] public List<Component> ComponentType => _type;
        private GameObject _character;
        private string _takeName;
        [CanBeNull] private List<Component> _type;
    }

    public class MotionRecorder : MonoBehaviour 
    {
        public MotionRecordSetting _setting;
        private AnimationClip clip;
        private GameObjectRecorder _recorder;
        private bool _isRecording = false;

        private int _takeNumber = 0;
        private string _tmpTakeName;
        void Init()
        {
            clip = new AnimationClip();
        }

        void Start()
        {
            
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (clip == null) return;
            
            if (_isRecording)
            {
                _recorder.TakeSnapshot(Time.deltaTime);
            }
        }

        private void OnDisable()
        {
            if (clip == null) return;

            if (_recorder.isRecording)
            {
                _recorder.SaveToClip(clip);
                _isRecording = false;
            }
        }



        public void StartRecord()
        {
            if (_isRecording) return;
            Debug.Log("Rec Start");
            Debug.Log(_setting.ComponentType.GetType());
            Init();
            _recorder = new GameObjectRecorder(_setting.Character);
            
            if (_setting.ComponentType.Count() == 0)
            {
                Debug.Log("Cant Record.  Target is null");
                //_recorder.BindAll(_setting.Character, true);
                
            }
            else
            {
                
                foreach (var component in _setting.ComponentType)
                {
                    _recorder.BindComponentsOfType(_setting.Character, component.GetType(), true);
                }
            }

            _isRecording = true;

        }

        public void StopRecord()
        {
            if (clip == null)
            {
                return;
            }

            if (_recorder.isRecording)
            {
                Debug.Log(_recorder.isRecording);
                if (clip == null)
                {
                    Debug.Log("clip is null");
                    return;
                }

                if (_recorder.isRecording)
                {
                    _recorder.SaveToClip(clip);
                    _isRecording = false;
                    SaveAnimation();
                    _recorder.ResetRecording();
                    return;
                }
            }
        }

       
        private void SaveAnimation()
        {
            if (clip != null && _recorder.isRecording)
            {
                if (_tmpTakeName == null) _tmpTakeName = _setting.TakeName;
                else if(_tmpTakeName == _setting.TakeName) _takeNumber++;
                else if (_tmpTakeName != _setting.TakeName) _takeNumber = 0;
                
                var date = DateTime.Now.ToString("-yyyy-MM-dd--HH-mm-ss");
                var endPath = _setting.TakeName+_takeNumber + date + ".anim";
                var fullPath = "Assets/RecordAnim/" + endPath;
                Debug.Log("trying write to " + fullPath);

                
                AssetDatabase.CreateAsset(clip, AssetDatabase.GenerateUniqueAssetPath(fullPath));
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Debug.Log("Done : " + fullPath);
            }
        }
    }
}