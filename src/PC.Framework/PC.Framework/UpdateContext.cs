using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PebbleCode.Entities
{
    /// <summary>
    /// Utilises disposable pattern to set a particular update context level against
    /// a set of entities, for the during of an update operation
    /// </summary>
    /// <typeparam name="UpdateContextConstants"></typeparam>
    public class UpdateContext<UpdateContextConstants> : IDisposable
    {
        private readonly ControlledUpdateEntity<UpdateContextConstants>[] _entities;
        public string Name { get; private set; }

        public UpdateContext(string name, params ControlledUpdateEntity<UpdateContextConstants>[] entities)
        {
            _entities = entities;
            foreach (var entity in _entities)
                entity.UpdateContext = this;
            Name = name;
        }

        public void Dispose()
        {
            foreach (var entity in _entities)
                entity.UpdateContext = null;
        }
    }
}
