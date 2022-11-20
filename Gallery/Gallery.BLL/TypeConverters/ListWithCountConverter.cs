using AutoMapper;
using Gallery.BLL.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BLL.TypeConverters
{
    public class EnumerableWithCountConverter<TSource, TDest> : ITypeConverter<IEnumerable<TSource>, EnumerableWithCountViewModel<TDest>>
    {
        public EnumerableWithCountViewModel<TDest> Convert(IEnumerable<TSource> source, EnumerableWithCountViewModel<TDest> destination, ResolutionContext context)
        {
            var mappedList = context.Mapper.Map<IEnumerable<TSource>, IEnumerable<TDest>>(source);
            return new EnumerableWithCountViewModel<TDest>
            {
                Values = mappedList,
                Total = source.Count()
            };
        }
    }
}
