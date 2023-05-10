using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace countMastersTest.infrastructure.constants
{
    public enum AnimationType
    {
        Idle,
        Run,
        Death,
        Attack
    }

    public static class AnimationParametersName
    {
        //Triggers
        public const string IDLE = "Idle";
        public const string DEATH = "Death";
        //Value
        public const string RUN = "MoveSpeed";
        public const string IS_ATTACK = "Attack";
    }
}
