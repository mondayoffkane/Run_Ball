
#if UNITY_EDITOR
using UnityEditor;
using System.Collections;


public class PlayOnEditorCoroutinesManager
{
    readonly IEnumerator mRoutine;

    public static PlayOnEditorCoroutinesManager StartEditorCoroutine( IEnumerator routine)
    {
        PlayOnEditorCoroutinesManager coroutine = new PlayOnEditorCoroutinesManager(routine);
        coroutine.start();
        return coroutine;
    }

    PlayOnEditorCoroutinesManager(IEnumerator routine)
    {
        mRoutine = routine;
    }

    void start()
    {
        EditorApplication.update += update;
    }

    void update()
    {
        if(!mRoutine.MoveNext())
        {
            StopEditorCoroutine();
        }
    }

    public void StopEditorCoroutine()
    {
        EditorApplication.update -= update;
    }
}
#endif
