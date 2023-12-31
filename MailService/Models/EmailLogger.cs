﻿namespace MailService.Models
{
    public class EmailLogger
    {

        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public DateTime DateTime { get; set; } = DateTime.Now;

    }
}
