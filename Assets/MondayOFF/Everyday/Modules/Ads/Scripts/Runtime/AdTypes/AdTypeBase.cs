namespace MondayOFF {
    internal abstract class AdTypeBase : System.IDisposable {
        internal abstract bool IsReady();
        internal abstract bool Show();
        public abstract void Dispose();
    }

    internal abstract class FullscreenAdType : AdTypeBase {
        protected const int MaxRetryCount = 3;
        protected const int RetryInterval = 25;
        private protected int _retryAttempt = 0;
    }
}