using UnityEngine;

namespace MondayOFF {
    public class Rotate : MonoBehaviour {
        private void Update() {
            this.transform.rotation = Quaternion.Euler(0f, Mathf.Sin(Time.timeSinceLevelLoad) * 60f, 0f);
        }
    }
}