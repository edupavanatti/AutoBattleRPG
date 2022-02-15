namespace AutoBattle
{
    public class Types
    {
        public struct CharacterClassSpecific
        {
            // Not being used for now.
            CharacterClass CharacterClass;
            float hpModifier;
            float ClassDamage;
            CharacterSkills[] skills;
        }

        public struct GridBox
        {
            public int xIndex;
            public int yIndex;
            public bool ocupied = false;
            public bool isOcupiedByPlayer = false;
            public int Index;

            public GridBox(int x, int y, int index)
            {
                xIndex = x;
                yIndex = y;
                Index = index;
            }
        }

        public struct CharacterSkills
        {
            // Not being used for now.
            string Name;
            float damage;
            float damageMultiplier;
        }

        // Added unique features to each character as my extra feature assignment as I was born in March.
        public enum CharacterClass : uint
        {
            // Has less health but no weaknesses.
            // Has 20% chance of blocking damage when attacked.
            Paladin = 1,

            // Strong against Archer; weak against Cleric.
            // Has 20% chance of attacking twice.
            Warrior = 2,

            // Strong against Warrior; weak against Archer.
            // Has 10% chance of healing himself when attacking.
            Cleric = 3,

            // Strong against Cleric; weak against Warrior.
            // Has 50% chance of attacking from distance.
            Archer = 4
        }
    }
}
