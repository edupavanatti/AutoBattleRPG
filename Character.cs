using System;
using static AutoBattle.Types;

namespace AutoBattle
{
    public class Character
    {
        public string Name { get; set; }
        public int Health;
        public int BaseDamage;
        string playerHealth = "";
        public int DamageMultiplier { get; set; }
        public GridBox currentBox;
        public int PlayerIndex;
        public Character Target { get; set; }
        public bool isPlayer;
        // Changed the battlefield reference.
        public Grid battlefield;
        bool isWeakness;
        int attackingDistance;
        CharacterClass characterClass;
        
        public Character(CharacterClass characterClass)
        {
            this.characterClass = characterClass;
            BaseDamage = 2;
            DamageMultiplier = 2;

            // If the character is a Paladin, it should have lower health.
            Health = characterClass == CharacterClass.Paladin ? 8 : 10;

            // If the character is an Archaer, it can attack from a longer distance.
            attackingDistance = characterClass == CharacterClass.Archer ? 3 : 1;
        }

        // Changed this method to also update the health bar for each character.
        public bool TakeDamage(int amount)
        {
            // Doubles the damage received if the character is weak against its target.
            if (isWeakness)
            {
                amount *= DamageMultiplier;
            }

            // The Paladin has a 20% chance of blocking the incoming attack.
            if (characterClass == CharacterClass.Paladin)
            {
                var blockingChance = Helper.GetRandomInt(0, 6);

                if (blockingChance == 0)
                {
                    amount = 0;
                    Console.WriteLine($"{Name} blocked the incoming damage!");
                }
            }

            UpdateHealthBar(amount);
            Health -= amount;

            if (Health <= 0)
            {
                Die();
                battlefield.drawBattlefield(true);
                return true;
            }
            else
            {
                if (amount > 0)
                {
                    Console.WriteLine($"{Target.Name} attacked {Name} and did {amount} damage!");

                    if (isWeakness)
                    {
                        Console.WriteLine($"The attack was super effective!");
                    }

                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine($"{Target.Name}'s attack missed and did no damage!\n");
                }
            }

            battlefield.drawBattlefield(false);
            return false;
        }

        //
        public void Die()
        {
            // Removes the dead character from the battlefied.
            battlefield.grids[currentBox.Index] = currentBox;
            currentBox.ocupied = false;
            battlefield.grids[currentBox.Index] = currentBox;

            // Notifies who died and who won the match.
            Console.WriteLine($"{Name} has died!");
            Console.WriteLine($"{Target.Name} was victorious!\n");
        }

        /// <summary>
        /// Moves the character to a specific GridBox within the battlefied.
        /// </summary>
        /// <param name="gridToMove">The GridBox to which the character should move.</param>
        /// <param name="direction">The direction the character is moving.</param>
        public void MoveToGridBox(GridBox gridToMove, string direction)
        {
            currentBox.ocupied = false;
            battlefield.grids[currentBox.Index] = currentBox;
            currentBox = gridToMove;
            currentBox.ocupied = true;
            currentBox.isOcupiedByPlayer = isPlayer;
            battlefield.grids[currentBox.Index] = currentBox;
            Console.WriteLine($"{Name} walked {direction}.\n");
        }

        public void StartTurn()
        {
            // Clears the console every time a new round starts.
            Console.Clear();

            if (CheckCloseTargets(battlefield)) 
            {
                // If the target character is within range of attack, the current character attacks.
                Attack(Target);
            }
            else
            {   // If there is no target close enough, calculates in wich direction this character should move to be closer to a possible target.
                // Refactored a bit this method and fixed the conditions to move to each direction.
                if (currentBox.xIndex > Target.currentBox.xIndex)
                {
                    if (battlefield.grids.Exists(gridBox => gridBox.Index == currentBox.Index - 1))
                    {
                        var gridToMove = battlefield.grids.Find(gridBox => gridBox.Index == currentBox.Index - 1);
                        MoveToGridBox(gridToMove, "left");
                    }
                }
                else if (currentBox.xIndex < Target.currentBox.xIndex)
                {
                    if (battlefield.grids.Exists(gridBox => gridBox.Index == currentBox.Index + 1))
                    {
                        var gridToMove = battlefield.grids.Find(gridBox => gridBox.Index == currentBox.Index + 1);
                        MoveToGridBox(gridToMove, "right");
                    }
                }
                else if (currentBox.yIndex > Target.currentBox.yIndex)
                {
                    if (battlefield.grids.Exists(gridBox => gridBox.Index == currentBox.Index - battlefield.yLength))
                    {
                        var gridToMove = battlefield.grids.Find(gridBox => gridBox.Index == currentBox.Index - battlefield.yLength);
                        MoveToGridBox(gridToMove, "up");
                    }
                }
                else if (currentBox.yIndex < Target.currentBox.yIndex)
                {
                    if (battlefield.grids.Exists(gridBox => gridBox.Index == currentBox.Index + battlefield.yLength))
                    {
                        var gridToMove = battlefield.grids.Find(gridBox => gridBox.Index == currentBox.Index + battlefield.yLength);
                        MoveToGridBox(gridToMove, "down");
                    }
                }

                battlefield.drawBattlefield(true);
            }

            PrintCharactersInfo();
        }

