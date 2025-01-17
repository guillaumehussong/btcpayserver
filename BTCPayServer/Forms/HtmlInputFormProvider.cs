using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BTCPayServer.Abstractions.Form;
using BTCPayServer.Validation;

namespace BTCPayServer.Forms;

public class FieldValueMirror : IFormComponentProvider
{
    public string View { get; } = null;
    public void Validate(Form form, Field field)
    {
        if (form.GetFieldByFullName(field.Value) is null)
        {
            field.ValidationErrors = new List<string>() { $"{field.Name} requires {field.Value} to be present" };
        }
    }

    public void Register(Dictionary<string, IFormComponentProvider> typeToComponentProvider)
    {
        typeToComponentProvider.Add("mirror", this);
    }

    public string GetValue(Form form, Field field)
    {
        return form.GetFieldByFullName(field.Value)?.Value;
    }
}
public class HtmlInputFormProvider : FormComponentProviderBase
{
    public override void Register(Dictionary<string, IFormComponentProvider> typeToComponentProvider)
    {
        foreach (var t in new[] {
            "text",
            "radio",
            "checkbox",
            "password",
            "file",
            "hidden",
            "button",
            "submit",
            "color",
            "date",
            "datetime-local",
            "month",
            "week",
            "time",
            "email",
            "image",
            "number",
            "range",
            "search",
            "url",
            "tel",
            "reset"})
            typeToComponentProvider.Add(t, this);
    }
    public override string View => "Forms/InputElement";

    public override void Validate(Form form, Field field)
    {
        if (field.Required)
        {
            ValidateField<RequiredAttribute>(field);
        }
        if (field.Type == "email")
        {
            ValidateField<MailboxAddressAttribute>(field);
        }
    }
}
