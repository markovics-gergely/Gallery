using Gallery.BLL.Validators.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BLL.Validators.Implementations
{
    public class SvgValidator : IValidator
    {
        private readonly string path;

        public SvgValidator(string path)
        {
            this.path = path;
        }

        public bool Validate()
        {
            throw new NotImplementedException();
        }
    }
}
