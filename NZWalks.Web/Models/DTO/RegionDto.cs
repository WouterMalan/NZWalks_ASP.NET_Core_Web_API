using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NZWalks.Web.Models;
{
    public class RegionDto
{
    public Guid Id { get; set; }

    public string Code { get; set; }

    public string Name { get; set; }

    public string? RegionImageUrl { get; set; }

}
}