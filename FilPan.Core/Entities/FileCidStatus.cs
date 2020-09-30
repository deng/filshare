using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FilPan.Services;

namespace FilPan.Entities
{
    public enum FileCidStatus : byte
    {
        None = 0,

        Success = 1
    }
}
