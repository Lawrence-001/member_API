﻿namespace MemberTestAPI.Dtos
{
    public class ConfirmEmailDto
    {
        public string UserId { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
