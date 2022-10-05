
namespace Interaction
{
    public class ShowDumbText : InteractionBase
    {
        public int textIndex;
        public override void InteractionBehavior()
        {
            UI.DumbShowManage.Instance.ShowDumbText(
                TextLoad.Instance.GetOneDumbText(textIndex), null);
        }
    }
}