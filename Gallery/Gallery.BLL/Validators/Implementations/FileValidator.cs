using Gallery.BLL.Validators.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BLL.Validators.Implementations
{
    public class FileValidator : IValidator
    {
        private readonly string filePath;
        private readonly string extension;

        public FileValidator(string filePath, string extension)
        {
            this.filePath = filePath;
            this.extension = extension.ToLower();
        }

        public IValidator GetValidatorForFile()
        {
            IValidator fileValidator = extension switch
            {
                ".jpg" => new JpegValidator(filePath),
                ".png" => new PngValidator(filePath),
                ".svg" => new SvgValidator(filePath),
                _ => throw new ArgumentException()
            };
            return fileValidator;
        }

        public bool Validate()
        {
            return GetValidatorForFile().Validate();
        }
    }
}
