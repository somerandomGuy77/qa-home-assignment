using CardValidation.ViewModels;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text;
using System.Net;
using FluentAssertions;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using CardValidation.Core.Enums;
using Allure.Net.Commons;
using System.Text.Json;

namespace CardValidation.Tests.Controllers
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("Credit card validation (API tests)")]
    public class CardValidationControllerTests
    {
        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;

        [SetUp]
        public void SetUp()
        {
            _factory = new WebApplicationFactory<Program>();
            _client = _factory.CreateClient();
        }

        [AllureName("Validate Credit Card - Valid Visa Card - Returns Ok")]
        [Test]
        public async Task ValidateCreditCard_ValidVisaCard_ReturnsOk()
        {
            var validCard = new CreditCard
            {
                Owner = "John Doe",
                Number = "4111111111111111",
                Date = "06/30",
                Cvv = "123"
            };

            var content = new StringContent(JsonSerializer.Serialize(validCard), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/cardvalidation/card/credit/validate", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            AllureApi.AddAttachment("request json", "application/json", Encoding.UTF8.GetBytes(JsonSerializer.Serialize(validCard)));
            AllureApi.AddAttachment("response json", "application/json", Encoding.UTF8.GetBytes(responseContent));

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            Assert.That(responseContent, Does.Contain(PaymentSystemType.Visa.ToString()));
        }

        [AllureName("Validate Credit Card - Valid MasterCard - Returns Ok")]
        [Test]
        public async Task ValidateCreditCard_ValidMasterCard_ReturnsOk()
        {
            var validCard = new CreditCard
            {
                Owner = "John Doe",
                Number = "5105105105105100",
                Date = "06/30",
                Cvv = "123"
            };

            var content = new StringContent(JsonSerializer.Serialize(validCard), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/cardvalidation/card/credit/validate", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            AllureApi.AddAttachment("request json", "application/json", Encoding.UTF8.GetBytes(JsonSerializer.Serialize(validCard)));
            AllureApi.AddAttachment("response json", "application/json", Encoding.UTF8.GetBytes(responseContent));

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            Assert.That(responseContent, Does.Contain(PaymentSystemType.MasterCard.ToString()));
        }

        [AllureName("Validate Credit Card - Valid American Express Card - Returns Ok")]
        [Test]
        public async Task ValidateCreditCard_ValidAmericanExpressCard_ReturnsOk()
        {
            var validCard = new CreditCard
            {
                Owner = "John Doe",
                Number = "340000000000009",
                Date = "06/30",
                Cvv = "123"
            };

            var content = new StringContent(JsonSerializer.Serialize(validCard), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/cardvalidation/card/credit/validate", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            AllureApi.AddAttachment("request json", "application/json", Encoding.UTF8.GetBytes(JsonSerializer.Serialize(validCard)));
            AllureApi.AddAttachment("response json", "application/json", Encoding.UTF8.GetBytes(responseContent));

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            Assert.That(responseContent, Does.Contain(PaymentSystemType.AmericanExpress.ToString()));
        }

        [AllureName("Validate Credit Card - Invalid Owner - Empty - Returns BadRequest")]
        [Test]
        public async Task ValidateCreditCard_InvalidOwner_Empty_ReturnsBadRequest()
        {
            var invalidCard = new CreditCard
            {
                Owner = "", // Invalid (empty owner)
                Number = "4111111111111111",
                Date = "12/30",
                Cvv = "123"
            };

            var content = new StringContent(JsonSerializer.Serialize(invalidCard), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/cardvalidation/card/credit/validate", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            AllureApi.AddAttachment("request json", "application/json", Encoding.UTF8.GetBytes(JsonSerializer.Serialize(invalidCard)));
            AllureApi.AddAttachment("response json", "application/json", Encoding.UTF8.GetBytes(responseContent));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            responseContent.Should().Contain("Owner is required");
        }

        [AllureName("Validate Credit Card - Invalid Owner - Null - Returns BadRequest")]
        [Test]
        public async Task ValidateCreditCard_InvalidOwner_Null_ReturnsBadRequest()
        {
            var invalidCard = new CreditCard
            {
                Owner = null, // Invalid (null owner)
                Number = "4111111111111111",
                Date = "12/30",
                Cvv = "123"
            };

            var content = new StringContent(JsonSerializer.Serialize(invalidCard), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/cardvalidation/card/credit/validate", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            AllureApi.AddAttachment("request json", "application/json", Encoding.UTF8.GetBytes(JsonSerializer.Serialize(invalidCard)));
            AllureApi.AddAttachment("response json", "application/json", Encoding.UTF8.GetBytes(responseContent));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            responseContent.Should().Contain("Owner is required");
        }

        [AllureName("Validate Credit Card - Invalid Owner - Format - Returns BadRequest")]
        [Test]
        public async Task ValidateCreditCard_InvalidOwner_Format_ReturnsBadRequest()
        {
            var invalidCard = new CreditCard
            {
                Owner = "12345", // Invalid (numbers in name)
                Number = "4111111111111111",
                Date = "12/30",
                Cvv = "123"
            };

            var content = new StringContent(JsonSerializer.Serialize(invalidCard), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/cardvalidation/card/credit/validate", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            AllureApi.AddAttachment("request json", "application/json", Encoding.UTF8.GetBytes(JsonSerializer.Serialize(invalidCard)));
            AllureApi.AddAttachment("response json", "application/json", Encoding.UTF8.GetBytes(responseContent));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            responseContent.Should().Contain("Wrong owner");
        }

        [AllureName("Validate Credit Card - Invalid Number - Empty - Returns BadRequest")]
        [Test]
        public async Task ValidateCreditCard_InvalidNumber_Empty_ReturnsBadRequest()
        {
            var invalidCard = new CreditCard
            {
                Owner = "John Doe",
                Number = "", // Invalid (empty number)
                Date = "12/30",
                Cvv = "123"
            };

            var content = new StringContent(JsonSerializer.Serialize(invalidCard), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/cardvalidation/card/credit/validate", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            AllureApi.AddAttachment("request json", "application/json", Encoding.UTF8.GetBytes(JsonSerializer.Serialize(invalidCard)));
            AllureApi.AddAttachment("response json", "application/json", Encoding.UTF8.GetBytes(responseContent));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            responseContent.Should().Contain("Number is required");
        }

        [AllureName("Validate Credit Card - Invalid Number - Format - Returns BadRequest")]
        [Test]
        public async Task ValidateCreditCard_InvalidNumber_Format_ReturnsBadRequest()
        {
            var invalidCard = new CreditCard
            {
                Owner = "John Doe",
                Number = "12345", // Invalid (incorrect format)
                Date = "12/30",
                Cvv = "123"
            };

            var content = new StringContent(JsonSerializer.Serialize(invalidCard), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/cardvalidation/card/credit/validate", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            AllureApi.AddAttachment("request json", "application/json", Encoding.UTF8.GetBytes(JsonSerializer.Serialize(invalidCard)));
            AllureApi.AddAttachment("response json", "application/json", Encoding.UTF8.GetBytes(responseContent));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            responseContent.Should().Contain("Wrong number");
        }

        [AllureName("Validate Credit Card - Invalid Date - Empty - Returns BadRequest")]
        [Test]
        public async Task ValidateCreditCard_InvalidDate_Empty_ReturnsBadRequest()
        {
            var invalidCard = new CreditCard
            {
                Owner = "John Doe",
                Number = "4111111111111111",
                Date = "", // Invalid (empty date)
                Cvv = "123"
            };

            var content = new StringContent(JsonSerializer.Serialize(invalidCard), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/cardvalidation/card/credit/validate", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            AllureApi.AddAttachment("request json", "application/json", Encoding.UTF8.GetBytes(JsonSerializer.Serialize(invalidCard)));
            AllureApi.AddAttachment("response json", "application/json", Encoding.UTF8.GetBytes(responseContent));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            responseContent.Should().Contain("Date is required");
        }

        [AllureName("Validate Credit Card - Invalid Date - Format - Returns BadRequest")]
        [Test]
        public async Task ValidateCreditCard_InvalidDate_Format_ReturnsBadRequest()
        {
            var invalidCard = new CreditCard
            {
                Owner = "John Doe",
                Number = "4111111111111111",
                Date = "99/99", // Invalid (incorrect format)
                Cvv = "123"
            };

            var content = new StringContent(JsonSerializer.Serialize(invalidCard), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/cardvalidation/card/credit/validate", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            AllureApi.AddAttachment("request json", "application/json", Encoding.UTF8.GetBytes(JsonSerializer.Serialize(invalidCard)));
            AllureApi.AddAttachment("response json", "application/json", Encoding.UTF8.GetBytes(responseContent));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            responseContent.Should().Contain("Wrong date");
        }

        [AllureName("Validate Credit Card - Invalid Cvv - Empty - Returns BadRequest")]
        [Test]
        public async Task ValidateCreditCard_InvalidCvv_Empty_ReturnsBadRequest()
        {
            var invalidCard = new CreditCard
            {
                Owner = "John Doe",
                Number = "4111111111111111",
                Date = "12/30",
                Cvv = "" // Invalid (empty CVV)
            };

            var content = new StringContent(JsonSerializer.Serialize(invalidCard), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/cardvalidation/card/credit/validate", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            AllureApi.AddAttachment("request json", "application/json", Encoding.UTF8.GetBytes(JsonSerializer.Serialize(invalidCard)));
            AllureApi.AddAttachment("response json", "application/json", Encoding.UTF8.GetBytes(responseContent));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            responseContent.Should().Contain("Cvv is required");
        }

        [AllureName("Validate Credit Card - Invalid Cvv - Length - Returns BadRequest")]
        [Test]
        public async Task ValidateCreditCard_InvalidCvv_Length_ReturnsBadRequest()
        {
            var invalidCard = new CreditCard
            {
                Owner = "John Doe",
                Number = "4111111111111111",
                Date = "12/30",
                Cvv = "12" // Invalid (CVV is too short)
            };

            var content = new StringContent(JsonSerializer.Serialize(invalidCard), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/cardvalidation/card/credit/validate", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            AllureApi.AddAttachment("request json", "application/json", Encoding.UTF8.GetBytes(JsonSerializer.Serialize(invalidCard)));
            AllureApi.AddAttachment("response json", "application/json", Encoding.UTF8.GetBytes(responseContent));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            responseContent.Should().Contain("Wrong cvv");
        }

        [AllureName("Validate Credit Card - Invalid Visa Cvv Length - Returns BadRequest")]
        [Test]
        public async Task ValidateCreditCard_InvalidVisaCvv_Length_ReturnsBadRequest()
        {
            var invalidCard = new CreditCard
            {
                Owner = "John Doe",
                Number = "4111111111111111", // Visa
                Date = "12/30",
                Cvv = "1234" // Invalid (CVV is too long for Visa)
            };

            var content = new StringContent(JsonSerializer.Serialize(invalidCard), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/cardvalidation/card/credit/validate", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            AllureApi.AddAttachment("request json", "application/json", Encoding.UTF8.GetBytes(JsonSerializer.Serialize(invalidCard)));
            AllureApi.AddAttachment("response json", "application/json", Encoding.UTF8.GetBytes(responseContent));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            responseContent.Should().Contain("Wrong cvv");
        }

        [AllureName("Validate Credit Card - Invalid Mastercard Cvv Length - Returns BadRequest")]
        [Test]
        public async Task ValidateCreditCard_InvalidMastercardCvv_Length_ReturnsBadRequest()
        {
            var invalidCard = new CreditCard
            {
                Owner = "John Doe",
                Number = "5105105105105100", // Mastercard
                Date = "12/30",
                Cvv = "1234" // Invalid (CVV is too long for Mastercard)
            };

            var content = new StringContent(JsonSerializer.Serialize(invalidCard), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/cardvalidation/card/credit/validate", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            AllureApi.AddAttachment("request json", "application/json", Encoding.UTF8.GetBytes(JsonSerializer.Serialize(invalidCard)));
            AllureApi.AddAttachment("response json", "application/json", Encoding.UTF8.GetBytes(responseContent));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            responseContent.Should().Contain("Wrong cvv");
        }

        [AllureName("Validate Credit Card - Invalid American Express Cvv Length - Returns BadRequest")]
        [Test]
        public async Task ValidateCreditCard_InvalidAericanExpressCvv_Length_ReturnsBadRequest()
        {
            var invalidCard = new CreditCard
            {
                Owner = "John Doe",
                Number = "340000000000009", // American Express
                Date = "12/30",
                Cvv = "123" // Invalid (CVV is too short for American Express)
            };

            var content = new StringContent(JsonSerializer.Serialize(invalidCard), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/cardvalidation/card/credit/validate", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            AllureApi.AddAttachment("request json", "application/json", Encoding.UTF8.GetBytes(JsonSerializer.Serialize(invalidCard)));
            AllureApi.AddAttachment("response json", "application/json", Encoding.UTF8.GetBytes(responseContent));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            responseContent.Should().Contain("Wrong cvv");
        }

        [AllureName("Validate Credit Card - Expired Date - Returns BadRequest")]
        [Test]
        public async Task ValidateCreditCard_ExpiredDate_ReturnsBadRequest()
        {
            var invalidCard = new CreditCard
            {
                Owner = "John Doe",
                Number = "4111111111111111",
                Date = "10/24", // Invalid (date expired)
                Cvv = "123"
            };

            var content = new StringContent(JsonSerializer.Serialize(invalidCard), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/cardvalidation/card/credit/validate", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            AllureApi.AddAttachment("request json", "application/json", Encoding.UTF8.GetBytes(JsonSerializer.Serialize(invalidCard)));
            AllureApi.AddAttachment("response json", "application/json", Encoding.UTF8.GetBytes(responseContent));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            responseContent.Should().Contain("Wrong date");
        }

        [AllureName("Validate Credit Card - Invalid Card Number - Returns BadRequest")]
        [Test]
        public async Task ValidateCreditCard_InvalidCardNumber_ReturnsBadRequest()
        {
            var invalidCard = new CreditCard
            {
                Owner = "John Doe",
                Number = "123123123123123123", // Invalid card provider
                Date = "10/30",
                Cvv = "123"
            };

            var content = new StringContent(JsonSerializer.Serialize(invalidCard), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/cardvalidation/card/credit/validate", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            AllureApi.AddAttachment("request json", "application/json", Encoding.UTF8.GetBytes(JsonSerializer.Serialize(invalidCard)));
            AllureApi.AddAttachment("response json", "application/json", Encoding.UTF8.GetBytes(responseContent));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            responseContent.Should().Contain("Wrong number");
        }

        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }
}
