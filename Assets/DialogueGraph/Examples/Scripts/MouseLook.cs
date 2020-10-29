using System;
using UnityEngine;

[Serializable]
public class MouseLook : MonoBehaviour
{
    public float XSensitivity = 2f;
    public float YSensitivity = 2f;
    public bool clampVerticalRotation = true;
    public float MinimumX = -90F;
    public float MaximumX = 90F;
    public bool smooth;
    public float smoothTime = 18f;
    public bool lockCursor = true;

    [HideInInspector]
    public float xRot = 0f, yRot = 0f;

    public MouseLookMode lookMode = MouseLookMode.Smooth;

    private Quaternion m_CharacterTargetRot;
    private Quaternion m_CameraTargetRot;
    public Camera m_Camera;

    public bool canLook = false;

    public void Init(Transform character, Transform camera)
    {
        m_CharacterTargetRot = character.localRotation;
        m_CameraTargetRot = camera.localRotation;
    }
    private void Start()
    {
        Init(transform, m_Camera.transform);
    }
    private void Update()
    {
        LookRotation(transform, m_Camera.transform);

    }
    public void LookRotation(Transform character, Transform camera)
    {

        if (lookMode == MouseLookMode.Smooth)
        {
            yRot = Input.GetAxis("Mouse X") * XSensitivity;
            xRot = Input.GetAxis("Mouse Y") * YSensitivity;

            m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
            m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

            if (clampVerticalRotation)
                m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);

            character.localRotation = Quaternion.Slerp(character.localRotation, m_CharacterTargetRot,
                smoothTime * Time.deltaTime);
            camera.localRotation = Quaternion.Slerp(camera.localRotation, m_CameraTargetRot,
                smoothTime * Time.deltaTime);
        }
        else if (lookMode == MouseLookMode.Precise)
        {
            yRot = Input.GetAxis("Mouse X") * XSensitivity;
            xRot = Input.GetAxis("Mouse Y") * YSensitivity;

            m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
            m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

            if (clampVerticalRotation)
                m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);

            character.localRotation = m_CharacterTargetRot;
            camera.localRotation = m_CameraTargetRot;
        }
    }

    public void SetCursorLock(bool value)
    {
        lockCursor = value;
        if (!lockCursor)
        {//we force unlock the cursor if the user disable the cursor locking helper
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void SetLookMode(MouseLookMode value)
    {
        lookMode = value;
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

    public enum MouseLookMode
    {
        Smooth,
        Precise
    }
}
