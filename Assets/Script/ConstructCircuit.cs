/*using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using WireBuilder;

public class ConstructCircuit : MonoBehaviour
{
    [System.Serializable] public class ComponentMeta
    {
        public string Name;
        public string Type;
        public string Title;
        public string Description;
        public float[] Position;
        public string[] Interfaces;
        public float[] Parameters;
    }
    [System.Serializable] public class ComponentMetaList
    {
        public ComponentMeta[] Components;
    }
    public class LabInfo
    {
        public string LabTitle;
        public string Aim;
        public string Background;
        public string Theory;
        public string Diagram;
    }
    [System.Serializable] public class LabMaterials
    {
        public string[] MaterialsRequired;
    }
    [System.Serializable] public class LabObservations
    {
        public string[] Observations;
    }

    public static string labJSON;

    public TextMeshPro labTitleField;
    public TextMeshPro componentTitleField;
    public TextMeshPro componentDescriptionField;

    public static string componentTitle = "";
    public static string componentDescription = "";
    public List<CircuitComponent> circuitComponents;

    public SpiceSharp.Circuit Ckt;
    public SpiceSharp.Simulations.BiasingSimulation Sim;

    public ComponentMetaList componentMetaList = new ComponentMetaList();

    public const string PREFAB_PATH = "Prefabs/";
    public static bool isLabelWindowOpen = false;

    private List<Wire> wires = new List<Wire>();

    // Start is called before the first frame update
    void Start()
    {
        TextAsset textJSON = Resources.Load<TextAsset>(labJSON);
        circuitComponents = new List<CircuitComponent>();
        componentMetaList = JsonUtility.FromJson<ComponentMetaList>(textJSON.text);
        InitUIWidgets(textJSON);
        InitCircuit();
        RunCircuit();
    }

    // Update is called once per frame
    void Update()
    {
        updateLabelInfo(); 
    }

    public void InitUIWidgets(TextAsset textJSON)
    {
        LabInfo labeInfoJSON = JsonUtility.FromJson<LabInfo>(textJSON.text);
        labTitleField.SetText(labeInfoJSON.LabTitle);
    }

    private void updateLabelInfo()
    {
        if (isLabelWindowOpen)
        {
            componentTitleField.SetText(componentTitle);
            componentDescriptionField.SetText(componentDescription);
            componentTitleField.gameObject.SetActive(true);
            componentDescriptionField.gameObject.SetActive(true);
        }
        else
        {
            componentTitleField.gameObject.SetActive(false);
            componentDescriptionField.gameObject.SetActive(false);
        }
    }

    public void InitCircuit()
    {
        Ckt = new SpiceSharp.Circuit();
        Sim = new SpiceSharp.Simulations.OP("Sim");
        foreach(ComponentMeta meta in componentMetaList.Components)
        {
            string prefabPath = PREFAB_PATH + meta.Type;
            Debug.Log(prefabPath);
            GameObject prefabObject = Resources.Load<GameObject>(prefabPath);
            Debug.Log(prefabObject);
            var instance = Instantiate(prefabObject, this.transform, true);
            instance.name = meta.Name;

            componentTitleField.SetText(meta.Name);
            componentDescriptionField.SetText(meta.Description);

            instance.transform.position = new Vector3(meta.Position[0], meta.Position[1], meta.Position[2]);

            CircuitComponent thisComponent = instance.GetComponent<CircuitComponent>();
            thisComponent.InitSpiceEntity(meta.Name,meta.Interfaces,meta.Parameters,meta.Title,meta.Description);

            circuitComponents.Add(thisComponent);
            thisComponent.RegisterComponent(this);

            thisComponent.InitInterfaces(meta.Interfaces);
        }
    }

    public void GenerateWires()
    {
        Dictionary<string, List<WireConnector>> interfaces = new Dictionary<string, List<WireConnector>>();
        foreach(CircuitComponent thisComponent in circuitComponents)
        {
            for(int i = 0; i < thisComponent.connectors.Count; i++)
            {
                string interfaceName = thisComponent.Interfaces[i];
                WireConnector connector = thisComponent.connectors[i];
                if (!interfaces.ContainsKey(interfaceName)) interfaces.Add(interfaceName, new List<WireConnector>());
                interfaces[interfaceName].Add(connector);
            }
        }
        foreach(var item in interfaces)
        {
            for(int i = 1; i < item.Value.Count; i++)
            {
                Wire wire = WireManager.CreateWireObject(item.Value[i - 1], item.Value[i - 1], item.Value[i].wireType);
                wire.transform.SetParent(this.transform);
                wires.Add(wire);
            }
        }
    }

    public void DestroyWires()
    {
        foreach(Wire wire in wires)
        {
            WireManager.DestroyWire(wire);
        }
        wires = new List<Wire>();
    }
    public void RunCircuit()
    {
        Sim.Run(Ckt);
    }
}
*/