        // Check in x and y directions if there is any character close enough to be a target.
        bool CheckCloseTargets(Grid battlefield)
        {
            // As the Archer has a long range attack, need to check for all grids within its range.
            for (int index = attackingDistance; index >= 1; index--)
            {
                bool left = battlefield.grids.Find(grid => grid.Index == currentBox.Index - index).ocupied;
                bool right = battlefield.grids.Find(grid => grid.Index == currentBox.Index + index).ocupied;
                bool up = battlefield.grids.Find(grid => grid.Index == currentBox.Index - (battlefield.yLength * index)).ocupied;
                bool down = battlefield.grids.Find(grid => grid.Index == currentBox.Index + (battlefield.yLength * index)).ocupied;

                // Fixed the conditions for this method to return true.
                if (left || right || up || down)
                {
                    return true;
                }
            }

            return false; 
        }

        // Created a method to visually update the player's health.
        public void UpdateHealthBar(int amount, bool isHealing = false)
        {
            if (!isHealing && amount > 0)
            {
                playerHealth = playerHealth.Remove(playerHealth.Length - amount);
            }
            else if (isHealing)
            {
                playerHealth = playerHealth.Insert(0, "*");
            }
        }

        /// <summary>
        /// Sets the health bar UI according to the character's health.
        /// </summary>
        public void SetHealthBar()
        {
            string currentHealth = "";

            for (int i = 0; i < Health; i++)
            {
                currentHealth = currentHealth.Insert(0, "*");
            }

            playerHealth = currentHealth;
        }

        /// <summary>
        /// Prints each character info.
        /// </summary>
        public void PrintCharactersInfo()
        {
            Console.WriteLine($"Current Player: {Name} ({characterClass})\nHealth: {playerHealth}\n");
            Console.WriteLine($"Enemy Player: {Target.Name} ({Target.characterClass})\nHealth: {Target.playerHealth}\n");
        }

        public void Attack (Character target)
        {
            var damage = Helper.GetRandomInt(0, BaseDamage);
            target.TakeDamage(damage);

            // The Warrior character has a 20% chance to attack twice per turn.
            if (characterClass == CharacterClass.Warrior)
            {
                var doubleAttackChance = Helper.GetRandomInt(0, 6);

                if (doubleAttackChance == 0)
                {
                    Console.WriteLine($"{Name} managed to attack a second time!\n");
                    Attack(target);
                }
            }

            // The Cleric character has a 10% chance of healing itself when attacking.
            if (characterClass == CharacterClass.Cleric)
            {
                var healingChance = Helper.GetRandomInt(0, 11);

                if (healingChance == 0)
                {
                    Console.WriteLine($"{Name} has healed itself!\n");
                    Heal();
                }
            }
        }

        /// <summary>
        /// Heals the character's health by one unit (currently only used by the Cleric class).
        /// </summary>
        void Heal()
        {
            Health++;
            UpdateHealthBar(1, true);
        }

        /// <summary>
        /// Sets the weakness according to each character class.
        /// </summary>
        public void SetWeakness()
        {
            switch (characterClass)
            {
                case CharacterClass.Warrior:
                    isWeakness = Target.characterClass == CharacterClass.Cleric;
                    break;
                case CharacterClass.Cleric:
                    isWeakness = Target.characterClass == CharacterClass.Archer;
                    break;
                case CharacterClass.Archer:
                    isWeakness = Target.characterClass == CharacterClass.Warrior;
                    break;
                case CharacterClass.Paladin:
                default:
                    break;
            }
        }
    }
}
