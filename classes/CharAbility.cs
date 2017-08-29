using System;
using System.Data.SQLite;

namespace AlphaCodex
{
    public class CharAbility : Ability
    {
        // Properties
        public short value { get; set; }

        //constructor
        public CharAbility(Ability ability) : base(ability)
        {

        }

        // Methods //
    }
}