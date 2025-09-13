using CineTrackFE.AppServises;
using CineTrackFE.Models;
using CineTrackFE.Tests.Helpers.Common;
using FluentAssertions;

namespace CineTrackFE.Tests.Unit.AppServices
{
    public class ModelMappingServiceTests
    {

        // CLONE FILM //
        [Fact]
        public void CloneFilm__ShouldThrowArgumentNullException_WhenInputIsNull()
        {
            // Arrange
            Film? source = null;

            // Act & Assert
            FluentActions
                .Invoking(() => ModelMappingService.CloneFilm(source!))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void CloneFilm__ShouldReturnNewFilmObject_WithSameProperties()
        {
            // Arrange
            var genres = Fakers.Genre.Generate(3);
            var film = Fakers.Film.RuleFor(fm => fm.Genres, f => genres).Generate();

            // Act
            var result = ModelMappingService.CloneFilm(film);

            // Assert
            result.Should().NotBeNull();
            result.Should().NotBeSameAs(film);
            result.Should().BeEquivalentTo(film);
        }

        [Fact]
        public void CloneGenre__ShouldThrowArgumentNullException_WhenInputIsNull()
        {
            // Arrange
            Genre? source = null;

            // Act & Assert
            FluentActions
                .Invoking(() => ModelMappingService.CloneGenre(source!))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void CloneGenre__ShouldReturnNewGenreObject_WithSameProperties()
        {
            // Arrange
            var genre = Fakers.Genre.Generate();

            // Act
            var result = ModelMappingService.CloneGenre(genre);

            // Assert
            result.Should().NotBeNull();
            result.Should().NotBeSameAs(genre);
            result.Should().BeEquivalentTo(genre);
        }

        [Fact]
        public void CloneUser__ShouldThrowArgumentNullException_WhenInputIsNull()
        {
            // Arrange
            User? source = null;

            // Act & Assert
            FluentActions
                .Invoking(() => ModelMappingService.CloneUser(source!))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void CloneUser__ShouldReturnNewUserObject_WithSameProperties()
        {
            // Arrange
            var user = Fakers.User.Generate();

            // Act
            var result = ModelMappingService.CloneUser(user);

            // Assert
            result.Should().NotBeNull();
            result.Should().NotBeSameAs(user);
            result.Should().BeEquivalentTo(user);
        }
    }
}
