using System.Collections.Generic;
using UnityEngine;
public class PlayerModel : MyMonobehaviour // gan vao model
{
    [SerializeField] protected List<SpriteRenderer> sr = new List<SpriteRenderer>();
    public List<SpriteRenderer> Sr => sr;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadModel();
    }
    protected override void Reset()
    {
        base.Reset();
    }
    protected virtual void LoadModel()
    {
        if (this.sr.Count != 0) return;
        SpriteRenderer[] newSr = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer child in newSr)
        {
            sr.Add(child);
        }
    }
}