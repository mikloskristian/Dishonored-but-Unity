using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestInteractible : Interactible
{
    int _koinKaunter = 0;
    public override void OnFocus()
    {
        print("Looking at " + gameObject.name);
    }

    public override void OnInteract()
    {
        gameObject.transform.position += new Vector3(0, -200.0f, 0);
        _koinKaunter++;
        print("Sada imas " + _koinKaunter + "konsinsa. Bravo");
    }

    public override void OnLoseFocus()
    {
        print("Stopped looking at " + gameObject.name);
    }
}
