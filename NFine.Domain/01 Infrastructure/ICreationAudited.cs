using System;

namespace NFine.Domain
{
    public interface ICreationAudited
    {
        string F_Id { get; set; }
        string F_CreatorUserId { get; set; }
        bool? F_EnabledMark { get; set; }
        DateTime? F_CreatorTime { get; set; }
    }
}