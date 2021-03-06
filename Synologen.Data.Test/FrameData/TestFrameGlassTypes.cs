using System.Linq;
using NUnit.Framework;
using Spinit.Data;
using Spinit.Wpc.Synologen.Core.Domain.Exceptions;
using Spinit.Wpc.Synologen.Core.Domain.Model.FrameOrder;
using Spinit.Wpc.Synologen.Data.Test.FrameData.Factories;
using Spinit.Wpc.Synologen.Integration.Data.Test.FrameData;
using Spinit.Wpc.Synologen.Test.Core;

namespace Spinit.Wpc.Synologen.Data.Test.FrameData
{
	[TestFixture, Category("TestFrameGlassTypes")]
	public class Given_a_frameglasstype : TestBase
	{
		[SetUp]
		public void Context()
		{
			SetupDefaultContext();
		}

		[Test]
		public void Can_get_persisted_frameglasstype()
		{
			//Arrange
			const int expectedNumberOfOrderConnections = 36;

			//Act
			var savedFrameGlassType = SavedFrameGlassTypes.First();
			var persistedFrameGlassType = FrameGlassTypeValidationRepository.Get(savedFrameGlassType.Id);
			
			//Assert
			Expect(persistedFrameGlassType, Is.Not.Null);
			Expect(persistedFrameGlassType.Id, Is.EqualTo(savedFrameGlassType.Id));
			Expect(persistedFrameGlassType.Name, Is.EqualTo(savedFrameGlassType.Name));
			Expect(persistedFrameGlassType.IncludeAdditionParametersInOrder, Is.EqualTo(savedFrameGlassType.IncludeAdditionParametersInOrder));
			Expect(persistedFrameGlassType.IncludeHeightParametersInOrder, Is.EqualTo(savedFrameGlassType.IncludeHeightParametersInOrder));
			Expect(persistedFrameGlassType.NumberOfConnectedOrdersWithThisGlassType, Is.EqualTo(expectedNumberOfOrderConnections));

			Expect(persistedFrameGlassType.Sphere.Increment, Is.EqualTo(savedFrameGlassType.Sphere.Increment));
			Expect(persistedFrameGlassType.Sphere.Max, Is.EqualTo(savedFrameGlassType.Sphere.Max));
			Expect(persistedFrameGlassType.Sphere.Min, Is.EqualTo(savedFrameGlassType.Sphere.Min));

			Expect(persistedFrameGlassType.Cylinder.Increment, Is.EqualTo(savedFrameGlassType.Cylinder.Increment));
			Expect(persistedFrameGlassType.Cylinder.Max, Is.EqualTo(savedFrameGlassType.Cylinder.Max));
			Expect(persistedFrameGlassType.Cylinder.Min, Is.EqualTo(savedFrameGlassType.Cylinder.Min));

		}

		[Test]
		public void Can_edit_persisted_frameglasstype()
		{
			//Arrange
			const int expectedNumberOfOrderConnections = 36;

			//Act
			var editedFrameGlassType = FrameGlassTypeFactory.ScrabmleFrameGlass(SavedFrameGlassTypes.First());
			FrameGlassTypeRepository.Save(editedFrameGlassType);
			var persistedFrameGlassType = FrameGlassTypeValidationRepository.Get(SavedFrameGlassTypes.First().Id);
			
			//Assert
			Expect(persistedFrameGlassType, Is.Not.Null);
			Expect(persistedFrameGlassType.Id, Is.EqualTo(editedFrameGlassType.Id));
			Expect(persistedFrameGlassType.Name, Is.EqualTo(editedFrameGlassType.Name));
			Expect(persistedFrameGlassType.IncludeAdditionParametersInOrder, Is.EqualTo(editedFrameGlassType.IncludeAdditionParametersInOrder));
			Expect(persistedFrameGlassType.IncludeHeightParametersInOrder, Is.EqualTo(editedFrameGlassType.IncludeHeightParametersInOrder));
			Expect(persistedFrameGlassType.NumberOfConnectedOrdersWithThisGlassType, Is.EqualTo(expectedNumberOfOrderConnections));

			Expect(persistedFrameGlassType.Sphere.Increment, Is.EqualTo(editedFrameGlassType.Sphere.Increment));
			Expect(persistedFrameGlassType.Sphere.Max, Is.EqualTo(editedFrameGlassType.Sphere.Max));
			Expect(persistedFrameGlassType.Sphere.Min, Is.EqualTo(editedFrameGlassType.Sphere.Min));

			Expect(persistedFrameGlassType.Cylinder.Increment, Is.EqualTo(editedFrameGlassType.Cylinder.Increment));
			Expect(persistedFrameGlassType.Cylinder.Max, Is.EqualTo(editedFrameGlassType.Cylinder.Max));
			Expect(persistedFrameGlassType.Cylinder.Min, Is.EqualTo(editedFrameGlassType.Cylinder.Min));
		}

