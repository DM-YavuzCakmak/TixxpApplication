using System;
using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;

namespace Tixxp.Entities.Channel;

[Table("Channel")]
public class ChannelEntity : BaseEntity
{
    [Column("Name")]
    public string Name { get; set; }
}
