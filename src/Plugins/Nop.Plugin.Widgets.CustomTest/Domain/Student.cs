﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;

namespace Nop.Plugin.Widgets.CustomTest.Domain;
public class Student : BaseEntity
{
    public int Age { get; set; }
    public string Name { get; set; }
}
