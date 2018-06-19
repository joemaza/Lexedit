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
    public class FileClosedEvent : PubSubEvent { }

    public class FileCreatedEvent : PubSubEvent { }

    public class FileDataChangedEvent : PubSubEvent { }

    public class FileErrorEvent : PubSubEvent<Exception> { }

    public class FileOpenedEvent : PubSubEvent { }

    public class FileSavedEvent : PubSubEvent { }
}