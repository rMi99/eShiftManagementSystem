using System;

namespace eShiftManagementSystem.Models
{
    public class AuditLog
    {
        public int AuditId { get; set; }
        public string TableName { get; set; } = string.Empty;
        public int RecordId { get; set; }
        public string ActionType { get; set; } = string.Empty; // INSERT, UPDATE, DELETE
        public string? OldValues { get; set; } // JSON string
        public string? NewValues { get; set; } // JSON string
        public int? ChangedBy { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation property
        public User? ChangedByUser { get; set; }

        // Computed properties
        public string DisplayAction => ActionType switch
        {
            "INSERT" => "Created",
            "UPDATE" => "Updated",
            "DELETE" => "Deleted",
            _ => ActionType
        };
        
        public string FormattedDate => CreatedAt.ToString("dd/MM/yyyy HH:mm:ss");
        public string UserInfo => ChangedByUser?.DisplayName ?? "System";
        public string TableDisplayName => TableName.Replace("_", " ").ToTitleCase();
        public bool HasChangedData => !string.IsNullOrEmpty(OldValues) || !string.IsNullOrEmpty(NewValues);
        public string ShortIpAddress => IpAddress?.Split('.').Length > 0 ? 
            string.Join(".", IpAddress.Split('.').Take(2)) + ".xxx.xxx" : "Unknown";
    }
}

// Extension method for string conversion
public static class StringExtensions
{
    public static string ToTitleCase(this string input)
    {
        if (string.IsNullOrEmpty(input)) return string.Empty;
        
        var words = input.Split(' ');
        for (int i = 0; i < words.Length; i++)
        {
            if (words[i].Length > 0)
            {
                words[i] = char.ToUpper(words[i][0]) + 
                          (words[i].Length > 1 ? words[i].Substring(1).ToLower() : "");
            }
        }
        return string.Join(" ", words);
    }
}