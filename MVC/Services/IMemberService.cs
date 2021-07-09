using System;
using System.Collections.Generic;
using MVC.Models;

namespace MVC.Services
{
    public interface IMemeberServices
    {
        List<Member> AllMembers();
        Member GetMemberById(Guid id);
        void Create(Member member);

        void Update(Member member);
        void Delete(Guid id);
    }
}