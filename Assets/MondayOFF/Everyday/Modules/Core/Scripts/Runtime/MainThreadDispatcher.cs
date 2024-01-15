using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MondayOFF {
    [AddComponentMenu("")]
    public class MainThreadDispatcher : MonoBehaviour {
        private static MainThreadDispatcher _instance;
        public static MainThreadDispatcher Instance {
            get {
                if (_instance == null) {
                    _instance = new GameObject("MondayOFFMainThreadDispatcher").AddComponent<MainThreadDispatcher>();
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }
        private readonly Queue<System.Action> _commands = new Queue<System.Action>();

        public void Enqueue(System.Action action) {
            lock (_commands) {
                _commands.Enqueue(action);
            }
        }

        public void EnqueueCoroutine(IEnumerator action) {
            lock (_commands) {
                _commands.Enqueue(() => {
                    StartCoroutine(action);
                });
            }
        }

        private void Awake() {
            if (_instance != null) {
                Destroy(gameObject);
            }
        }

        private void Update() {
            lock (_commands) {
                while (_commands.Count > 0) {
                    _commands.Dequeue().Invoke();
                }
            }
        }

        private void OnDestroy() {
            if (_instance == this) {
                _instance = null;
            }
        }
    }
}