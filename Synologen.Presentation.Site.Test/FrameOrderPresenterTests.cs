﻿using System;
using System.Linq;
using NUnit.Framework;
using Spinit.Wpc.Synologen.Core.Domain.Model.FrameOrder;
using Spinit.Wpc.Synologen.Core.Domain.Persistence;
using Spinit.Wpc.Synologen.Core.Domain.Services;
using Spinit.Wpc.Synologen.Presentation.Site.Logic.EventArguments;
using Spinit.Wpc.Synologen.Presentation.Site.Logic.Presenters;
using Spinit.Wpc.Synologen.Presentation.Site.Logic.Views;
using Spinit.Wpc.Synologen.Presentation.Site.Models;

namespace Spinit.Wpc.Synologen.Presentation.Site.Test
{
	[TestFixture]
	public class Given_a_FrameOrderPresenter : AssertionHelper
	{
		private FrameOrderPresenter presenter;
		private IFrameOrderView<FrameOrderModel> view;
		private IFrameRepository repository;
		private IFrameGlassTypeRepository frameGlassTypeRepository;
		private IFrameOrderSettingsService frameOrderSettingsService;

		[SetUp]
		public void Context()
		{
			repository = Factories.RepositoryFactory.GetFrameRepository();
			frameGlassTypeRepository = Factories.RepositoryFactory.GetFrameGlassRepository();
			frameOrderSettingsService = Factories.ServiceFactory.GetFrameOrderSettingsService();
			view = Factories.ViewsFactory.GetFrameOrderView();
			presenter = new FrameOrderPresenter(view, repository, frameGlassTypeRepository, frameOrderSettingsService);
		}

		[Test]
		public void When_View_Is_Loaded_Model_Has_Expected_Values()
		{
			//Arrange
			const int expectedNumberOfFrames = 11;
			const int expectedNumberOfPDs = 1;
			const int expectedNumberOfSpheres = 50;
			const int expectedNumberOfCylinders = 10;
			const int expectedNumberOfGlassTypes = 11;
			const int expectedNumberOfAdditions = 1;
			const int expectedNumberOfHeights = 1;

			//Act
			presenter.View_Load(null, new EventArgs());

			//Assert
			Expect(view.Model.SelectedFrameId, Is.EqualTo(0));
			Expect(view.Model.SelectedGlassTypeId, Is.EqualTo(0));

			Expect(view.Model.PupillaryDistance.Selection.Left, Is.EqualTo(int.MinValue));
			Expect(view.Model.PupillaryDistance.Selection.Right, Is.EqualTo(int.MinValue));

			Expect(view.Model.Sphere.Selection.Left, Is.EqualTo(int.MinValue));
			Expect(view.Model.Sphere.Selection.Right, Is.EqualTo(int.MinValue));

			Expect(view.Model.Cylinder.Selection.Left, Is.EqualTo(int.MinValue));
			Expect(view.Model.Cylinder.Selection.Right, Is.EqualTo(int.MinValue));


			Expect(view.Model.AxisSelection.Left, Is.EqualTo(0));
			Expect(view.Model.AxisSelection.Right, Is.EqualTo(0));

			Expect(view.Model.FramesList.Count(), Is.EqualTo(expectedNumberOfFrames));
			Expect(view.Model.FramesList.ToList()[0].Id, Is.EqualTo(0));
			Expect(view.Model.FramesList.ToList()[0].Name, Is.EqualTo("-- Välj båge --"));
			Expect(view.Model.FrameRequiredErrorMessage, Is.EqualTo("Båge saknas"));

			Expect(view.Model.GlassTypesList.Count(), Is.EqualTo(expectedNumberOfGlassTypes));
			Expect(view.Model.GlassTypesList.ToList()[0].Id, Is.EqualTo(0));
			Expect(view.Model.GlassTypesList.ToList()[0].Name, Is.EqualTo("-- Välj glastyp --"));
			Expect(view.Model.GlassTypeRequiredErrorMessage, Is.EqualTo("Glastyp saknas"));

			Expect(view.Model.PupillaryDistance.List.Count(), Is.EqualTo(expectedNumberOfPDs));
			Expect(view.Model.PupillaryDistance.List.ToList()[0].Value, Is.EqualTo(int.MinValue));
			Expect(view.Model.PupillaryDistance.List.ToList()[0].Name, Is.EqualTo("-- Välj PD --"));
			Expect(view.Model.PupillaryDistanceRequiredErrorMessage, Is.EqualTo("PD saknas"));

			Expect(view.Model.Sphere.List.Count(), Is.EqualTo(expectedNumberOfSpheres));
			Expect(view.Model.Sphere.List.ToList()[0].Value, Is.EqualTo(int.MinValue));
			Expect(view.Model.Sphere.List.ToList()[0].Name, Is.EqualTo("-- Välj Sfär --"));
			Expect(view.Model.SphereRequiredErrorMessage, Is.EqualTo("Sfär saknas"));

			Expect(view.Model.Cylinder.List.Count(), Is.EqualTo(expectedNumberOfCylinders));
			Expect(view.Model.Cylinder.List.ToList()[0].Value, Is.EqualTo(int.MinValue));
			Expect(view.Model.Cylinder.List.ToList()[0].Name, Is.EqualTo("-- Välj Cylinder --"));
			Expect(view.Model.CylinderRequiredErrorMessage, Is.EqualTo("Cylinder saknas"));

			Expect(view.Model.Addition.List.Count(), Is.EqualTo(expectedNumberOfAdditions));
			Expect(view.Model.Addition.List.ToList()[0].Value, Is.EqualTo(int.MinValue));
			Expect(view.Model.Addition.List.ToList()[0].Name, Is.EqualTo("-- Välj Addition --"));
			Expect(view.Model.AdditionRequiredErrorMessage, Is.EqualTo("Addition saknas"));

			Expect(view.Model.Height.List.Count(), Is.EqualTo(expectedNumberOfHeights));
			Expect(view.Model.Height.List.ToList()[0].Value, Is.EqualTo(int.MinValue));
			Expect(view.Model.Height.List.ToList()[0].Name, Is.EqualTo("-- Välj Höjd --"));
			Expect(view.Model.HeightRequiredMessage, Is.EqualTo("Höjd saknas"));

			Expect(view.Model.AxisRangeMessage, Is.EqualTo("Axel saknas"));
			Expect(view.Model.AxisRequiredMessage, Is.EqualTo("Axel måste ligga i intervallet 0-180"));

			Expect(view.Model.NotSelectedIntervalValue, Is.EqualTo(int.MinValue));
			Expect(view.Model.HeightParametersEnabled, Is.False);
			Expect(view.Model.AdditionParametersEnabled, Is.False);
		}

