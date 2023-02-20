namespace MondayOFF {
    internal abstract class AdTypeBase : System.IDisposable {
        private protected readonly AdSettings _settings;
        internal abstract bool IsReady();
        internal abstract bool Show();
        public abstract void Dispose();
        internal AdTypeBase(in AdSettings settings){
            _settings = settings;
        }
    }

    internal abstract class FullscreenAdType : AdTypeBase {
        protected const int MaxRetryCount = 10;
        protected const int RetryInterval = 25;
        private protected int _retryAttempt = 0;
        internal static event System.Action OnBeforeShow = default;
        internal static event System.Action OnAfterShow = default;
        protected void CallOnBeforeShow() { OnBeforeShow?.Invoke(); }
        protected void CallOnAfterShow() { OnAfterShow?.Invoke(); }
        
        internal FullscreenAdType(in AdSettings settings): base(settings){
            
        }
    }
}