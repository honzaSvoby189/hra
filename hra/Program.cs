using System;
using System.Collections.Generic;

namespace RPG_Game
{
    // Třída pro dungeony, které generují místnosti
    public class Dungeon
    {
        public List<Room> Rooms { get; private set; }

        public Dungeon(int numberOfRooms)
        {
            Rooms = new List<Room>();
            GenerateRooms(numberOfRooms);
        }

        private void GenerateRooms(int numberOfRooms)
        {
            for (int i = 0; i < numberOfRooms; i++)
            {
                Rooms.Add(new Room { Name = $"Místnost {i + 1}", Description = "Tajemná místnost." });
            }
        }
    }

    // Třída pro místnosti
    public class Room
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    // Třída pro hráče
    public class Player
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public List<Item> Inventory { get; private set; }

        public Player(string name, int health)
        {
            Name = name;
            Health = health;
            MaxHealth = health;
            Inventory = new List<Item>();

            // Přidání 5 lektvarů do inventáře
            for (int i = 0; i < 5; i++)
            {
                Inventory.Add(new Item { Name = "Léčivý lektvar", Effect = "Obnovuje 20 zdraví" });
            }
        }

        public void Attack(Enemy enemy)
        {
            Console.WriteLine($"{Name} útočí na {enemy.Name}!");
            enemy.TakeDamage(20); // Hráč dává 20 poškození
        }

        public void Defend()
        {
            Console.WriteLine($"{Name} se brání!");
        }

        public void UseItem(Item item)
        {
            item.Use(this);
            Inventory.Remove(item);
        }
    }

    // Třída pro nepřátele
    public class Enemy
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int Damage { get; set; }

        public Enemy(string name, int health, int damage)
        {
            Name = name;
            Health = health;
            Damage = damage;
        }

        public void Attack(Player player)
        {
            Console.WriteLine($"{Name} útočí na {player.Name} a dává {Damage} poškození!");
            player.Health -= Damage;
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            Console.WriteLine($"{Name} dostal {damage} poškození. Zbývající zdraví: {Health}");
        }
    }

    // Třída pro předměty
    public class Item
    {
        public string Name { get; set; }
        public string Effect { get; set; }

        public void Use(Player player)
        {
            if (Name == "Léčivý lektvar")
            {
                player.Health += 20;
                if (player.Health > player.MaxHealth)
                {
                    player.Health = player.MaxHealth;
                }
                Console.WriteLine($"{player.Name} použil {Name} a obnovil 20 zdraví. Aktuální zdraví: {player.Health}/{player.MaxHealth}");
            }
        }
    }

    // Hlavní herní smyčka
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Vítejte ve hře RPG!");

            // Získání jména hráče
            Console.Write("Zadejte své jméno: ");
            string playerName = Console.ReadLine();

            // Vytvoření hráče
            Player player = new Player(playerName, 200);
            Console.WriteLine($"Hráč vytvořen: {player.Name} se zdravím {player.Health}/{player.MaxHealth}.");

            List<Enemy> enemies = new List<Enemy>();

            bool gameRunning = true;

            // Hlavní smyčka hry
            while (gameRunning)
            {
                Console.WriteLine($"\n{player.Name}'s zdraví: {player.Health}/{player.MaxHealth}");
                Console.WriteLine("Vyberte kam chcete jít:");
                Console.WriteLine("1. Jeskyně");
                Console.WriteLine("2. Pokračovat dál");
                Console.WriteLine("3. Ukončit hru");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        // Vytvoření nepřátel pro jeskyni
                        enemies = new List<Enemy>
                        {
                            new Enemy("Goblin", 50, 10),
                            new Enemy("Ork", 80, 15),
                            new Enemy("Drak", 200, 30)
                        };
                        Console.WriteLine("Jdete do jeskyně... Cítíte tmu a nebezpečí.");
                        EncounterEnemy(player, enemies);
                        break;

                    case "2":
                        // Vytvoření nepřátel na cestě
                        enemies = new List<Enemy>
                        {
                            new Enemy("Bandita", 40, 12),
                            new Enemy("Vlk", 60, 8)
                        };
                        Console.WriteLine("Pokračujete na další cestu...");
                        EncounterEnemy(player, enemies);
                        break;

                    case "3":
                        Console.WriteLine("Ukončuji hru. Na shledanou!");
                        gameRunning = false;
                        break;

                    default:
                        Console.WriteLine("Neplatná volba. Zkuste to znovu.");
                        break;
                }
            }
        }

        static void EncounterEnemy(Player player, List<Enemy> enemies)
        {
            // Náhodné setkání s nepřitelem
            Random rand = new Random();
            int enemyIndex = rand.Next(enemies.Count);
            Enemy encounteredEnemy = enemies[enemyIndex];

            Console.WriteLine($"Narazil jsi na nepřítele: {encounteredEnemy.Name}!");

            // Volba zda bojovat nebo ne
            Console.Write("Chceš s ním bojovat? (ano/ne): ");
            string fightChoice = Console.ReadLine().ToLower();

            if (fightChoice == "ano")
            {
                Console.WriteLine($"Bojíš se s {encounteredEnemy.Name}!");

                // Boj
                while (encounteredEnemy.Health > 0 && player.Health > 0)
                {
                    Console.WriteLine($"\n{player.Name}'s zdraví: {player.Health}/{player.MaxHealth}");
                    Console.WriteLine($"{encounteredEnemy.Name}'s zdraví: {encounteredEnemy.Health}");
                    Console.WriteLine("Vyberte svou akci:");
                    Console.WriteLine("1. Útok");
                    Console.WriteLine("2. Obrana");
                    Console.WriteLine("3. Použít lektvar");

                    string battleChoice = Console.ReadLine();

                    switch (battleChoice)
                    {
                        case "1":
                            player.Attack(encounteredEnemy);
                            break;
                        case "2":
                            player.Defend();
                            break;
                        case "3":
                            if (player.Inventory.Count > 0)
                            {
                                // Použití prvního lektvaru v inventáři
                                player.UseItem(player.Inventory[0]);
                            }
                            else
                            {
                                Console.WriteLine("Nemáte žádné lektvary.");
                            }
                            break;
                        default:
                            Console.WriteLine("Neplatná volba. Zkuste to znovu.");
                            break;
                    }

                    if (encounteredEnemy.Health > 0)
                    {
                        encounteredEnemy.Attack(player);
                    }

                    if (player.Health <= 0)
                    {
                        Console.WriteLine("Byli jste poraženi. Konec hry!");
                        break;
                    }

                    if (encounteredEnemy.Health <= 0)
                    {
                        Console.WriteLine($"{encounteredEnemy.Name} byl poražen!");
                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Nepřítel odchází. Můžete pokračovat.");
            }

            // Po boji nebo úprku možnost výběru cesty
            Console.WriteLine("\nCo chcete dělat teď?");
            Console.WriteLine("1. Jít zpět do jeskyně");
            Console.WriteLine("2. Pokračovat dál");
            Console.WriteLine("3. Ukončit hru");
            string nextChoice = Console.ReadLine();
            if (nextChoice == "1")
            {
                Console.WriteLine("Vracíš se zpět do jeskyně.");
                EncounterEnemy(player, enemies);
            }
            else if (nextChoice == "2")
            {
                Console.WriteLine("Pokračuješ dál.");
            }
            else
            {
                Console.WriteLine("Ukončuji hru. Na shledanou!");
            }
        }
    }
}