		[Test]
		public void When_Model_Is_Bound_Selected_Model_Has_Expected_Values()
		{
			//Arrange
			var frameSelectedEventArgs = new FrameFormEventArgs
			{
				SelectedFrameId = 5, 
				SelectedGlassTypeId = 8,  // Returns a glasstype with both addition and height
				SelectedPupillaryDistance = new EyeParameter{Left = 22, Right = 33},
				SelectedSphere = new EyeParameter{Left = -5, Right = 2.25M},
                SelectedCylinder = new EyeParameter{ Left = 0.25M, Right = 1.75M}, 
                SelectedAxis = new EyeParameter{ Left = 20, Right = 179},
				SelectedAddition = new EyeParameter{Left = 1.25M, Right = 2.75M},
				SelectedHeight = new EyeParameter{Left = 19, Right = 27},
			};
			const int expectedNumberOfFramesInList = 11;
			const int expectedNumberOfGlassTypesInList = 11;
			const int expectedNumberOfPDsInList = 22;
			const int expectedNumberOfSpheresInList = 50;
			const int expectedNumberOfCylindersInList = 10;
			const int expectedNumberOfAdditionsInList = 10;
			const int expectedNumberOfHeightsInList = 12;

			//Act
			presenter.View_Load(null, new EventArgs());
			presenter.View_BindModel(null, frameSelectedEventArgs);

			//Assert
			Expect(view.Model.PupillaryDistance.List.Count(), Is.EqualTo(expectedNumberOfPDsInList));
			Expect(view.Model.Sphere.List.Count(), Is.EqualTo(expectedNumberOfSpheresInList));
			Expect(view.Model.Cylinder.List.Count(), Is.EqualTo(expectedNumberOfCylindersInList));
			Expect(view.Model.Addition.List.Count(), Is.EqualTo(expectedNumberOfAdditionsInList));
			Expect(view.Model.Height.List.Count(), Is.EqualTo(expectedNumberOfHeightsInList));
			Expect(view.Model.GlassTypesList.Count(), Is.EqualTo(expectedNumberOfGlassTypesInList));
			Expect(view.Model.FramesList.Count(), Is.EqualTo(expectedNumberOfFramesInList));

			Expect(view.Model.SelectedFrameId, Is.EqualTo(frameSelectedEventArgs.SelectedFrameId));
			Expect(view.Model.SelectedGlassTypeId, Is.EqualTo(frameSelectedEventArgs.SelectedGlassTypeId));
			Expect(view.Model.PupillaryDistance.Selection.Left, Is.EqualTo(frameSelectedEventArgs.SelectedPupillaryDistance.Left));
			Expect(view.Model.PupillaryDistance.Selection.Right, Is.EqualTo(frameSelectedEventArgs.SelectedPupillaryDistance.Right));
			Expect(view.Model.Sphere.Selection.Left, Is.EqualTo(frameSelectedEventArgs.SelectedSphere.Left));
			Expect(view.Model.Sphere.Selection.Right, Is.EqualTo(frameSelectedEventArgs.SelectedSphere.Right));
			Expect(view.Model.Cylinder.Selection.Left, Is.EqualTo(frameSelectedEventArgs.SelectedCylinder.Left));
			Expect(view.Model.Cylinder.Selection.Right, Is.EqualTo(frameSelectedEventArgs.SelectedCylinder.Right));
			Expect(view.Model.AxisSelection.Left, Is.EqualTo(frameSelectedEventArgs.SelectedAxis.Left));
			Expect(view.Model.AxisSelection.Right, Is.EqualTo(frameSelectedEventArgs.SelectedAxis.Right));
			Expect(view.Model.Addition.Selection.Left, Is.EqualTo(frameSelectedEventArgs.SelectedAddition.Left));
			Expect(view.Model.Addition.Selection.Right, Is.EqualTo(frameSelectedEventArgs.SelectedAddition.Right));
			Expect(view.Model.Height.Selection.Left, Is.EqualTo(frameSelectedEventArgs.SelectedHeight.Left));
			Expect(view.Model.Height.Selection.Right, Is.EqualTo(frameSelectedEventArgs.SelectedHeight.Right));
			Expect(view.Model.HeightParametersEnabled, Is.True);
			Expect(view.Model.AdditionParametersEnabled, Is.True);
		}

