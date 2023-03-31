using System;
using System.Collections.Generic;

namespace CRUD_First.Models;

public partial class State
{
    public int StateId { get; set; }

    public string? StateName { get; set; }

    public int? CountryId { get; set; }
}
