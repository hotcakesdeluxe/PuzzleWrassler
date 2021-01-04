using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PHL.Common.Utility;
using TMPro;

namespace PHL.Common.Input
{
    public class DeviceEvent : SecureEvent<InputDeviceData> { }

    [CreateAssetMenu(fileName = "InputDevice", menuName = "PHL/Input Device Data", order = 0)]
    public class InputDeviceData : ScriptableObject
    {
        [System.Serializable]
        public struct DeviceButtonIcon
        {
            public string id;
            public Sprite sprite;
            public string tmpSpriteName;
        }

        [SerializeField] private TMP_SpriteAsset _tmpSpriteAsset;
        [SerializeField] private List<DeviceButtonIcon> _deviceButtonIcons;

        public TMP_SpriteAsset tmpSpriteAsset
        {
            get
            {
                return _tmpSpriteAsset;
            }
        }

        public Sprite GetDeviceButtonIcon(string id)
        {
            return _deviceButtonIcons.Find(x => x.id == id).sprite;
        }

        public string GetTMPSpriteName(string id)
        {
            return _deviceButtonIcons.Find(x => x.id == id).tmpSpriteName;
        }

#if UNITY_EDITOR
        [ContextMenu("Copy sprite names")]
        private void CopySpriteNames()
        {
            for(int i = 0; i < _deviceButtonIcons.Count; i++)
            {
                DeviceButtonIcon temp = _deviceButtonIcons[i];

                if(temp.sprite != null)
                {
                    temp.tmpSpriteName = temp.sprite.name;
                }

                _deviceButtonIcons[i] = temp;
            }

            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
        }
#endif
    }
}