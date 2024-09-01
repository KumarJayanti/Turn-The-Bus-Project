using SpiceSharp;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Microammeter : CircuitComponent
{
    public double Indicator = 0;
    public float Scale = 1.0e6f;

    public string componentTitleString = "";
    public string componentDescriptionString = "";

    public override void InitSpiceEntity(string name, string[] interfaces, float[] parameters,string title,string description)
    {
        this.name = name;
        this.Interfaces = interfaces;
        this.Parameters = parameters;
        this.Scale = parameters[0];
        this.Title = title;
        this.Description = description;

        spiceEntitys = new List<SpiceSharp.Entities.IEntity>();
        spiceEntitys.Add(new SpiceSharp.Components.Resistor(name, interfaces[0], interfaces[1], parameters[1]));
    }

    public override void RegisterComponent(Circuit circuit)
    {
        base.RegisterComponent(circuit);

        gameObject.GetComponentInChildren<MicroammeterText>().InitAmmeterValue();
        var currentExport = new SpiceSharp.Simulations.RealPropertyExport(circuit.Sim, this.name, "i");
        circuit.Sim.ExportSimulationData += (sender, args) =>
        {
            this.Indicator = currentExport.Value;
            gameObject.GetComponentInChildren<MicroammeterText>().UpdateAmmeterValue(this.Indicator * this.Scale);
        };
    }
    private void OnMouseDown()
    {
        Circuit.isLabelWindowOpen = true;
        Circuit.componentTitle = Title;
        Circuit.componentDescription = Description;
        if (this.Indicator * this.Scale >= 100000 || this.Indicator * this.Scale <= -100000)
        {
            Circuit.componentValue = string.Format("{0:0.##}", this.Indicator * this.Scale / 1e6) + " A";
        }
        else if (this.Indicator * this.Scale >= 100 || this.Indicator * this.Scale <= -100)
        {
            Circuit.componentValue = string.Format("{0:0.##}", this.Indicator * this.Scale / 1e3) + " mA";
        }
        else
        {
            Circuit.componentValue = string.Format("{0:0.##}", this.Indicator * this.Scale) + " uA";
        }
    }
}
