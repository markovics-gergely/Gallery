using Gallery.BLL.Validators.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BLL.Validators.Implementations
{
    public class AndCondition : IValidator
    {
        private readonly IValidator validator1;
        private readonly IValidator validator2;

        public AndCondition(IValidator validator1, IValidator validator2) 
        {
            this.validator1 = validator1;
            this.validator2 = validator2;
        }

        public bool Validate()
        {
            return validator1.Validate() && validator2.Validate(); 
        }
    }
}
