﻿using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Sessions
{
    public class AddSessionDTO
    {
        [Required]
        public DateTime Date { get; set; }
    }
}
