using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interaction;

public class Scene3 : InteractionBase
{
    public override void InteractionBehavior()
    {
        EndUI.Instance.ShowEnd(TextLoad.Instance.GetOneDumbText(3));
    }

}
