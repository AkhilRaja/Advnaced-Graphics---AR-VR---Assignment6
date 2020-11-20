using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceMultipleObjectsOnPlane : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject m_PlacedPrefab;

    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }

    /// <summary>
    /// Invoked whenever an object is placed in on a plane.
    /// </summary>
    public static event Action onPlacedObject;
    
    [Tooltip("This is the pointer gameobject")]
    public GameObject pointerObject;

    private bool m_Placed = false;
    
    [Tooltip("Turn off the ScreenUI on placing the object")]
    public GameObject ScreenUI;

    ARRaycastManager m_RaycastManager;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
        
    }

    void Update()
    {
        //This is my bracket pointer object
        if (m_RaycastManager.Raycast(new Vector3 (Screen.width * 0.5f, Screen.height * 0.5f, 0), s_Hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = s_Hits[0].pose;

            if (pointerObject != null)
            {
                pointerObject.transform.position = hitPose.position;
                pointerObject.transform.rotation = hitPose.rotation;

                //Modify this to only place the TV once in the scene
                if (Input.touchCount > 0 && !m_Placed)
                {
                    Touch touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Began)
                    {
                        spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, Quaternion.identity);
                        if (onPlacedObject != null)
                        {
                            ScreenUI.SetActive(false);
                            pointerObject.SetActive(false);
                            m_Placed = true;
                            onPlacedObject();
                        }
                    }
                }
            }
        }
    }
}
