using System;
using UnityEngine;

namespace MondayOFF {
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    internal class DrawIfAttribute : PropertyAttribute {
        internal string comparedPropertyName { get; private set; }
        internal string displayName { get; private set; }

        internal DrawIfAttribute(string comparedPropertyName, string displayName = null) {
            this.comparedPropertyName = comparedPropertyName;
            this.displayName = displayName;
        }
    }
}