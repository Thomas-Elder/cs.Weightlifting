﻿namespace DTO
{
    public class ResponseDTO
    {
        public bool Success { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
