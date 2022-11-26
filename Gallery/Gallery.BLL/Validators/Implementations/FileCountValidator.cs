using Gallery.BLL.Validators.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BLL.Validators.Implementations
{
    public class EnumerableMaxCountValidator<T> : IValidator
    {
        private readonly int _count;
        private readonly IEnumerable<T> _list;

        public EnumerableMaxCountValidator(IEnumerable<T> list, int count)
        {
            _count = count;
            _list = list;
        }

        public bool Validate()
        {
            return _list.Count() <= _count;
        }
    }
}
