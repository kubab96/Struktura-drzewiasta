using TreeStructure.Models;

namespace TreeStructure.Services
{
    public interface ITreeService
    {
        Node Create(NodeUpsertDTO nodeCreateDTO);
        void Delete(int id);
        IEnumerable<NodeDTO> GetAll(NodeQuery query);
        List<NodeDTO> GetById(int id, NodeQuery query);
        void MoveTreeNode(int nodeId, int newParentId);
        Node Update(int id, NodeUpsertDTO nodeUpsertDTO);
    }
}