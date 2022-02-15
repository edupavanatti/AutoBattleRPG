using System;
using System.Collections.Generic;
using System.Linq;
using static AutoBattle.Types;

namespace AutoBattle
{
    class Program
    {
        /*
        IMPORTANT
        ---------------------------------------------------------------------------------------------------------------------------
        I've changed the project's .NET version to 6.0.
        It might be necessary to change it back to 3.1 depending on the ambient setup in order for the application to run correctly.
        ---------------------------------------------------------------------------------------------------------------------------
        */

        static void Main(string[] args)
        {
            Grid grid = null;
            GridBox PlayerCurrentLocation;
            GridBox EnemyCurrentLocation;
            Character PlayerCharacter;
            Character EnemyCharacter;
            List<Character> AllPlayers = new List<Character>();
            int currentTurn = 0;
            int numberOfPossibleTiles = 0;
            string playerName;

            Setup();

            void Setup()
            {
                // Added a welcome message.
                Console.WriteLine("Welcome to the AutoBattle RPG!\n");
                SetBattlefieldSize();
            }

            // Added a method to set the battlefield size.
            void SetBattlefieldSize()
            {
                int lines;
                int collumns;

                // Gets the number of lines of the battlefield based on user input.
                while (true)
                {
                    Console.WriteLine("Enter the number of lines of the battlefield (between 2 and 10):");
                    int.TryParse(Console.ReadLine(), out lines);
                    
                    if (lines >= 2 && lines <= 10)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("\nInvalid value! Please insert a number between 2 and 10.\n");
                        continue;
                    }
                }

                // Gets the number of collumns of the battlefield based on user input.
                while (true)
                {
                    Console.WriteLine("\nEnter the number of collumns of the battlefield (between 2 and 10):");
                    int.TryParse(Console.ReadLine(), out collumns);

                    if (collumns >= 2 && collumns <= 10)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("\nInvalid value! Please insert a number between 2 and 10.\n");
                        continue;
                    }
                }

                // Create a new battlefiend (grid) based on the defines number of lines and collumns.
                grid = new Grid(lines, collumns);
                numberOfPossibleTiles = grid.grids.Count;

                GetCharacterName();
            }

            void GetCharacterName()
            {
                Console.WriteLine("Enter your character name:");
                playerName = Console.ReadLine();

                // Checks if the inputted name is not empty.
                if (playerName.Trim().Any())
                {
                    GetPlayerChoice();
                }
                else
                {
                    Console.WriteLine("The character name must not be empty!\n");
                    GetCharacterName();
                }
            }

            void GetPlayerChoice()
            {
                // Asks for the player to choose between four possible classes via console.
                Console.WriteLine("\nChoose between one of these classes:");
                Console.WriteLine("[1] Paladin [2] Warrior [3] Cleric [4] Archer");
                // Stores the player choice in a variable.
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreatePlayerCharacter(Int32.Parse(choice));
                        break;
                    case "2":
                        CreatePlayerCharacter(Int32.Parse(choice));
                        break;
                    case "3":
                        CreatePlayerCharacter(Int32.Parse(choice));
                        break;
                    case "4":
                        CreatePlayerCharacter(Int32.Parse(choice));
                        break;
                    default:
                        // Added a text to warn the user that the choice was invalid.
                        Console.WriteLine("\nInvalid choice! Please select a valid option.");
                        GetPlayerChoice();
                        break;
                }
            }

            void CreatePlayerCharacter(int classIndex)
            {
                CharacterClass characterClass = (CharacterClass) classIndex;
                Console.WriteLine($"\nPlayer Class Choice: {characterClass} (O)");

                PlayerCharacter = new Character(characterClass);
                // Added the player's character name definition.
                PlayerCharacter.Name = playerName;
                PlayerCharacter.PlayerIndex = 0;
                PlayerCharacter.isPlayer = true;
                
                CreateEnemyCharacter();
            }

            void CreateEnemyCharacter()
            {
                // Randomly choose the enemy class and set up vital variables.
                var rand = new Random();
                int randomInteger = rand.Next(1, 4);

                CharacterClass enemyClass = (CharacterClass) randomInteger;
                Console.WriteLine($"Enemy Class Choice: {enemyClass} (X)\n");

                EnemyCharacter = new Character(enemyClass);
                // Added the enemy's character name definition.
                EnemyCharacter.Name = "Unnamed";
                EnemyCharacter.PlayerIndex = 1;
                StartGame();
            }

            void StartGame()
            {
                // Populates the character variables and targets.
                EnemyCharacter.Target = PlayerCharacter;
                PlayerCharacter.Target = EnemyCharacter;
                AllPlayers.Add(PlayerCharacter);
                AllPlayers.Add(EnemyCharacter);
                // Shuffles the players list so the starting player is random.
                AllPlayers = AllPlayers.OrderBy(player => Guid.NewGuid()).ToList();
                // Assigns the grid and sets the weakness for each player.
                AllPlayers.ForEach(player => player.battlefield = grid);
                AllPlayers.ForEach(player => player.SetWeakness());
                AlocatePlayerCharacter();
                StartTurn();
            }

            void StartTurn()
            {
                // Separated each player's turn.
                if (currentTurn % 2 == 0)
                {
                    AllPlayers[0].StartTurn();
                }
                else
                {
                    AllPlayers[1].StartTurn();
                }

                currentTurn++;
                HandleTurn();
            }

            void HandleTurn()
            {
                if (PlayerCharacter.Health == 0)
                {
                    // Added a text to be shown when the player character is defeated.
                    Console.WriteLine("You were defeated! Better luck next time...");
                    Console.ReadKey();
                    return;
                }
                else if (EnemyCharacter.Health == 0)
                {
                    // Added a text to be shown when the enemy character is defeated.
                    Console.WriteLine("The enemy was defeated! Congratulations, you've won the game!");
                    Console.ReadKey();
                    return;
                }
                else
                {
                    Console.WriteLine("Press any key to start the next turn...");
                    Console.ReadKey();
                    StartTurn();
                }
            }

            void AlocatePlayerCharacter()
            {
                // Changed this variable to have a random value.
                int random = Helper.GetRandomInt(0, numberOfPossibleTiles - 1);
                GridBox RandomLocation = (grid.grids.ElementAt(random));

                if (!RandomLocation.ocupied)
                {
                    // Fixed setting the player to a random location.
                    PlayerCurrentLocation = RandomLocation;
                    RandomLocation.ocupied = true;
                    RandomLocation.isOcupiedByPlayer = true;
                    grid.grids[random] = RandomLocation;
                    PlayerCharacter.currentBox = grid.grids[random];
                    PlayerCharacter.SetHealthBar();
                    AlocateEnemyCharacter();
                }
                else
                {
                    AlocatePlayerCharacter();
                }
            }

            void AlocateEnemyCharacter()
            {
                // Changed this variable to have a random value.
                int random = Helper.GetRandomInt(0, numberOfPossibleTiles - 1);
                GridBox RandomLocation = grid.grids.ElementAt(random);

                if (!RandomLocation.ocupied)
                {
                    EnemyCurrentLocation = RandomLocation;
                    RandomLocation.ocupied = true;
                    grid.grids[random] = RandomLocation;
                    EnemyCharacter.currentBox = grid.grids[random];
                    EnemyCharacter.SetHealthBar();
                    FinishSetup();
                }
                else
                {
                    AlocateEnemyCharacter();
                }
            }

            // Finishes the match setup after both players were allocated.
            void FinishSetup()
            {
                grid.drawBattlefield(true);
                Console.WriteLine("Press any key to start the game.");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}
