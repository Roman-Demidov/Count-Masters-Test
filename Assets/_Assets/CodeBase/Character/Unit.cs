using System;
using countMastersTest.constants;
using UnityEngine;

namespace countMastersTest.character
{
    public class Unit: MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float _radius;

        internal float getRadius() => _radius;

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
    }
}
