using UnityEngine;

namespace MondayOFF {
    internal class LabelOverrideAttribute : PropertyAttribute {
        internal string displayName;
        internal LabelOverrideAttribute(string displayName) {
            this.displayName = displayName;
        }
    }
}