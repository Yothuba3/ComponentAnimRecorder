using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Yothuba.MotionRecord
{


    [CustomEditor(typeof(MotionRecorderWrapper))]
    public class VMCMotionRecorderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if(EditorApplication.isPlaying)
            {
                MotionRecorderWrapper vmcmr = target as MotionRecorderWrapper;
                if (GUILayout.Button("変更の反映"))
                {
                    Debug.Log("ReSyncSetting");
                    vmcmr.ReSyncSetting();
                }
            }
        }
    }
}