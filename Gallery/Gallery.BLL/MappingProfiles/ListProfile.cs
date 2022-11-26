using AutoMapper;
using Gallery.BLL.Infrastructure.ViewModels;
using Gallery.BLL.TypeConverters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BLL.MappingProfiles
{
    public class ListProfile : Profile
    {
        public ListProfile()
        {
            CreateMap(typeof(IEnumerable<>), typeof(EnumerableWithTotalViewModel<>)).ConvertUsing(typeof(EnumerableWithTotalConverter<,>));
        }
    }
}

