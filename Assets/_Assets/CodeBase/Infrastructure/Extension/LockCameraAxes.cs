using Cinemachine;
using Unity.Mathematics;
using UnityEngine;

namespace countMastersTest.infrastructure.extension
{
    [AddComponentMenu("")] // Hide in menu
    [ExecuteInEditMode] 
    [SaveDuringPlay]
    public class LockCameraAxes : CinemachineExtension
    {
        [Tooltip("Lock the camera's Axes position")]
        public bool3 _axis;
        public Vector3 _axisValue;

        protected override void PostPipelineStageCallback(
            CinemachineVirtualCameraBase vcam,
            CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            if (stage == CinemachineCore.Stage.Body)
            {
                var pos = state.RawPosition;
                pos.x = _axis.x ? _axisValue.x : pos.x;
                pos.y = _axis.y ? _axisValue.y : pos.y;
                pos.z = _axis.z ? _axisValue.z : pos.z;

                state.RawPosition = pos;
            }
        }
    }
}
