using System.ComponentModel.DataAnnotations;

namespace api.Entities
{
    public class Group
    {
        public Group(string name)
        {
            Name = name;
        }

        public Group(string name, ICollection<Connection> connections)
        {
            Name = name;
            Connections = connections;
        }

        [Key]
        public string Name { get; set; }

        public ICollection<Connection> Connections {get;set;} = new List<Connection>();

    }
}