public class GameScene : BaseScene
{
    protected override void Init()
    {
        SceneType = Define.Scene.Game;
        Managers.UI.ShowSceneUI<UI_GameScene>();
        Managers.GameInit();
    }

    public override void Clear()
    {

    }
}
