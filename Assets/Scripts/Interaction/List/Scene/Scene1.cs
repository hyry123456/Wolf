using Interaction;
public class Scene1 : InteractionBase
{
    public override void InteractionBehavior()
    {
        UI.DumbShowManage.Instance.ShowDumbText(
            TextLoad.Instance.GetOneDumbText(0), () =>
            {
                ChangeScene.Instance.BeginChangeMap();
            }, new Common.INonReturnAndNonParam[30]);
    }
}
