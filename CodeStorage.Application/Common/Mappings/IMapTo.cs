using AutoMapper;

namespace CodeStorage.Application.Common;

public interface IMapTo<T>
{
    void Mapping(Profile profile) => profile.CreateMap(GetType(),typeof(T));
}