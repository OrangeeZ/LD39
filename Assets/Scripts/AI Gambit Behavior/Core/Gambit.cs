using System;
using UnityEngine;
using System.Collections;

namespace AI.Gambits
{
    public abstract class Gambit
    {
        protected readonly Character Character;

        protected Gambit(Character character)
        {
            Character = character;
        }

        public virtual bool Execute()
        {
            throw new NotImplementedException();
        }
    }

    public abstract class Gambit<T> : Gambit where T : GambitInfo
    {
        protected readonly T Info;

        protected Gambit(Character character, T info) : base(character)
        {
             Info = info;
        }
    }
}