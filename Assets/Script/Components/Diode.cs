using SpiceSharp.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diode : CircuitComponent
{
    public override void InitSpiceEntity(string name, string[] interfaces, float[] parameters, string title, string description)
    {
        this.Name = name;
        this.Interfaces = interfaces;
        this.Parameters = parameters;
        this.Title = title;
        this.Description = description;

        var model = new DiodeModel("test");
        model.SetParameter("is", 7.69e-11);
        model.SetParameter("rs", 4.2e-2);
        model.SetParameter("bv", 1.0e3);
        model.SetParameter("ibv", 5.0e-6);
        model.SetParameter("cjo", 2.65e-11);
        model.SetParameter("m", 3.33e-1);
        model.SetParameter("n", 1.45e0);
        model.SetParameter("tt", 4.32e-6);

        spiceEntitys = new List<SpiceSharp.Entities.IEntity>();
        spiceEntitys.Add(new SpiceSharp.Components.DiodeModel(model.Name));
        spiceEntitys.Add(new SpiceSharp.Components.Diode(name, interfaces[0], interfaces[1], "test"));
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Circuit.isLabelWindowOpen = true;
        Circuit.componentTitle = Title;
        Circuit.componentDescription = Description;
        Circuit.componentValue = "1N4007 DIODE";
    }
}
