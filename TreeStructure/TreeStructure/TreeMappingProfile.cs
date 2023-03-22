using AutoMapper;
using TreeStructure.Models;

namespace TreeStructure
{
    public class TreeMappingProfile : Profile
    {
        public TreeMappingProfile() {
            CreateMap<Node, NodeDTO>();
            CreateMap<NodeUpsertDTO, Node>();
        }
    }
}
