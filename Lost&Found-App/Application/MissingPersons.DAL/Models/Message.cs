using System;
using System.Collections.Generic;

namespace MissingPersons.DAL.Models;

public partial class Message
{
    public int Id { get; set; }

    public int? ConversationId { get; set; }

    public int? SenderId { get; set; }

    public string? Content { get; set; }

    public DateTime? SentAt { get; set; }

    public bool? IsRead { get; set; }

    public virtual Conversation? Conversation { get; set; }

    public virtual User? Sender { get; set; }
}
