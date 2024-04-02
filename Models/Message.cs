using System;
using System.Collections.Generic;

namespace property_rental_management.Models;

public partial class Message
{
    public int MessageId { get; set; }

    public string Sender { get; set; } = null!;

    public string Receiver { get; set; } = null!;

    public string Message1 { get; set; } = null!;

    public string Subject { get; set; } = null!;
}
