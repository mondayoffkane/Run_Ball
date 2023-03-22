using UnityEngine;

namespace MondayOFF {
    internal partial class EverydaySettings : ScriptableObject {
        [Header("Enable verbose logging")]
        [SerializeField] internal bool verboseLogging = true;
        [Header("Ad Settings")]
        [SerializeField] internal AdSettings adSettings = default;
    }
}