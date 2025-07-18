using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System;

namespace PTLab7{

    
    [XmlRoot("Models")]

    public class SerializableModels
    {
        [XmlElement("Model")]
        public List<SerializableModel> Items { get; set; } = new List<SerializableModel>();
    }


    public class SerializableModel
    {
        [XmlIgnore]
        public int Id { get; set; }

        public string Name { get; set; }
        public float Value { get; set; }

        public AdditionalData AdditionalData { get; set; }
    }

    public class RecursiveModel {
        private static readonly Random _random = new Random(); //init rand

        public int Id { get; set; }
        public string Name { get; set; }
        public float Value { get; set; }

        public AdditionalData AdditionalData { get; set; }
        public ObservableCollection<RecursiveModel> Instances { get; set; }

        public RecursiveModel(int id, string name, float value)
        {
            Id = id;
            Name = name;
            Value = value;
            Instances = new ObservableCollection<RecursiveModel>();

            AdditionalData = new AdditionalData
            {
                Number = _random.Next(1, 11),
                Category = (Categories)_random.Next(0,Enum.GetValues(typeof(Categories)).Length)
            };
        }

        public RecursiveModel()
        {
            Id = 0;
            Name = "Root";
            Value = 0;
            AdditionalData = new AdditionalData
            {
                Number = 1,
                Category = Categories.CategoryA
            };
            Instances = new ObservableCollection<RecursiveModel>();
        }

        public void AddInstance(RecursiveModel instance)
        {
            if (instance != this)
                Instances.Add(instance);
        }

        public static List<RecursiveModel> GenerateRandomModels(int minRoots, int maxChilds)
        {
            var rand = new Random();
            var models = new List<RecursiveModel>();

            var roots = rand.Next(minRoots, minRoots+20);
           // var roots = rand.Next(minRoots, maxRoots);
            var count = 0;
            for (int i = 0; i < roots; i++)
            {
                models.Add(new RecursiveModel(count++, $"Root{i + 1}", (float)(rand.NextDouble() * 100)));
            }

            foreach (var model in models)
            {
                var childs = rand.Next(0, maxChilds);
                for (int i = 0; i<childs; i++)
                {
                    model.AddInstance(new RecursiveModel(count++, $"Child{i + 1}", (float)(rand.NextDouble() * 100)));
                }
            }

            return models;
        }

        public int countChilds()
        {
            int count = 0;
            foreach (var child in Instances)
            {
                count++;
                count += child.countChilds();
            }
            return count;
        }
        public override string ToString() => Name;
    }
}
