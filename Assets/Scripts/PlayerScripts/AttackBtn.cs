using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBtn : MonoBehaviour
{
    public SignalSender attackBtnSignal;
    // Start is called before the first frame update
    public void AttackPress()
    {
        attackBtnSignal.Raise();
    }
}
