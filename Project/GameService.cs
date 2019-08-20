using System;
using CastleGrimtol.Project.Interfaces;
using CastleGrimtol.Project.Models;

namespace CastleGrimtol.Project
{
  public class GameService : IGameService
  {
    public IRoom CurrentRoom { get; set; }
    public Victim CurrentVictim { get; set; }
    public bool running { get; private set; }
    public Item Items { get; set; }

    //Initializes the game, creates rooms, their exits, and add items to rooms
    public void Setup()
    {
      running = true;
      Room DungeonRoomStart = new Room("Entrance", "This must be the start of this dungeon. Its cold and dark given what your wearing appears to be only rags. ");
      Room DungeonRoom1 = new Room("Room 1", "The room is slightly lit with stone walls and a rotted wooden door at the other side of the room.");
      Room DungeonRoom2 = new Room("Room 2", "The room is dimmer then the last, but you can still make out another rotten door at the end of the room. You also notice a large peice of paper in the corner of the room. Curious.");
      Item scroll = new Item("Scroll", "This is a scroll. Inscribed is the spell fireball. Spells from scrolls can only be used once so use it wisly.");
      Room DungeonRoom3 = new Room("Room 3", "This room is almost pitch black. The only light scource seems to be the door at the end of the room giving off an ominous glow. You hear the sound that sends shivers down your spine. This must be the etherials room past this door.");
      Room DungeonRoomBoss = new Room("Etherials Lair", "As you enter the room, the temperature drops to and almost unbearable level. Infront of you stands the etherial staring with starving eyes in your direction. Around the etherial are the bones and corpeses of previous victims. If you have an item now would be the time to use it!");
      Item Monster = new Item("Etherial", "Etherials. A beast of pure magic. ");
      Room DungeonRoomTREASURE = new Room("Treasure Room", "Welp this wouldn't be much of a dungeon if there wasn't treasure right? The room is fully lit with millions of gold artifacts and gold peices glowing in the light. This was well worth fighting your fate!");

      DungeonRoomStart.Exits.Add("north", DungeonRoomStart);
      DungeonRoomStart.Exits.Add("south", DungeonRoom1);
      DungeonRoom1.Exits.Add("north", DungeonRoomStart);
      DungeonRoom1.Exits.Add("south", DungeonRoom2);
      DungeonRoom2.Exits.Add("north", DungeonRoom1);
      DungeonRoom2.Exits.Add("south", DungeonRoom3);
      DungeonRoom3.Exits.Add("north", DungeonRoom2);
      DungeonRoom3.Exits.Add("south", DungeonRoomBoss);
      DungeonRoomBoss.Exits.Add("north", DungeonRoom3);
      DungeonRoomBoss.Exits.Add("south", DungeonRoomTREASURE);

      DungeonRoomBoss.Items.Add(Monster);
      DungeonRoom2.Items.Add(scroll);


      CurrentRoom = DungeonRoomStart;

    }

    //Restarts the game 
    public void Reset()
    {
      Console.Clear();
      StartGame();
    }

    //Setup and Starts the Game loop
    public void StartGame()
    {
      Setup();
      Console.WriteLine("Welcome victim of fate. Let us see if you can fight your destiny. ");
      Console.WriteLine("Tell us your name so we may proceed.");
      var name = Console.ReadLine();
      CurrentVictim = new Victim(name);
      // Console.Clear();
      Console.WriteLine($"Welcome {CurrentVictim.VictimName}, You have been thrown into the dungeon home to the ethearial. You were a victim and prisoner of war. The king that inprisoned you throws all sorry souls into this dungeon. Hopefully you make this more interesting then the last prisoner.");
      while (running)
      {
        GetUserInput();
      }
    }

    //Gets the user input and calls the appropriate command
    public void GetUserInput()
    {
      Console.WriteLine($"What do you wish to do, {CurrentVictim.VictimName} ?");
      string UserInput = Console.ReadLine();
      //FIXME make this two inputs similar to planets
      // Console.WriteLine($"What do you wish to do, {CurrentVictim.VictimName} ?");
      // string[] UserInput = Console.ReadLine().Split(' ');
      // string command = UserInput[0].ToLower();
      // string option = UserInput[1];
      // switch (command)
      switch (UserInput.ToLower())
      {
        case "proceed south":
          Go("south");
          break;
        case "proceed north":
          Go("north");
          break;

        //FIXME command and option will simplify this
        case "take item":
          TakeItem("scroll");
          break;
        case "use item":
          UseItem("scroll");
          break;
        case "inventory":
          Inventory();
          break;
        case "quit":
          Quit();
          break;
        case "help":
          Help();
          break;
        case "reset":
          Reset();
          break;
        case "look":
          Look();
          break;
        default:
          System.Console.WriteLine("Uhmmmm..... What?");
          break;
      }
    }
    #region Console Commands

    //Stops the application
    public void Quit()
    {
      Console.Clear();
      Console.WriteLine("You've accepted your fate. Pitiful.");
      running = false;

    }