		[Test]
		public void Can_delete_persisted_frameglasstype_without_connections()
		{
			//Arrange
			var frameGlassType = FrameGlassTypeFactory.GetGlassType(SavedFrameSuppliers.First());
			FrameGlassTypeRepository.Save(frameGlassType);

			//Act
			FrameGlassTypeRepository.Delete(frameGlassType);
			var persistedFrameGlassType = FrameGlassTypeValidationRepository.Get(frameGlassType.Id);
			
			//Assert
			Expect(persistedFrameGlassType, Is.Null);

		}

		[Test]
		public void Cannot_delete_persisted_frameglasstype_with_connections()
		{
			//Arrange

			//Act
			
			//Assert
			Expect(() => FrameGlassTypeValidationRepository.Delete(SavedFrameGlassTypes.First()), Throws.InstanceOf<SynologenDeleteItemHasConnectionsException>());
		}
	}

	[TestFixture, Category("TestFrameGlassTypes")]
	public class Given_multiple_frameglassTypes : TestBase
	{
		[SetUp]
		public void Context()
		{
			SetupDefaultContext();
		}

		[Test]
		public void Can_get_frameglasstypes_by_PageOfFrameGlassTypesMatchingCriteria_paged()
		{
			//Arrange
			const int expectedNumberOfItemsMatchingCriteria = 3;
			var criteria = new PagedSortedCriteria(typeof(FrameGlassType))
			{
				OrderBy = null,
				Page = 1,
				PageSize = expectedNumberOfItemsMatchingCriteria,
				SortAscending = true
			};

			//Act
			var itemsMatchingCriteria = FrameGlassTypeValidationRepository.FindBy(criteria);
			
			//Assert
			Expect(itemsMatchingCriteria.Count(), Is.EqualTo(expectedNumberOfItemsMatchingCriteria));
		}

		[Test]
		public void Can_get_frameglasstypes_by_PageOfFrameGlassTypesMatchingCriteria_sorted_by_id()
		{
			//Arrange
			var criteria = new PagedSortedCriteria(typeof(FrameGlassType))
			{
				OrderBy = "Id",
				Page = 1,
				PageSize = 100,
				SortAscending = true
			};

			//Act
			var itemsMatchingCriteria = FrameGlassTypeValidationRepository.FindBy(criteria);
			
			//Assert
			itemsMatchingCriteria.ShouldBeOrderedAscendingBy(x => x.Id);
		}

		[Test]
		public void Can_get_brands_by_PageOfFrameGlassTypesMatchingCriteria_sorted_by_name()
		{
			//Arrange
			var criteria = new PagedSortedCriteria(typeof(FrameGlassType))
			{
				OrderBy = "Name",
				Page = 1,
				PageSize = 100,
				SortAscending = true
			};

			//Act
			var itemsMatchingCriteria = FrameGlassTypeValidationRepository.FindBy(criteria);
			
			//Assert
			itemsMatchingCriteria.ShouldBeOrderedAscendingBy(x => x.Name);
		}

		[Test]
		public void Can_get_brands_by_PageOfFrameGlassTypesMatchingCriteria_sorted_descending()
		{
			//Arrange
			var criteria = new PagedSortedCriteria(typeof(FrameGlassType))
			{
				OrderBy = "Id",
				Page = 1,
				PageSize = 100,
				SortAscending = false
			};

			//Act
			var itemsMatchingCriteria = FrameGlassTypeValidationRepository.FindBy(criteria);
			
			//Assert
			itemsMatchingCriteria.ShouldBeOrderedDescendingBy(x => x.Id);
		}
	}
}