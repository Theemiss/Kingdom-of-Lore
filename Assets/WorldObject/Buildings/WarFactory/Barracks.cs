using UnityEngine;
using System.Collections;

public class Barracks : Building
{
    public Animator animator;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        actions = new string[] { "Footman", "Archer","Horsman","Knight" };
        Debug.Log(animator.GetBool("Spawning"));
    }
    public override void PerformAction(string actionToPerform)
    {
        animator.SetBool("Spawning", true);
        Debug.Log(animator.GetBool("Spawning"));
        base.PerformAction(actionToPerform);
        CreateUnit(actionToPerform);
        //animator.SetBool("Spawning", false);
        Debug.Log(animator.GetBool("Spawning"));



    }

}
