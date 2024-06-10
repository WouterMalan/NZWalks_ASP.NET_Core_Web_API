using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NZWalks.API.Models.DTO
{
    public class UpdateWalkRequestDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Name must be less than 100 characters long")]
        public string Name { get; set; }

        [Required]
        [MaxLength(1000, ErrorMessage = "Description must be less than 500 characters long")]
        public string Description { get; set; }

        [Required]
        [Range(0, 50, ErrorMessage = "Length must be between 0 and 50 km")]
        public double LengthInKm { get; set; }

        public string? WalkImageUrl { get; set; }

        [Required]
        public Guid DifficultyId { get; set; }

        [Required]
        public Guid RegionId { get; set; }
    }
}