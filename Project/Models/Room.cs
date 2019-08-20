using System.Collections.Generic;
using CastleGrimtol.Project.Interfaces;

namespace CastleGrimtol.Project.Models
{
  public class Room : IRoom
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Item> Items { get; set; }
    public bool Gameover { get; set; }
    public bool IsLocked { get; set; }
    public Dictionary<string, Room> Exits { get; set; }
    public Room(string name, string description)
    {
      Name = name;
      Description = description;
      Exits = new Dictionary<string, Room>();
      Items = new List<Item>();
      IsLocked = false;
      Gameover = false;
    }
  }
}


