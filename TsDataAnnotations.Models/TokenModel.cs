using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsModelGeneratorLib.Attributes;

namespace TsDataAnnotations.Models
{
    [GenerateTypeScript]
    public partial class TokenModel : BaseViewModel
    {
        [ObservableProperty]
        private string _username = "";

        [ObservableProperty]
        private string [] _roles = [];

        [ObservableProperty]
        private DateTimeOffset _expires = DateTimeOffset.MinValue;

        [ObservableProperty]
        private string _token = "";

        [ObservableProperty]
        private string _message = "";
    }
}
