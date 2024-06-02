using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Experiment_list : MonoBehaviour
{
    public TextAsset textJSON;
    public Button labButton;
    public Transform parent;

    [System.Serializable] public class Lab
    {
        public string labTitle;
        public string labPic;
        public string labJSON;
    }

    [System.Serializable] public class LabList
    {
        public Lab[] Labs;
    }

    public class ButtonContent : MonoBehaviour
    {
        public TextMeshProUGUI buttonText;
        public RawImage buttonImg;
    }

    public LabList labs = new LabList();

    // Start is called before the first frame update
    void Start()
    {
        labs = JsonUtility.FromJson<LabList>(textJSON.text);
        float height = -110f;
        float width = -748.5f;
        foreach(Lab lab in labs.Labs)
        {
            Button btn = Instantiate(labButton);
            btn.transform.parent = parent;
            btn.transform.position = new Vector3(width, height, 0);
            btn.GetComponent<RectTransform>().sizeDelta = new Vector2(326.7f, 60.4f);
            width += 392.51f;
            btn.onClick.AddListener(() => loadScene("WorkingPlace", lab.labJSON));
            TextMeshProUGUI buttonText = btn.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = lab.labTitle;
            buttonText.fontSize = 24;
            RawImage buttonImg = btn.GetComponentInChildren<RawImage>();
            byte[] imageBytes = File.ReadAllBytes(lab.labPic);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imageBytes);
            buttonImg.texture = tex;

        }
    }

    void loadScene(string sceneName, string json)
    {
        Circuit.labJSON = json;
        SceneManager.LoadScene(sceneName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
