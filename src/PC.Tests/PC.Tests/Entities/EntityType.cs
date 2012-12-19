using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PebbleCode.Framework.Collections;

namespace PebbleCode.Tests.Entities
{
    public static class EntityType
    {
        private static int _idx = 1;

        public static Flags None                = Flags.None;

        public static Flags Thing               = new Flags(_idx++);
        public static Flags Widget              = new Flags(_idx++);
        public static Flags FieldTest           = new Flags(_idx++);
        public static Flags VersionedThing      = new Flags(_idx++);
        public static Flags NodeBuilderTest     = new Flags(_idx++);
        public static Flags ControlledUpdateThing = new Flags(_idx++);
    }
}

