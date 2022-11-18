using Gallery.BLL.Validators.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BLL.Validators.Implementations
{
    public class PngValidator : IValidator
    {
        private readonly string path;

        public PngValidator(string path)
        {
            this.path = path;
        }

        public bool Validate()
        {
            throw new NotImplementedException();
        }
    }
}
