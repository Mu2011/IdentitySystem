using System.ComponentModel.DataAnnotations;

namespace IdentitySystem.Helpers;

public class ValidateDomainNameAttribute(string domainName) : ValidationAttribute 
{
  private readonly string _domainName = domainName;
  public override bool IsValid(object value)
  {
    string [] val = value.ToString().Split('@');
    return val[1].ToUpper() == domainName.ToUpper();
  }
}  
