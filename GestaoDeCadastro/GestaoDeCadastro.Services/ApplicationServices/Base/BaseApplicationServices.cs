using System.ComponentModel.DataAnnotations;

namespace GestaoDeCadastro.Services.ApplicationServices.Base
{
    public class BaseApplicationServices
    {
        public void ValidateObj(object obj)
        {
            var context = new ValidationContext(obj, null, null);
            var result = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, context, result, true);
            string msg = "";

            if (!isValid)
            {
                foreach (var x in result)
                {
                    msg += x.ErrorMessage + " \r\n";
                }
                throw new Exception(msg);
            }
        }
    }
}
