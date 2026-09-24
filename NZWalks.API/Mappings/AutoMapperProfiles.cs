using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // Map classes.
            /*CreateMap< Region, RegionDto >()
                .ForMember( x => x.Name, opt => opt.MapFrom( x => x.Name ) )
                .ReverseMap();*/
            CreateMap< Region, RegionDto >().ReverseMap();
            CreateMap< AddRegionRequestDto, Region >().ReverseMap();
            CreateMap< UpdateRegionRequestDto, Region >().ReverseMap();
            CreateMap< AddWalkRequestDto, Walk >().ReverseMap();
            CreateMap< WalkDto, Walk >().ReverseMap();
            CreateMap< DifficultyDto, Difficulty >().ReverseMap();
            CreateMap< UpdateWalkRequestDto, Walk >().ReverseMap();
        }
    }
}
