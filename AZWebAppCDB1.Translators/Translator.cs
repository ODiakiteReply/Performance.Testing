using System;
using AutoMapper;
using AZWebAppCDB1.Models;
using Common.Extensions.Enum;
using AZWebAppCDB1.Common.Domain;
using AZWebAppCDB1.Common.Enums;

namespace AZWebAppCDB1.Translators
{
    public class Translator : Profile
    {
        public Translator()
        {
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src =>src.RoleId.ToString().ToEnum<RoleEnum>()));
            CreateMap<UserDTO, User>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Role.ToInt().ToString()));

            CreateMap<Post, PostDTO>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.CreatedBy));
            CreateMap<PostDTO, Post>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.Author))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt ?? DateTime.UtcNow));

            CreateMap<Comment, CommentDTO>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.CreatedBy));
            CreateMap<CommentDTO, Comment>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.Author));
        }
    }
}
