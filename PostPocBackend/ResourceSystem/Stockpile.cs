using System;
using System.Collections.Generic;

namespace PostPocModel.ResourceSystem
{
    public class Stockpile {

        private Dictionary<IResource, int> resources = new Dictionary<IResource, int>();

        public void Adjust(IResource resource, int amount) {
            if (resource == null) throw new ArgumentNullException("resource");

            if(resources.ContainsKey(resource) == false)
                resources.Add(resource, amount);
            else
                resources[resource] += amount;
        }

        public int GetLevel(IResource target) {
            resources.TryGetValue(target, out int value);
            return value;
        }
    }
}
