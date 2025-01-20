using CardValidation.Core.Services;
using CardValidation.Core.Enums;
using Allure.NUnit;
using Allure.NUnit.Attributes;

namespace CardValidation.Tests
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("Credit card validation (Unit tests)")]
    public class CardValidationServiceTests
    {
        private CardValidationService _cardValidationService;

        [SetUp]
        public void Setup()
        {
            _cardValidationService = new CardValidationService();
        }

        [AllureName("Validate Owner - Valid Owner - Returns True")]
        [TestCase("John Doe")]
        [TestCase("JOHN DOE")]
        [TestCase("John Doe Smith")]
        [TestCase("John-Doe Smith")]
        [TestCase("Mary Ann-Kennedy")]
        [TestCase("John O'Doe")]
        [TestCase("John")]
        [TestCase("jOhN dOe")]
        [TestCase("J. D. Smith")]
        [TestCase("José Hernández")]
        [TestCase("Mário López")]
        [TestCase("无名氏")]
        [TestCase("Γιάννης Παπαδόπουλος")]
        [TestCase("محمد علي")]
        public void ValidateOwner_ValidOwner_ReturnsTrue(string owner)
        {
            var result = _cardValidationService.ValidateOwner(owner);
            Assert.IsTrue(result);
        }

        [AllureName("Validate Owner - Invalid Owner - Returns False")]
        [TestCase("")]
        [TestCase("John123 Doe")]
        [TestCase("Dr. John Doe")]
        [TestCase("John_Doe")]
        [TestCase("John, Doe.")]
        [TestCase("Alexander Maximilian Frederick Wilhelm von Habsburg Lothringen Alexander Maximilian Frederick Wilhelm von Habsburg Lothringen")]
        [TestCase("John, Doe.")]
        [TestCase("John, Doe.")]
        [TestCase("John@Doe!")]
        [TestCase("John\tDoe")]
        [TestCase("1234")]
        [TestCase("!#¤%&()=?")]
        public void ValidateOwner_InvalidOwner_ReturnsFalse(string owner)
        {
            var result = _cardValidationService.ValidateOwner(owner);
            Assert.IsFalse(result);
        }

        [AllureName("Validate Issue Date - Valid Issue Date - Returns True")]
        [TestCase("01/2030")]
        [TestCase("02/2030")]
        [TestCase("03/2030")]
        [TestCase("04/2030")]
        [TestCase("05/2030")]
        [TestCase("06/2030")]
        [TestCase("07/2030")]
        [TestCase("08/2030")]
        [TestCase("09/2030")]
        [TestCase("10/2030")]
        [TestCase("11/2030")]
        [TestCase("12/2030")]
        [TestCase("12/2050")]
        [TestCase("12/9999")]
        public void ValidateIssueDate_ValidIssueDate_ReturnsTrue(string issueDate)
        {
            var result = _cardValidationService.ValidateIssueDate(issueDate);
            Assert.IsTrue(result);
        }

        [AllureName("Validate Issue Date - Valid Current Month/Year - Returns True")]
        [Test]
        public void ValidateIssueDate_ValidCurrentMonthYear_ReturnsTrue()
        {
            string currentMonthYear = DateTime.Now.ToString("MM/yyyy");
            var result = _cardValidationService.ValidateIssueDate(currentMonthYear);

            Assert.IsTrue(result);
        }

        [AllureName("Validate Issue Date - Invalid Issue Date - Returns False")]
        [TestCase("")]
        [TestCase("/")]
        [TestCase("05.2030")]
        [TestCase("05-2030")]
        [TestCase("1/2030")]
        [TestCase("06/1")]
        [TestCase("-/-")]
        [TestCase("13/2030")]
        [TestCase("06/20255123142312343212234234234234234")]
        [TestCase("061231245131242131231231212412312/2025")]
        [TestCase("02/2000")]
        [TestCase("00/2030")]
        [TestCase("06/999")]
        [TestCase("12/2024")]
        public void ValidateIssueDate_InvalidIssueDate_ReturnsFalse(string issueDate)
        {
            var result = _cardValidationService.ValidateIssueDate(issueDate);
            Assert.IsFalse(result);
        }

        [AllureName("Validate Cvc - Valid Cvc - Returns True")]
        [TestCase("123")]
        [TestCase("456")]
        [TestCase("789")]
        [TestCase("1234")]
        [TestCase("5678")]
        [TestCase("0999")]
        public void ValidateCvc_ValidCvc_ReturnsTrue(string cvc)
        {
            var result = _cardValidationService.ValidateCvc(cvc);
            Assert.IsTrue(result);
        }

        [AllureName("Validate Cvc - Invalid Cvc - Returns False")]
        [TestCase("")]
        [TestCase("!@#$%^&*()_+")]
        [TestCase("12")]
        [TestCase("12345")]
        [TestCase("abcd")]
        public void ValidateCvc_InvalidCvc_ReturnsFalse(string cvc)
        {
            var result = _cardValidationService.ValidateCvc(cvc);
            Assert.IsFalse(result);
        }

        [AllureName("Validate Card Number - Valid Card Number - Returns True")]
        [TestCase("4111111111111111")]  // Visa card example
        [TestCase("5105105105105100")]  // MasterCard card example
        [TestCase("340000000000009")]   // American Express card example
        public void ValidateNumber_ValidCardNumber_ReturnsTrue(string cardNumber)
        {
            var result = _cardValidationService.ValidateNumber(cardNumber);
            Assert.IsTrue(result);
        }

        [AllureName("Validate Card Number - Invalid Card Number - Returns False")]
        [TestCase("1234567890123456")] // Invalid card number
        [TestCase("1111111111111111")] // Invalid card number
        [TestCase("0000000000000000")] // Invalid card number
        public void ValidateNumber_InvalidCardNumber_ReturnsFalse(string cardNumber)
        {
            var result = _cardValidationService.ValidateNumber(cardNumber);
            Assert.IsFalse(result);
        }

        [AllureName("Get Payment System Type - Valid Card Number - Returns Correct Payment System")]
        [Test]
        public void GetPaymentSystemType_ValidCardNumber_ReturnsCorrectPaymentSystem()
        {
            var visaCard = "4111111111111111";          // Visa card example
            var masterCard = "5105105105105100";        // MasterCard card example
            var amexCard = "340000000000009";           // American Express card example
            var invalidCardNumber = "123123123123123123"; // Invalid card provider

            var visaResult = _cardValidationService.GetPaymentSystemType(visaCard);
            var masterCardResult = _cardValidationService.GetPaymentSystemType(masterCard);
            var amexResult = _cardValidationService.GetPaymentSystemType(amexCard);

            Assert.That(visaResult, Is.EqualTo(PaymentSystemType.Visa));
            Assert.That(masterCardResult, Is.EqualTo(PaymentSystemType.MasterCard));
            Assert.That(amexResult, Is.EqualTo(PaymentSystemType.AmericanExpress));
            Assert.Throws<NotImplementedException>(() => _cardValidationService.GetPaymentSystemType(invalidCardNumber));
        }
    }
}