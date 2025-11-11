using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace MQ3DVirtualHangout.VRInteraction
{
    /// <summary>
    /// Manages VR controller inputs and interactions for Meta Quest 3
    /// Handles button presses, triggers, and grip interactions
    /// </summary>
    public class VRControllerManager : MonoBehaviour
    {
        [Header("Controller References")]
        [SerializeField] private XRNode leftController = XRNode.LeftHand;
        [SerializeField] private XRNode rightController = XRNode.RightHand;

        [Header("Interaction Settings")]
        [SerializeField] private float raycastDistance = 10f;
        [SerializeField] private LayerMask interactionLayers;

        private InputDevice leftDevice;
        private InputDevice rightDevice;
        private bool isLeftTriggerPressed = false;
        private bool isRightTriggerPressed = false;

        // Events for other systems to subscribe to
        public event System.Action<Vector3> OnLeftTriggerPressed;
        public event System.Action<Vector3> OnRightTriggerPressed;
        public event System.Action OnLeftTriggerReleased;
        public event System.Action OnRightTriggerReleased;
        public event System.Action<float> OnLeftGripPressed;
        public event System.Action<float> OnRightGripPressed;

        private void Start()
        {
            InitializeControllers();
        }

        private void InitializeControllers()
        {
            leftDevice = InputDevices.GetDeviceAtXRNode(leftController);
            rightDevice = InputDevices.GetDeviceAtXRNode(rightController);

            if (!leftDevice.isValid || !rightDevice.isValid)
            {
                Debug.LogWarning("VR Controllers not detected. Attempting to connect...");
                InvokeRepeating(nameof(TryConnectControllers), 1f, 1f);
            }
            else
            {
                Debug.Log("VR Controllers connected successfully");
            }
        }

        private void TryConnectControllers()
        {
            if (!leftDevice.isValid)
            {
                leftDevice = InputDevices.GetDeviceAtXRNode(leftController);
            }

            if (!rightDevice.isValid)
            {
                rightDevice = InputDevices.GetDeviceAtXRNode(rightController);
            }

            if (leftDevice.isValid && rightDevice.isValid)
            {
                Debug.Log("VR Controllers connected");
                CancelInvoke(nameof(TryConnectControllers));
            }
        }

        private void Update()
        {
            if (!leftDevice.isValid || !rightDevice.isValid)
            {
                return;
            }

            HandleTriggerInputs();
            HandleGripInputs();
            HandleButtonInputs();
        }

        private void HandleTriggerInputs()
        {
            // Left trigger
            leftDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool leftTrigger);
            
            if (leftTrigger && !isLeftTriggerPressed)
            {
                isLeftTriggerPressed = true;
                Vector3 leftPosition = GetControllerPosition(leftController);
                OnLeftTriggerPressed?.Invoke(leftPosition);
            }
            else if (!leftTrigger && isLeftTriggerPressed)
            {
                isLeftTriggerPressed = false;
                OnLeftTriggerReleased?.Invoke();
            }

            // Right trigger
            rightDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool rightTrigger);
            
            if (rightTrigger && !isRightTriggerPressed)
            {
                isRightTriggerPressed = true;
                Vector3 rightPosition = GetControllerPosition(rightController);
                OnRightTriggerPressed?.Invoke(rightPosition);
            }
            else if (!rightTrigger && isRightTriggerPressed)
            {
                isRightTriggerPressed = false;
                OnRightTriggerReleased?.Invoke();
            }
        }

        private void HandleGripInputs()
        {
            // Left grip
            leftDevice.TryGetFeatureValue(CommonUsages.grip, out float leftGrip);
            if (leftGrip > 0.1f)
            {
                OnLeftGripPressed?.Invoke(leftGrip);
            }

            // Right grip
            rightDevice.TryGetFeatureValue(CommonUsages.grip, out float rightGrip);
            if (rightGrip > 0.1f)
            {
                OnRightGripPressed?.Invoke(rightGrip);
            }
        }

        private void HandleButtonInputs()
        {
            // Primary button (A/X)
            rightDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButton);
            leftDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonLeft);

            // Secondary button (B/Y)
            rightDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButton);
            leftDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButtonLeft);

            // Thumbstick
            rightDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 rightThumbstick);
            leftDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 leftThumbstick);
        }

        /// <summary>
        /// Gets the current position of a controller
        /// </summary>
        public Vector3 GetControllerPosition(XRNode node)
        {
            InputDevice device = (node == XRNode.LeftHand) ? leftDevice : rightDevice;
            
            if (device.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position))
            {
                return position;
            }

            return Vector3.zero;
        }

        /// <summary>
        /// Gets the current rotation of a controller
        /// </summary>
        public Quaternion GetControllerRotation(XRNode node)
        {
            InputDevice device = (node == XRNode.LeftHand) ? leftDevice : rightDevice;
            
            if (device.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation))
            {
                return rotation;
            }

            return Quaternion.identity;
        }

        /// <summary>
        /// Performs a raycast from the specified controller
        /// </summary>
        public bool ControllerRaycast(XRNode node, out RaycastHit hit)
        {
            Vector3 position = GetControllerPosition(node);
            Quaternion rotation = GetControllerRotation(node);
            Vector3 direction = rotation * Vector3.forward;

            return Physics.Raycast(position, direction, out hit, raycastDistance, interactionLayers);
        }

        /// <summary>
        /// Triggers haptic feedback on a controller
        /// </summary>
        public void TriggerHaptic(XRNode node, float amplitude = 0.5f, float duration = 0.1f)
        {
            InputDevice device = (node == XRNode.LeftHand) ? leftDevice : rightDevice;
            
            if (device.TryGetHapticCapabilities(out HapticCapabilities capabilities))
            {
                if (capabilities.supportsImpulse)
                {
                    device.SendHapticImpulse(0, amplitude, duration);
                }
            }
        }

        /// <summary>
        /// Gets the velocity of a controller
        /// </summary>
        public Vector3 GetControllerVelocity(XRNode node)
        {
            InputDevice device = (node == XRNode.LeftHand) ? leftDevice : rightDevice;
            
            if (device.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 velocity))
            {
                return velocity;
            }

            return Vector3.zero;
        }
    }
}
