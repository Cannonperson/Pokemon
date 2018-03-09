using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Pokemon
{
    class Program
    {
        static void Main(string[] args)
        {
            Regex numbers = new Regex("[0-9]");
            List<Pokemon> roster = new List<Pokemon>();


            List<Move> pokemonMoves = new List<Move>();
            pokemonMoves.Add(new Move("Ember"));
            pokemonMoves.Add(new Move("Fire Blast"));
            roster.Add(new Pokemon("Charmander", 3, 52, 43, 39, Elements.Fire, pokemonMoves));

            pokemonMoves = new List<Move>();
            pokemonMoves.Add(new Move("Bubble"));
            pokemonMoves.Add(new Move("Bite"));
            roster.Add(new Pokemon("Squirtle", 2, 48, 65, 44, Elements.Water, pokemonMoves));

            pokemonMoves = new List<Move>();
            pokemonMoves.Add(new Move("Cut"));
            pokemonMoves.Add(new Move("Mega Drain"));
            pokemonMoves.Add(new Move("Razor Leaf"));
            roster.Add(new Pokemon("Bulbasaur", 3, 49, 49, 45, Elements.Grass, pokemonMoves));

            Console.WriteLine("Welcome to the world of Pokemon!\nThe available commands are list/fight/heal/quit");

            while (true)
            {
                Console.WriteLine("\nPlese enter a command");
                switch (Console.ReadLine())
                {
                    case "list":
                        foreach (Pokemon pokemon in roster)
                            Console.Write(pokemon.Name + " ");
                        Console.WriteLine();
                        break;

                    case "fight":
                        Console.WriteLine("Select two pokemon by writing each their names seperated by a space!");
                        Console.Write("Choose who you'll fight with(");
                        foreach (Pokemon pokemon in roster)
                            Console.Write(pokemon.Name + " ");
                        Console.Write(")\n");

                        //READ INPUT, REMEMBER IT SHOULD BE TWO POKEMON NAMES
                        Regex pokeNames = new Regex("(charmander|squirtle|bulbasaur) (charmander|squirtle|bulbasaur)");
                        string input = Console.ReadLine().ToLower();

                        Pokemon player = null;
                        Pokemon enemy = null;

                        if(pokeNames.IsMatch(input))
                        {
                            switch (input.Substring(0,input.IndexOf(" ")))
                            {
                                case "charmander":
                                    player = roster[0];
                                    break;
                                case "squirtle":
                                    player = roster[1];
                                    break;
                                case "bulbasaur":
                                    player = roster[2];
                                    break;
                            }
                            switch (input.Substring(input.IndexOf(" ") + 1))
                            {
                                case "charmander":
                                    enemy = roster[0];
                                    break;
                                case "squirtle":
                                    enemy = roster[1];
                                    break;
                                case "bulbasaur":
                                    enemy = roster[2];
                                    break;
                            }
                        }

                        //if everything is fine and we have 2 pokemons let's make them fight
                        if (player != null && enemy != null && player != enemy)
                        {
                            Console.WriteLine("A wild " + enemy.Name + " appears!");
                            Console.Write(player.Name + " I choose you! ");
                            
                            while (player.Hp > 0 && enemy.Hp > 0)
                            {
                                Console.Write("What move should we use? Choose one by its number\n(");
                                foreach (Move move in player.Moves)
                                    Console.Write(move.Name + "[" + player.Moves.IndexOf(move) + "]");
                                Console.Write(")\n");

                                Move playerMove = null;
                                while (playerMove == null)
                                {
                                    input = Console.ReadLine();
                                    if (numbers.IsMatch(input)) {
                                        int intInput = int.Parse(input);
                                        if (intInput <= 0 || intInput < player.Moves.Count)
                                            playerMove = player.Moves[intInput];
                                        if (playerMove == null)
                                            Console.WriteLine("Invalid move! Choose another!");
                                    }
                                }

                                int damage = player.Attack(enemy);
                                enemy.ApplyDamage(damage);

                                //print the move and damage
                                Console.WriteLine(player.Name + " uses " + player.Moves[int.Parse(input)].Name + ". " + enemy.Name + " loses " + damage + " HP");

                                //if the enemy is not dead yet, it attacks
                                if (enemy.Hp > 0)
                                {
                                    Random rand = new Random();
                                    /*the C# random is a bit different than the Unity random
                                     * you can ask for a number between [0,X) (X not included) by writing
                                     * rand.Next(X) 
                                     * where X is a number 
                                     */
                                    int enemyMove = rand.Next(0,enemy.Moves.Count);
                                    int enemyDamage = enemy.Attack(player);
                                    player.ApplyDamage(enemyDamage);

                                    //print the move and damage
                                    Console.WriteLine(enemy.Name + " uses " + enemy.Moves[enemyMove].Name + ". " + player.Name + " loses " + enemyDamage + " HP");
                                }
                            }
                            //The loop is over, so either we won or lost
                            if (enemy.Hp <= 0)
                            {
                                Console.WriteLine(enemy.Name + " faints, you won!");
                            }
                            else
                            {
                                Console.WriteLine(player.Name + " faints, you lost...");
                            }
                        }
                        //otherwise let's print an error message
                        else
                        {
                            Console.WriteLine("Invalid pokemons");
                        }
                        break;

                    case "heal":
                        foreach (Pokemon pokemon in roster)
                            pokemon.Restore();

                        Console.WriteLine("All pokemons have been healed");
                        break;

                    case "quit":
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Unknown command");
                        break;
                }
            }
        }
    }
}
