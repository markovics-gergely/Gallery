using Gallery.BLL.Validators.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BLL.Validators.Implementations
{
    public class NotCondition : IValidator
    {
        private readonly IValidator validator;

        public NotCondition(IValidator validator)
        {
            this.validator = validator;
        }

        public bool Validate()
        {
            return !validator.Validate();
        }
    }
}