    //Should display a list of commands to the console
    public void Help()
    {
      Console.WriteLine(@"Possible Commands:
      proceed(North/South)   Inventory       Use item     Help
      Look              Take Item       Reset        Quit ");
    }

    //Validate CurrentRoom.Exits contains the desired direction
    //if it does change the CurrentRoom
    public void Go(string direction)
    {
      Item Monster = CurrentRoom.Items.Find(x => x.Name.Contains("Etherial"));
      Item stashedItem = CurrentVictim.Inventory.Find(x => x.Name.Contains("Scroll"));
      if (CurrentRoom.Exits.ContainsKey(direction))
      {
        CurrentRoom = CurrentRoom.Exits[direction];
        Console.WriteLine($"You are now in {CurrentRoom.Name} {CurrentRoom.Description}");
        if (Monster != null && CurrentRoom.Name == "Treasure Room")
        {
          Console.WriteLine("The monster follows you into the treasure room and eats you. Game Over");
          running = false;
        }
      }

      else
      {
        Console.WriteLine("Why are you trying to continue? You won.");
        running = false;
      }
    }





    //When taking an item be sure the item is in the current room 
    //before adding it to the player inventory, Also don't forget to 
    //remove the item from the room it was picked up in

    //NOTE Runs through the array of items and pulls the item based on where it is placed. 
    public void TakeItem(string itemName)
    {
      // foreach (Item a in CurrentRoom.Items)
      // {
      //   Console.WriteLine(a.Name);
      // }
      Item collectable = CurrentRoom.Items.Find(x => x.Name.Contains("Scroll"));
      if (collectable != null)
      {
        Console.WriteLine("The piece of paper on the ground was a Scroll! Inscribed is the spell fireball.");
        Console.WriteLine("Your previous knowledge reminds you that a scrolls spell can only be used once. Use it wisley.");
        CurrentVictim.Inventory.Add(CurrentRoom.Items[0]);
      }
      else
      {
        Console.WriteLine("There are no items here.");
      }

      // if (CurrentRoom.Items[0].Name == "Scroll")
      // {
      //   Console.WriteLine("The piece of paper on the ground was a Scroll! Inscribed is the spell fireball.");
      //   Console.WriteLine("Your previous knowledge reminds you that a scrolls spell can only be used once. Use it wisley.");
      //   CurrentVictim.Inventory.Add(CurrentRoom.Items[0]);
      // }
      // else
      // {
      //   Console.WriteLine("There are no items here.");
      // }
    }

    //No need to Pass a room since Items can only be used in the CurrentRoom
    //Make sure you validate the item is in the room or player inventory before
    //being able to use the item
    public void UseItem(string itemName)
    {
      //FIXME Player.Inventory.find
      Item foundItem = CurrentVictim.Inventory.Find(x => x.Name.Contains("Scroll"));
      Item Monster = CurrentRoom.Items.Find(x => x.Name.Contains("Etherial"));
      if (foundItem != null)
      {
        if (Monster != null)
        {
          Console.WriteLine("You cast the fireball at the Etherial! It lets out a wail as it begins to fade from existence.");
          CurrentVictim.Inventory.Remove(foundItem);
          CurrentRoom.Items.Remove(Monster);
          Console.WriteLine("You may proceed south for your reward.");
        }
        else
        {
          Console.WriteLine("You cast the fireball into the air! It flys off for a second then hits the wall scorching it black!");
          CurrentVictim.Inventory.Remove(foundItem);
          Console.WriteLine("That was a waste.");

        }
      }
      else
      {
        Console.WriteLine("You don't have any items to use.");
      }
    }



    //Print the list of items in the players inventory to the console
    public void Inventory()
    {
      Item stashedItem = CurrentVictim.Inventory.Find(x => x.Name.Contains("Scroll"));
      if (stashedItem != null)
      {
        Console.WriteLine("You have a scroll of fire ball stashed.");
      }
      else
      {
        Console.WriteLine("You dont have any items in your inventory.");
      }

      // if (CurrentVictim.Inventory.Count < 1)
      // {
      //   Console.WriteLine("Your pockets are empty.");
      // }
      // else
      // {
      //   Console.WriteLine("You have a scroll of fireball!");
      // }
      //   foreach (Item a in CurrentVictim.Inventory)
      //   {
      //     Console.WriteLine("name =", a.Name);
      //   }
      //   Console.WriteLine(CurrentVictim.Inventory.Count);
      //   Console.WriteLine("You have a Scroll of Fireball! Use it wisely.");
      // }
      // else
      // {
      //   Console.WriteLine(CurrentVictim.Inventory.Count);
      //   Console.WriteLine("Your pockets are empty.");
      // }
    }


    //Display the CurrentRoom Description, Exits, and Items
    public void Look()
    {
      Console.WriteLine($"You are currently in {CurrentRoom.Name}");
      Console.WriteLine($"{CurrentRoom.Description}");

    }
  }

}

#endregion




