using AutoMapper;
using Gallery.BLL.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BLL.TypeConverters
{
    public class EnumerableWithTotalConverter<TSource, TDest> : ITypeConverter<IEnumerable<TSource>, EnumerableWithTotalViewModel<TDest>>
    {
        public EnumerableWithTotalViewModel<TDest> Convert(IEnumerable<TSource> source, EnumerableWithTotalViewModel<TDest> destination, ResolutionContext context)
        {
            var mappedList = context.Mapper.Map<IEnumerable<TSource>, IEnumerable<TDest>>(source);
            return new EnumerableWithTotalViewModel<TDest>
            {
                Values = mappedList,
                Total = source.Count()
            };
        }
    }
}
