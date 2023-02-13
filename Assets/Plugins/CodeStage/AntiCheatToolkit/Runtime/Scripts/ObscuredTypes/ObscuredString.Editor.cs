#if UNITY_EDITOR
namespace CodeStage.AntiCheat.ObscuredTypes
{
    public sealed partial class ObscuredString
    {
        internal bool IsDataValid
        {
            get
            {
                if (!inited || !fakeValueActive) return true;
                return fakeValue == GetDecrypted();
            }
        }
    }
}
#endif