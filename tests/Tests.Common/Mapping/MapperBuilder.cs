using AutoMapper;

namespace Tests.Common.Mapping;

public class MapperBuilder
{
    private readonly List<Type> _profiles = [];

    public Mapper Build()
    {
        MapperConfiguration mapperConfiguration = new(cfg =>
        {
            foreach (Type type in _profiles)
            {
                cfg.AddProfile(type);
            }
        });
        return new Mapper(mapperConfiguration);
    }

    public MapperBuilder WithProfile<T>()
    {
        _profiles.Add(typeof(T));
        return this;
    }
}