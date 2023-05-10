using System;
using countMastersTest.infrastructure.constants;
using countMastersTest.interactiveObjects.obstacles;
using UnityEngine;

namespace countMastersTest.character
{
    public class Unit: MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public event Action<Unit> onUnitHitObstacle;

        public void playAnimation(AnimationType animationType)
        {
            if(animationType != AnimationType.Attack)
            {
                _animator.SetBool(AnimationParametersName.IS_ATTACK, false);
            }
            if (animationType != AnimationType.Run)
            {
                _animator.SetFloat(AnimationParametersName.RUN, 0);
            }

            switch (animationType)
            {
                case AnimationType.Idle: _animator.SetTrigger(AnimationParametersName.IDLE); return;
                case AnimationType.Run: _animator.SetFloat(AnimationParametersName.RUN, 1); return;
                case AnimationType.Death: _animator.SetTrigger(AnimationParametersName.DEATH); return;
                case AnimationType.Attack: _animator.SetBool(AnimationParametersName.IS_ATTACK, true); return;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out IObstacle obstacle))
            {
                onUnitHitObstacle?.Invoke(this);
            }
        }
    }
}
