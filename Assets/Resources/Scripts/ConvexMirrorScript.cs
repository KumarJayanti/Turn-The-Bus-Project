using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ConvexMirrorScript : MonoBehaviour
{
    public GameObject pencil; // The object needle
    public GameObject convexLens; // The convex lens
    public GameObject convexMirror; // The convex mirror
    public GameObject opticalCenter; // The optical center of the lens
    public GameObject principalAxis; // LineRenderer for principal axis
    public Slider mirrorPositionSlider; // Slider to adjust mirror position
    public TMP_Text finalImageText; // Text to display the final image information

    private GameObject imageNeedle; // The image needle
    private Vector3 initialLensPosition;
    private Vector3 initialMirrorPosition;

    private float objectDistanceFromLens = 4.0f; // Fixed object distance from the lens
    private float focalLengthLens = 2.0f; // Focal length of the lens
    private float focalLengthMirror = 1.0f; // Focal length of the convex mirror

    void Start()
    {
        // Verify if all required GameObjects are assigned
        if (pencil == null || convexLens == null || convexMirror == null || opticalCenter == null || principalAxis == null || mirrorPositionSlider == null || finalImageText == null)
        {
            Debug.LogError("One or more required GameObjects are not assigned in the Inspector.");
            return;
        }

        initialLensPosition = convexLens.transform.position;
        initialMirrorPosition = convexMirror.transform.position;

        // Initialize slider
        if (mirrorPositionSlider != null)
        {
            mirrorPositionSlider.onValueChanged.AddListener(OnMirrorPositionChanged);
            mirrorPositionSlider.minValue = 0;
            mirrorPositionSlider.maxValue = 3; // Adjust based on your scene setup
            mirrorPositionSlider.value = 1; // Set a default value
            Debug.Log("Mirror position from lens:" + mirrorPositionSlider);

            // Add event triggers for OnPointerDown and OnDrag
            AddEventTrigger(mirrorPositionSlider.gameObject, EventTriggerType.PointerDown, (eventData) => { Debug.Log("Mirror Position Slider Pointer Down"); });
            AddEventTrigger(mirrorPositionSlider.gameObject, EventTriggerType.Drag, (eventData) => { Debug.Log("Mirror Position Slider Dragging"); });
        }

        // Instantiate the image needle as a clone of the pencil
        imageNeedle = Instantiate(pencil);
        imageNeedle.name = "ImageNeedle";
        imageNeedle.GetComponent<SpriteRenderer>().color = pencil.GetComponent<SpriteRenderer>().color;

        // Position the optical center at the lens position
        if (opticalCenter != null)
        {
            opticalCenter.transform.position = initialLensPosition;
            //AddLabelToPoint(opticalCenter, "LabelC", "C");
        }

        // Draw the principal axis
        DrawPrincipalAxis();

        // Mark basic points on the principal axis
        MarkPrincipalAxisPoints();

        // Set initial positions
        OnMirrorPositionChanged(mirrorPositionSlider.value);
    }

    void OnMirrorPositionChanged(float value)
    {
        Debug.Log("Slider Value Changed: " + value);
        UpdateMirrorPosition(value);
        CalculateFinalImage();
    }

    void UpdateMirrorPosition(float value)
    {
        if (convexMirror == null) return;

        // Move the mirror along the principal axis (x-axis)
        convexMirror.transform.position = initialLensPosition + new Vector3(value, 0, 0);
        Debug.Log("Mirror Position Updated: " + convexMirror.transform.position);
    }

    void CalculateFinalImage()
    {
        if (pencil == null || convexLens == null || convexMirror == null || imageNeedle == null) return;

        // Position the pencil (object) at a fixed distance from the lens, just below the principal axis
        pencil.transform.position = initialLensPosition - new Vector3(objectDistanceFromLens, 0.5f, 0);

        // Calculate the intermediate image distance using the lens formula: 1/f = 1/do + 1/di
        float intermediateImageDistance = 1 / (1 / focalLengthLens - 1 / objectDistanceFromLens);

        // Calculate the position of the intermediate image
        Vector3 intermediateImagePosition = new Vector3(intermediateImageDistance, 0, 0);

        // Calculate the virtual object distance for the convex mirror (distance from the mirror to the intermediate image)
        float virtualObjectDistance = Vector3.Distance(convexMirror.transform.position, intermediateImagePosition);

        // Calculate the reflection image distance using the mirror formula: 1/f = 1/do + 1/di
        float reflectionImageDistance = 1 / (1 / focalLengthMirror - 1 / virtualObjectDistance);

        // Position the reflection image on the same side as the object
        Vector3 reflectionImagePosition = convexMirror.transform.position + new Vector3(reflectionImageDistance, 0, 0);
        float reflectionImageDistanceFromLens = Vector3.Distance(initialLensPosition, convexMirror.transform.position) + reflectionImageDistance;

        // Calculate the final image distance using the lens formula again, taking the reflection image as the object
        float finalImageDistance = 1 / (1 / focalLengthLens - 1 / reflectionImageDistanceFromLens);

        // Position the final image relative to the lens, just above the principal axis
        Vector3 finalImagePosition = -new Vector3(finalImageDistance, 0, 0);

        // Move the image needle to the final image position
        imageNeedle.transform.position = new Vector3(finalImagePosition.x, -pencil.transform.position.y - 0.2f, pencil.transform.position.z);

        // Display the final image information or indicate coincidence
        if (finalImageText != null)
        {
            if (Mathf.Abs(finalImageDistance - objectDistanceFromLens) < 0.1f) // Allowing a small margin for floating point precision
            {
                finalImageText.text = "The tips of Image and Object needle coincides\nFocal length of convex mirror is 1";
            }
            else
            {
                finalImageText.text = "Final Image Distance (v): " + (finalImageDistance * 5).ToString("F1");
            }
        }

        Debug.Log("Intermediate Image Position: " + intermediateImagePosition);
        Debug.Log("Reflection Image Position: " + reflectionImagePosition);
        Debug.Log("Final Image Distance: " + finalImageDistance);
        Debug.Log("Final Image Position: " + finalImagePosition);
        Debug.Log("reflectionImageDistanceFromLens " + reflectionImageDistanceFromLens);
    }

    void DrawPrincipalAxis()
    {
        if (principalAxis == null) return;

        LineRenderer lr = principalAxis.GetComponent<LineRenderer>();
        if (lr != null)
        {
            lr.positionCount = 2;
            lr.SetPosition(0, new Vector3(-20, 0, 0)); // Start point of the line
            lr.SetPosition(1, new Vector3(20, 0, 0)); // End point of the line
            lr.startWidth = 0.1f;
            lr.endWidth = 0.1f;
        }
    }

    void MarkPrincipalAxisPoints()
    {
        AddLabelToPoint(new GameObject("F1"), "LabelF1", "F1", initialLensPosition + new Vector3(focalLengthLens, 0, 0));
        AddLabelToPoint(new GameObject("2F1"), "Label2F1", "2F1", initialLensPosition + new Vector3(2 * focalLengthLens, 0, 0));
        AddLabelToPoint(new GameObject("F2"), "LabelF2", "F2", initialLensPosition - new Vector3(focalLengthLens, 0, 0));
        AddLabelToPoint(new GameObject(" 2F2"), "Label 2F2", " 2F2", initialLensPosition - new Vector3(2 * focalLengthLens, 0, 0));
        AddLabelToPoint(new GameObject("O"), "LabelO", "O", initialLensPosition);
        AddLabelToPoint(pencil, "LabelAB", "AB", pencil.transform.position);
        AddLabelToPoint(imageNeedle, "LabelA'B'", "A'B'", imageNeedle.transform.position);
    }

    void AddEventTrigger(GameObject obj, EventTriggerType type, UnityEngine.Events.UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        if (trigger == null) trigger = obj.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = type };
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }

    void AddLabelToPoint(GameObject point, string labelName, string labelText, Vector3 position)
    {
        if (point == null) return;

        point.transform.position = position;

        GameObject labelObject = new GameObject(labelName);
        labelObject.transform.SetParent(point.transform);
        labelObject.transform.localPosition = new Vector3(0, -0.3f, 0);

        TextMeshPro label = labelObject.AddComponent<TextMeshPro>();
        label.fontStyle = FontStyles.Bold;
        label.text = labelText;
        label.fontSize = 4;
        label.color = Color.white;
        label.alignment = TextAlignmentOptions.Center;
    }
}