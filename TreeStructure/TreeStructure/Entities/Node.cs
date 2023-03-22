﻿namespace TreeStructure.Models
{
    public class Node
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public virtual ICollection<Node> Children { get; set; }
    }
}
