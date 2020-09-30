using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// A component that can be used to access the most
/// recently received light estimation information
/// for the physical environment as observed by an
/// AR device.
/// </summary>
[RequireComponent(typeof(Light))]
public class LightEstimation : MonoBehaviour
{
    [FormerlySerializedAs("m_CameraManager")]
    [SerializeField]
    [Tooltip("The ARCameraManager which will produce frame events containing light estimation information.")]
    ARCameraManager mCameraManager;

    /// <summary>
    /// Get or set the <c>ARCameraManager</c>.
    /// </summary>
    public ARCameraManager CameraManager
    {
        get => mCameraManager;
        set
        {
            if (mCameraManager == value)
                return;

            if (mCameraManager != null)
                mCameraManager.frameReceived -= FrameChanged;

            mCameraManager = value;

            if (mCameraManager != null & enabled)
                mCameraManager.frameReceived += FrameChanged;
        }
    }

    /// <summary>
    /// The estimated brightness of the physical environment, if available.
    /// </summary>
    public float? Brightness { get; private set; }

    /// <summary>
    /// The estimated color temperature of the physical environment, if available.
    /// </summary>
    public float? ColorTemperature { get; private set; }

    /// <summary>
    /// The estimated color correction value of the physical environment, if available.
    /// </summary>
    public Color? ColorCorrection { get; private set; }

    private void Awake ()
    {
        _mLight = GetComponent<Light>();
    }

    public void OnEnable()
    {
        if (mCameraManager != null)
            mCameraManager.frameReceived += FrameChanged;
    }

    public void OnDisable()
    {
        if (mCameraManager != null)
            mCameraManager.frameReceived -= FrameChanged;
    }

    public void FrameChanged(ARCameraFrameEventArgs args)
    {
        if (args.lightEstimation.averageBrightness.HasValue)
        {
            Brightness = args.lightEstimation.averageBrightness.Value;
            _mLight.intensity = Brightness.Value;
        }

        if (args.lightEstimation.averageColorTemperature.HasValue)
        {
            ColorTemperature = args.lightEstimation.averageColorTemperature.Value;
            _mLight.colorTemperature = ColorTemperature.Value;
        }
        
        if (args.lightEstimation.colorCorrection.HasValue)
        {
            ColorCorrection = args.lightEstimation.colorCorrection.Value;
            _mLight.color = ColorCorrection.Value;
        }
    }

    private Light _mLight;
}
