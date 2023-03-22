using AutoMapper;
using Microsoft.AspNetCore.Routing;
using TreeStructure.Exceptions;
using TreeStructure.Models;

namespace TreeStructure.Services
{
    public class TreeService : ITreeService
    {
        private readonly TreeContext _context;
        private readonly IMapper _mapper;

        public TreeService(TreeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<NodeDTO> GetAll(NodeQuery query)
        {
            var roots = _context.Nodes.Where(n => n.ParentId == null).ToList();

            if(roots.Count == 0)
            {
                throw new NotFoundException("No data found, try insert something.");
            }

            foreach (var root in roots)
            {
                GetChildren(root, query);
            }

            if (query.SortDirection == SortDirection.ASCENDING)
            {
                roots = roots.OrderBy(x => x.Name).ToList();
            }
            else
            {
                roots = roots.OrderByDescending(x => x.Name).ToList();
            }

            var rootsDTO = _mapper.Map<List<NodeDTO>>(roots);

            return rootsDTO;
        }

        public List<NodeDTO> GetById(int id, NodeQuery query)
        {
            var roots = _context.Nodes.Where(n => n.Id == id).ToList();

            if (roots.Count == 0)
            {
                throw new NotFoundException("Data not found.");
            }

            foreach (var root in roots)
            {
                GetChildren(root, query);
            }

            var rootsDTO = _mapper.Map<List<NodeDTO>>(roots);
            return rootsDTO;
        }

        private void GetChildren(Node node, NodeQuery query)
        {
            if (query.SortDirection == SortDirection.ASCENDING)
            {
                node.Children = _context.Nodes.Where(n => n.ParentId == node.Id).OrderBy(x => x.Name).ToList();
            }
            else
            {
                node.Children = _context.Nodes.Where(n => n.ParentId == node.Id).OrderByDescending(x => x.Name).ToList();
            }

            foreach (var child in node.Children)
            {
                GetChildren(child, query);
            }
        }

        public Node Create(NodeUpsertDTO nodeCreateDTO)
        {
            var node = _mapper.Map<Node>(nodeCreateDTO);
            _context.Nodes.Add(node);
            _context.SaveChanges();

            return node;
        }

        public Node Update(int id, NodeUpsertDTO nodeUpsertDTO)
        {
            var node = _context.Nodes.FirstOrDefault(x => x.Id == id);

            if (node == null)
            {
                throw new NotFoundException("The node with the given ID does not exist.");
            }

            node.Name = nodeUpsertDTO.Name;
            if (nodeUpsertDTO.ParentId != null)
            {
                node.ParentId = nodeUpsertDTO.ParentId;
            }
            _context.Nodes.Update(node);
            _context.SaveChanges();

            return node;
        }

        public void Delete(int id)
        {
            var node = _context.Nodes.Find(id);

            if (node == null)
            {
                throw new NotFoundException("The node with the given ID does not exist.");
            }

            var children = _context.Nodes.Where(n => n.ParentId == id).ToList();

            foreach (var child in children)
            {
                Delete(child.Id);
            }

            _context.Nodes.Remove(node);
            _context.SaveChanges();
        }

        public void MoveTreeNode(int nodeId, int newParentId)
        {
            var node = _context.Nodes.FirstOrDefault(x => x.Id == nodeId);
            if (node == null)
            {
                throw new NotFoundException("Node not found.");
            };

            var root = _context.Nodes.FirstOrDefault(x => x.Id == newParentId);

            if (root == null)
            {
                throw new NotFoundException("Node parent not found.");
            };

            var children = root;
            while (children != null)
            {
                if (children.Id == nodeId)
                {
                    throw new BadRequestException("Cannot move a node to its own children.");
                }
                children = _context.Set<Node>().Find(children.ParentId);
            }
            Node newNode = new() { Id = nodeId, Name = node.Name, ParentId = newParentId, Children = node.Children };

            _context.Remove(node);
            _context.Add(newNode);

            _context.SaveChanges();
        }
    }
}
