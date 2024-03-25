using System.ComponentModel.DataAnnotations;

﻿namespace Users.Dtos;

public class AvatarChangedDto
{
    [Url]
    [MaxLength(2048)]
    public string? AvatarHref { get; set; }
}
