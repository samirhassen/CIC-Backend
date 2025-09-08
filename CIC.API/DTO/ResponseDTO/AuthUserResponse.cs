using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public class PaWebRole
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}

public class AuthUserResponse
{
    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("pa_webloginname")]
    public string PaWebLoginName { get; set; }

    [JsonPropertyName("emailaddress1")]
    public string EmailAddress1 { get; set; }

    [JsonPropertyName("contactid")]
    public string ContactId { get; set; }

    [JsonPropertyName("pa_member")]
    public bool PaMember { get; set; }

    [JsonPropertyName("pa_contactnumber")]
    public string PaContactNumber { get; set; }

    [JsonPropertyName("fullname")]
    public string FullName { get; set; }

    [JsonPropertyName("parentcustomerid_id")]
    public string ParentCustomerId_Id { get; set; }

    [JsonPropertyName("parentcustomerid")]
    public string ParentCustomerId { get; set; }

    [JsonPropertyName("firstname")]
    public string FirstName { get; set; }

    [JsonPropertyName("defaultpricelevelid_id")]
    public string DefaultPriceLevelId_Id { get; set; }

    [JsonPropertyName("defaultpricelevelid")]
    public string DefaultPriceLevelId { get; set; }

    [JsonPropertyName("pa_labelname")]
    public string PaLabelName { get; set; }

    [JsonPropertyName("lastname")]
    public string LastName { get; set; }

    [JsonPropertyName("pa_token")]
    public string PaToken { get; set; }

    [JsonPropertyName("pa_webroles")]
    public List<PaWebRole> PaWebRoles { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }
}
