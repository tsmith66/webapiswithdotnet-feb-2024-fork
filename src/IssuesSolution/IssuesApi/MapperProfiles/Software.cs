﻿using AutoMapper;
using IssuesApi.Features.Catalog;
using IssuesApi.Services;

namespace IssuesApi.MapperProfiles;

public class Software : Profile
{
    public Software()
    {
        // From SoftwareItem -> SoftwareCatalogSummaryResponseItem
        CreateMap<SoftwareItem, SoftwareCatalogSummaryResponseItem>();
        // .ForMember(dest => dest.Title, opts => opts.MapFrom(src => src.Title + " " + src.Version));
        CreateMap<SoftwareItemRequestModel, SoftwareItem>();

    }
}