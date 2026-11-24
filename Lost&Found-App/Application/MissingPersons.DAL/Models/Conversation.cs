using System;
using System.Collections.Generic;

namespace MissingPersons.DAL.Models;

public partial class Conversation
{
    public int Id { get; set; }

    public int? MatchId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Match? Match { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
