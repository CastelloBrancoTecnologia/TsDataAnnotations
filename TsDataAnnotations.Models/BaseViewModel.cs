using CommunityToolkit;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TsModelGeneratorLib.Attributes;

namespace TsDataAnnotations.Models;

[TypeScriptIgnore]
public partial class BaseViewModel : ObservableValidator
{
    public bool IsValid()
    {
        this.ValidateAllProperties();

        return !this.HasErrors;
    }

    public string ErrorMessages()
    {
        return string.Join(", \r\n", this.GetErrors().Select(x => x.ErrorMessage).ToArray());
    }
}
