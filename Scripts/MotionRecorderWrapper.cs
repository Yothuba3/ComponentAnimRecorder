using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TypeInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Yothuba.MotionRecord
{
    public class MotionRecorderWrapper : MonoBehaviour
    {
        public GameObject character;
        public string takeName;
        [CanBeNull] public List<Component> recordTarget;

        private MotionRecordSetting _setting;
        private MotionRecorder _recorder;
        private void Start()
        {
             recordTarget = recordTarget.Select(x => x).Distinct().ToList();
             _setting = new MotionRecordSetting(character, takeName,recordTarget);
            _recorder = gameObject.AddComponent<MotionRecorder>();
            _recorder._setting = _setting;
        }

        public void StartRecord(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            _recorder.StartRecord();
        }

        public void StopRecord(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            _recorder.StopRecord();
        }

        public void ReSyncSetting()
        {
            recordTarget = recordTarget.Select(x => x).Distinct().ToList();
            _setting = new MotionRecordSetting(character, takeName,recordTarget);
            _recorder._setting = _setting;
        }
    }
    }


