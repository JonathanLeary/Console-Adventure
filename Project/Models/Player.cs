using System.Collections.Generic;
using CastleGrimtol.Project.Interfaces;

namespace CastleGrimtol.Project.Models
{
  public class Victim : IPlayer
  {
    public string VictimName { get; set; }
    public List<Item> Inventory { get; set; }
    public Victim(string name)
    {
      VictimName = name;
      Inventory = new List<Item>();
    }
  }
}