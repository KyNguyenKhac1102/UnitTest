using System;
using System.Collections.Generic;
using System.Linq;
using MVC.Models;

namespace MVC.Services
{
    public class MemberService : IMemeberServices
    {

        static List<Member> memberList = new List<Member>{
            new Member{Id = Guid.Parse("bce9010c-4c8c-4447-a618-3cc9b4cb2b7d"),
                        FirstName = "Ky",
                        LastName = "Nguyen Khac",
                        Gender = "Male",
                        DateOfBirth = new DateTime(1999, 11, 12),
                        PhoneNumber = "0222.222.222",
                        BirthPlace = "Ha Noi",
                        IsGraduated = false},
            new Member{Id = Guid.Parse("7fb11d52-9770-4977-a881-ba3f29ee75c9"),
                        FirstName = "Trang",
                        LastName = "Huyen Nguyen",
                        Gender = "Female",
                        DateOfBirth = new DateTime(2002, 05, 21),
                        PhoneNumber = "0222.222.222",
                        BirthPlace = "Hai Phong",
                        IsGraduated = false},
            new Member{Id = Guid.NewGuid(),
                        FirstName = "Tuan",
                        LastName = "Trinh Dat",
                        Gender = "Male",
                        DateOfBirth = new DateTime(1994, 01, 15),
                        PhoneNumber = "0222.222.222",
                        BirthPlace = "Bac Ninh",
                        IsGraduated = false},
            new Member{Id = Guid.NewGuid(),
                        FirstName = "Cong",
                        LastName = "Nguyen Van",
                        Gender = "Male",
                        DateOfBirth = new DateTime(1996, 08, 12),
                        PhoneNumber = "0222.222.222",
                        BirthPlace = "Ha Noi",
                        IsGraduated = false},
            new Member{Id = Guid.NewGuid(),
                        FirstName = "Phuoc",
                        LastName = "Hoang Nhat",
                        Gender = "Male",
                        DateOfBirth = new DateTime(1998, 06, 18),
                        PhoneNumber = "0222.222.222",
                        BirthPlace = "Bac Ninh",
                        IsGraduated = false},
            new Member{Id = Guid.NewGuid(),
                        FirstName = "Long",
                        LastName = "Thang Bao",
                        Gender = "Male",
                        DateOfBirth = new DateTime(2000, 07, 12),
                        PhoneNumber = "0222.222.222",
                        BirthPlace = "Binh Duong",
                        IsGraduated = false},
        };

        public void Create(Member member)
        {
            memberList.Add(member);
        }
        public void Update(Member member)
        {
            var model = memberList.FirstOrDefault(x => x.Id == member.Id);
            model.FirstName = member.FirstName;
            model.LastName = member.LastName;
            model.Gender = member.Gender;
            model.DateOfBirth = member.DateOfBirth;
            model.Email = member.Email;
        }
        public void Delete(Guid id)
        {
            var model = memberList.FirstOrDefault(x => x.Id == id);
            memberList.Remove(model);
        }
        public List<Member> AllMembers()
        {
            return memberList;
        }

        public Member GetMemberById(Guid id)
        {
            Member member = memberList.FirstOrDefault(x => x.Id == id);
            return member;
        }
    }
}