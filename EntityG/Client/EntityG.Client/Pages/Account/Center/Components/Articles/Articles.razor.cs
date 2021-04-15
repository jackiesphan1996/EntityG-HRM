using System.Collections.Generic;
using EntityG.Client.Models;
using Microsoft.AspNetCore.Components;

namespace EntityG.Client.Pages.Account.Center
{
    public partial class Articles
    {
        [Parameter] public IList<ListItemDataType> List { get; set; }
    }
}