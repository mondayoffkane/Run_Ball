using System.Collections;

namespace MondayOFF {

#if UNITY_ANDROID

    using System.Threading;
    using System.Threading.Tasks;
    using Google.Play.Review;

    public static class Review {
        private static ReviewManager _reviewManager = default;
        private static bool _isWorking = false;

        static Review() {
            _reviewManager = new ReviewManager();
        }

        public static void RequestReview() {
            // UnityMainThreadDispatcher.Instance().StartCoroutine(PrepareAndDisplay());
            if (_isWorking) {
                EverydayLogger.Warn("Review is already working");
                return;
            }
            ShowReview();
        }

        private static IEnumerator PrepareAndDisplay() {
            var requestFlowOperation = _reviewManager.RequestReviewFlow();
            yield return requestFlowOperation;
            if (requestFlowOperation.Error != ReviewErrorCode.NoError) {
                EverydayLogger.Error(requestFlowOperation.Error.ToString());
                yield break;
            }
            var _playReviewInfo = requestFlowOperation.GetResult();
            var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
            yield return launchFlowOperation;
            _playReviewInfo = null; // Reset the object
            if (launchFlowOperation.Error != ReviewErrorCode.NoError) {
                EverydayLogger.Error(requestFlowOperation.Error.ToString());
                yield break;
            }
        }

        private static async void ShowReview() {
            _isWorking = true;
            var requestFlowOperation = _reviewManager.RequestReviewFlow();
            await Task.Run(() => {
                while (!requestFlowOperation.IsDone) {
                    EverydayLogger.Info("Waiting for review flow operation to finish");
                    Thread.Sleep(200);
                }
            });
            // yield return requestFlowOperation;
            if (requestFlowOperation.Error != ReviewErrorCode.NoError) {
                EverydayLogger.Error(requestFlowOperation.Error.ToString());
                _isWorking = false;
                return;
            }
            var _playReviewInfo = requestFlowOperation.GetResult();
            var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
            await Task.Run(() => {
                while (!launchFlowOperation.IsDone) {
                    EverydayLogger.Info("Waiting for launch flow operation to finish");
                    Thread.Sleep(200);
                }
            });
            // yield return launchFlowOperation;
            _playReviewInfo = null; // Reset the object
            if (launchFlowOperation.Error != ReviewErrorCode.NoError) {
                EverydayLogger.Error(requestFlowOperation.Error.ToString());
                _isWorking = false;
                return;
            }
            _isWorking = false;
        }
    }

#elif UNITY_IOS

    public static class Review {
        public static void RequestReview() {
            UnityEngine.iOS.Device.RequestStoreReview();
        }
    }

#else

    public static class Review {
        public static void RequestReview() {
            EverydayLogger.Warn("Review is not supported on this platform");
        }
    }

#endif

}