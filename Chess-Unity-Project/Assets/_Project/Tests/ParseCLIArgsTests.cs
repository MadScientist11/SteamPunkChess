using System.Collections.Generic;
using FluentAssertions;
using UnityEngine;
using NSubstitute;
using NUnit.Framework;

namespace SteampunkChess.Tests
{
    public class ParseCLIArgsTests
    {
        [Test]
        public void WhenGameArgsAreEmpty_AndAddSkipUserValidationArg_ThenGameArgsShouldContainSkipUserValidationArg()
        {
            //Arrange
            IEnumerable<string> gameArgs;

            //Act
            gameArgs = GameCommandLineArgs.ParseCustomArguments(new string[]
                {"--SkipUserValidation"});

            //Assert
            gameArgs.Should().Contain(GameConstants.GameCLIArgs.SkipUserValidation);
        }
    }
}