		[Test]
		public void When_Model_Is_Bound_Without_Addition_Or_Height_Selected_Model_Has_Expected_Values()
		{
			//Arrange
			var frameSelectedEventArgs = new FrameFormEventArgs
			{
				SelectedFrameId = 5, 
				SelectedGlassTypeId = 1, // Returns a glasstype without addition or height
				SelectedPupillaryDistance = new EyeParameter{Left = 22, Right = 33},
				SelectedSphere = new EyeParameter{Left = -5, Right = 2.25M},
                SelectedCylinder = new EyeParameter{ Left = 0.25M, Right = 1.75M}, 
                SelectedAxis = new EyeParameter{ Left = 20, Right = 179},
				SelectedAddition = new EyeParameter{Left = 1.25M, Right = 2.75M},
				SelectedHeight = new EyeParameter{Left = 19, Right = 27},
			};
			const int expectedNumberOfAdditionsInList = 1;
			const int expectedNumberOfHeightsInList = 1;

			//Act
			presenter.View_Load(null, new EventArgs());
			presenter.View_BindModel(null, frameSelectedEventArgs);

			//Assert

			Expect(view.Model.Addition.List.Count(), Is.EqualTo(expectedNumberOfAdditionsInList));
			Expect(view.Model.Height.List.Count(), Is.EqualTo(expectedNumberOfHeightsInList));

			Expect(view.Model.Height.Selection.Left, Is.EqualTo(int.MinValue));
			Expect(view.Model.Height.Selection.Right, Is.EqualTo(int.MinValue));
			Expect(view.Model.HeightParametersEnabled, Is.False);
			Expect(view.Model.AdditionParametersEnabled, Is.False);
		}

		[Test]
		public void When_Model_Is_Bound_Model_With_Invalid_Parameters_Get_Default_Values()
		{
			//Arrange
			var frameSelectedEventArgs = new FrameFormEventArgs
			{
				SelectedFrameId = 1, 
				SelectedGlassTypeId = 8,
				SelectedPupillaryDistance = new EyeParameter{Left = 200, Right = -20},
				SelectedSphere = new EyeParameter{Left =10, Right = 42},
				SelectedCylinder = new EyeParameter{Left = -0.25M, Right = 3},
				SelectedAxis = new EyeParameter{ Left = -1, Right = 181},
				SelectedAddition = new EyeParameter{Left = 0.75M, Right = 3.75M},
				SelectedHeight = new EyeParameter{Left = 17, Right = 33},
			};

			//Act
			presenter.View_Load(null, new EventArgs());
			presenter.View_BindModel(null, frameSelectedEventArgs);

			//Assert
			Expect(view.Model.SelectedFrameId, Is.EqualTo(1));
			Expect(view.Model.SelectedGlassTypeId, Is.EqualTo(8));
			Expect(view.Model.PupillaryDistance.Selection.Left, Is.EqualTo(int.MinValue));
			Expect(view.Model.PupillaryDistance.Selection.Right, Is.EqualTo(int.MinValue));
			Expect(view.Model.Sphere.Selection.Left, Is.EqualTo(int.MinValue));
			Expect(view.Model.Sphere.Selection.Right, Is.EqualTo(int.MinValue));
			Expect(view.Model.Cylinder.Selection.Left, Is.EqualTo(int.MinValue));
			Expect(view.Model.Cylinder.Selection.Right, Is.EqualTo(int.MinValue));
			Expect(view.Model.AxisSelection.Left, Is.EqualTo(frameSelectedEventArgs.SelectedAxis.Left));
			Expect(view.Model.AxisSelection.Right, Is.EqualTo(frameSelectedEventArgs.SelectedAxis.Right));
			Expect(view.Model.Addition.Selection.Left, Is.EqualTo(int.MinValue));
			Expect(view.Model.Addition.Selection.Right, Is.EqualTo(int.MinValue));
			Expect(view.Model.Height.Selection.Left, Is.EqualTo(int.MinValue));
			Expect(view.Model.Height.Selection.Right, Is.EqualTo(int.MinValue));
		}
	}
}