//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eutyches.Spell.Lexedit.Services.Models
{
    public class GoToAffixEvent : PubSubEvent<string> { }

    public class GoToStemEvent : PubSubEvent<Guid> { }
}