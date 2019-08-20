using System.Collections.Generic;
using CastleGrimtol.Project.Models;

namespace CastleGrimtol.Project.Interfaces
{
  public interface IPlayer
  {
    string VictimName { get; set; }
    List<Item> Inventory { get; set; }
  }
}
