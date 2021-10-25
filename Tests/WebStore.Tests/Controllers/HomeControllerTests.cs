using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebStore.Controllers;

using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void Index_Returns_View()
        {
            var controller = new HomeController();

            var result = controller.Index();

            Assert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public void ContactUs_Returns_View()
        {
            var controller = new HomeController();

            var result = controller.ContactUs();

            Assert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public void Status_with_id_404_Returns_View()
        {
            // A-A-A = Arrange - Act - Assert

            #region Arrange

            const string id = "404";
            const string expected_view_name = "Error404";
            var controller = new HomeController();

            #endregion

            #region Act

            var result = controller.Status(id);

            #endregion

            #region Assert
            
            var view_result = Assert.IsType<ViewResult>(result);

            var actual_view_name = view_result.ViewName;

            Assert.Equal(expected_view_name, actual_view_name); 

            #endregion
        }

        [TestMethod]
        [DataRow("123")]
        [DataRow("QWE")]
        public void Status_with_id_Returns_View(string id)
        {
            //const string id = "123";
            var expected_content = "Status --- " + id;
            var controller = new HomeController();

            var result = controller.Status(id);

            var content_result = Assert.IsType<ContentResult>(result);

            var actual_content = content_result.Content;

            Assert.Equal(expected_content, actual_content);
            //AssertFailedException
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void Status_thrown_ArgumentNullException_when_id_is_null_1()
        {
            var controller = new HomeController();

            _ = controller.Status(null);
        }

        [TestMethod]
        public void Status_thrown_ArgumentNullException_when_id_is_null_2()
        {
            const string expecte_parameter_name = "id";
            var controller = new HomeController();

            Exception exception = null;
            try
            {
                _ = controller.Status(null);
            }
            catch (ArgumentNullException e)
            {
                exception = e;
            }
            if(exception is null)
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail();

            var actual_exception = Assert.IsType<ArgumentNullException>(exception);
            var actual_parameter_name = actual_exception.ParamName;
            Assert.Equal(expecte_parameter_name, actual_parameter_name);
        }

        [TestMethod]
        public void Status_thrown_ArgumentNullException_when_id_is_null_3()
        {
            const string expecte_parameter_name = "id";
            var controller = new HomeController();

            var actual_exception = Assert.Throws<ArgumentNullException>(() => controller.Status(null));
            var actual_parameter_name = actual_exception.ParamName;
            Assert.Equal(expecte_parameter_name, actual_parameter_name);
        }
    }
}
