using System.Net;
using TreeStructure.Models;

namespace TreeStructure
{
    public class TreeSeeder
    {
        private readonly TreeContext _dbContext;

        public TreeSeeder(TreeContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Nodes.Any())
                {
                    var nodes = GetNodes();
                    _dbContext.Nodes.AddRange(nodes);
                    _dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Node> GetNodes()
        {
            var nodes = new List<Node>()
            {
                new Node()
                {
                    Name = "Root",
                    Children = new List<Node>()
                    {
                        new Node()
                        {
                            Name = "Dokumenty",
                            ParentId = 1,
                        },
                        new Node()
                        {
                            Name = "Wideo",
                            ParentId = 1,
                        },
                        new Node()
                        {
                            Name = "Obrazki",
                            ParentId = 1,
                            Children = new List<Node>()
                            {
                                new Node()
                                {
                                    Name = "Moje zdjęcia",
                                    ParentId = 6,
                                },
                            }
                        },
                    },
                },
                new Node()
                {
                    Name = "Etc",
                    Children = new List<Node>()
                    {
                        new Node()
                        {
                            Name = "Default",
                            ParentId = 2,
                        },
                    },
                },
                new Node()
                {
                    Name = "Usr",
                    Children = new List<Node>()
                    {
                        new Node()
                        {
                            Name = "Bin",
                            ParentId = 3,
                        },
                        new Node()
                        {
                            Name = "Lib",
                            ParentId = 3,
                        },
                        new Node()
                        {
                            Name = "Disct",
                            ParentId = 3,
                        },
                    },
                },
            };
            return nodes;
        }
    }
}
