using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PostPocModel.ResourceSystem
{

    public class ResourceBank<T> : IResoruceBank<T>  where T : IResource
    {
        private List<T> resources = new List<T>();

        public T Get(string name) {
            return resources.Find(x => x.Name == name) ;
        }

        public List<T> GetByTag(string tag) {
            tag = tag.ToLower().Trim();

            if (tag == "") throw new ArgumentException("empty tag passed");

            return resources.FindAll(x => x.Tags.Contains(tag));

        }

        public void AddNewResource(T resource)
        {
            if (resource == null) throw new ArgumentNullException("resource");

            if ( resource.Name.Trim() == "") throw new ArgumentException("empty resource passed");

            if (Get(resource.Name) != null)
                throw new ArgumentException("Duplicate Resource add atempted");



            resources.Add(resource);
        }
    }

    public class Resource : IResource{
        public string Name { get; private set; }
        public ISet<string> Tags { get; private set; }


        public Resource(IResoruceBank<Resource> bank, string name, ISet<string> tags = null) {
            this.Name = name;

            if (tags == null)
                tags = new HashSet<string>();
            else
            {
                this.Tags = tags.Select(x=>x.ToLower().Trim()).ToHashSet();
            }
            
            bank.AddNewResource(this);
        }

    }

    public interface IResoruceBank<T> where T : IResource
    {

        void AddNewResource(T resource);

    }

    public interface IResource { 
        public string Name { get; }
        public ISet<string> Tags { get; }
    }
}
