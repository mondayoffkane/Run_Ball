using UnityEngine;

namespace MondayOFF {
    internal partial class EverydaySettings : ScriptableObject {
        [Header("Enable verbose logging")]
        [SerializeField] internal bool verboseLogging = false;
        [Header("Ad Settings")]
        [SerializeField] internal AdSettings adSettings = default;
    }
}