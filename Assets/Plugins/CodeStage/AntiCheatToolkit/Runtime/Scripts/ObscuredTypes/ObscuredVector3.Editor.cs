﻿#if UNITY_EDITOR
namespace CodeStage.AntiCheat.ObscuredTypes
{
    public partial struct ObscuredVector3
    {
        internal bool IsDataValid
        {
            get
            {
                if (!inited || !fakeValueActive) return true;
                return Compare(fakeValue, InternalDecrypt());
            }
        }
    }
}
#endif