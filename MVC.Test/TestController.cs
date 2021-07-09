using System;
using System.Collections.Generic;
using MVC.Controllers;
using MVC.Models;
using MVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace Controller.Test
{
    public class MemberControllerTest
    {
        static List<Member> _data = new List<Member>{
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
        private Mock<ILogger<MemberController>> _loggerMock;

        private Mock<IMemeberServices> _memberServiceMock;

        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<MemberController>>();

            _memberServiceMock = new Mock<IMemeberServices>();

            _memberServiceMock.Setup(x => x.AllMembers()).Returns(_data);
        }

        [Test]
        public void Rookies_ReturnsAViewResult_WithAListOfMembers()
        {
            // Arrange
            var controller = new MemberController(_loggerMock.Object, _memberServiceMock.Object);

            // Act
            var result = controller.Rookies("");

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var view = (ViewResult)result;
            Assert.IsAssignableFrom<List<Member>>(view.ViewData.Model);

            var list = (List<Member>)view.ViewData.Model;
            Assert.AreEqual(6, list.Count);

        }

        [Test]
        public void Rookies_ReturnsAViewResult_WithAListOfMaleMembers()
        {
            // Arrange
            var controller = new MemberController(_loggerMock.Object, _memberServiceMock.Object);

            // Act
            var result = controller.Rookies("Male");

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = (ViewResult)result;

            Assert.IsAssignableFrom<List<Member>>(viewResult.ViewData.Model);

            var maleListActual = (List<Member>)viewResult.ViewData.Model;
            var maleListExpected = _data.Where(x => x.Gender == "Male");

            Assert.AreEqual(maleListExpected, maleListActual);
        }

        [Test]
        public void Create_ReturnsRedirect_WithValidInput()
        {

            var memberController = new MemberController(_loggerMock.Object, _memberServiceMock.Object);
            var valid = memberController.ModelState.IsValid;
            var memberTest = new Member()
            {
                FirstName = "pepe the frog",
                LastName = "pop",
                Gender = "Female",
                DateOfBirth = new DateTime(1999, 11, 12),
                Email = "xxxPEPExxx@gmail.com"
            };

            var result = (RedirectToActionResult)memberController.Create(memberTest);

            Assert.IsTrue(valid);
            Assert.AreEqual("Rookies", result.ActionName);
            //same controller is null
            Assert.IsNull(result.ControllerName);

        }

        [Test]
        public void Create_ReturnsSameViewResultWithError_WhenModelStateIsNotValid()
        {

            var memberController = new MemberController(_loggerMock.Object, _memberServiceMock.Object);
            var memberTest = new Member()
            {
                FirstName = "",
                LastName = "",
                Gender = "",
                DateOfBirth = new DateTime(),
                Email = ""
            };

            Assert.IsTrue(ValidateModel(memberTest).Any(
                v => v.MemberNames.Contains("FirstName") && v.ErrorMessage.Contains("required")
            ));
            Assert.IsTrue(ValidateModel(memberTest).Any(
               v => v.MemberNames.Contains("LastName") && v.ErrorMessage.Contains("required")
           ));
            Assert.IsTrue(ValidateModel(memberTest).Any(
                v => v.MemberNames.Contains("Gender") && v.ErrorMessage.Contains("required")
            ));
            Assert.IsTrue(ValidateModel(memberTest).Any(
            v => v.MemberNames.Contains("Email") && v.ErrorMessage.Contains("required")
        ));

        }


        [Test]
        public void Edit_ReturnsRightObject_WhenGivenValidId()
        {
            Guid memberId = Guid.Parse("bce9010c-4c8c-4447-a618-3cc9b4cb2b7d");
            var memberController = new MemberController(_loggerMock.Object, _memberServiceMock.Object);

            var result = (ViewResult)memberController.Edit(memberId);
            var expectedObject = (Member)result.ViewData.Model;
            var actualObject = _data.FirstOrDefault(x => x.Id == memberId);

            Assert.AreEqual(expectedObject.Id, actualObject.Id);
        }

        [Test]
        public void Edit_ReturnsNotFound_WhenGivenInValidId()
        {
            Guid memberId = Guid.Parse("bce9010c-4c8c-4447-a618-3cc9b4cb2b73");
            var memberController = new MemberController(_loggerMock.Object, _memberServiceMock.Object);

            var result = (NotFoundResult)memberController.Edit(memberId);
            Assert.IsInstanceOf<NotFoundResult>(result);

        }
        [Test]
        public void Edit_ReturnsRedirect_WhenModelStateIsValid()
        {
            var memberTest = new Member()
            {
                FirstName = "das",
                LastName = "dsa",
                Gender = "Male",
                DateOfBirth = new DateTime(1999, 11, 11),
                Email = "xxxDSADxxx@gmail.com"
            };
            var memberController = new MemberController(_loggerMock.Object, _memberServiceMock.Object);

            var result = memberController.Edit(memberTest);
            Assert.IsInstanceOf<RedirectToActionResult>(result);

        }
        [Test]
        public void Edit_ReturnsSameValueWithError_WhenModelStateInValid()
        {
            var memberTest = new Member()
            {
                FirstName = "",
                LastName = "",
                Gender = "",
                DateOfBirth = new DateTime(1999, 11, 11),
                Email = "xxxDSADxxx"
            };
            var memberController = new MemberController(_loggerMock.Object, _memberServiceMock.Object);

            Assert.IsTrue(ValidateModel(memberTest).Any(
                v => v.MemberNames.Contains("FirstName") && v.ErrorMessage.Contains("required")
            ));
            Assert.IsTrue(ValidateModel(memberTest).Any(
               v => v.MemberNames.Contains("LastName") && v.ErrorMessage.Contains("required")
           ));
            Assert.IsTrue(ValidateModel(memberTest).Any(
                v => v.MemberNames.Contains("Gender") && v.ErrorMessage.Contains("required")
            ));


        }
        [Test]
        public void Detail_ReturnsHTTPNotFound_WhenInputInvalId()
        {
            // invalid id
            Guid memberId = Guid.Parse("bce9010c-4c8c-4447-a618-3cc9b4cb2b71");

            _memberServiceMock.Setup(x => x.GetMemberById(memberId)).Returns(_data.FirstOrDefault(x => x.Id == memberId));

            // Arrange
            var controller = new MemberController(_loggerMock.Object, _memberServiceMock.Object);

            // Act
            var result = controller.Detail(memberId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);

        }

        [Test]
        public void Detail_ReturnsObject_WhenGivenValidId()
        {
            // Valid id
            Guid memberId = Guid.Parse("bce9010c-4c8c-4447-a618-3cc9b4cb2b7d");

            _memberServiceMock.Setup(x => x.GetMemberById(memberId)).Returns(_data.FirstOrDefault(x => x.Id == memberId));

            // Arrange
            var controller = new MemberController(_loggerMock.Object, _memberServiceMock.Object);

            // Act
            var result = controller.Detail(memberId);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = (ViewResult)result;

            var expectedObject = (Member)viewResult.ViewData.Model;
            var actualObject = _data.FirstOrDefault(x => x.Id == memberId);
            Assert.AreEqual(expectedObject.Id, actualObject.Id);
        }

        [Test]
        public void Delete_ReturnsRedirect_WhenGivenValidId(){
            Guid memberId = Guid.Parse("bce9010c-4c8c-4447-a618-3cc9b4cb2b7d");

            var memberController = new MemberController(_loggerMock.Object, _memberServiceMock.Object);

            var result = memberController.Delete(memberId);

            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }
        [Test]
        public void Delete_ReturnsNotFound_WhenGivenInValidId()
        {
            Guid memberId = Guid.Parse("bce9010c-4c8c-4447-a618-3cc9b4cb2b74");
            var memberController = new MemberController(_loggerMock.Object, _memberServiceMock.Object);

            var result = memberController.Delete(memberId);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}