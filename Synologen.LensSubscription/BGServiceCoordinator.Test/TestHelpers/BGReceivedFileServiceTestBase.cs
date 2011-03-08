﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FakeItEasy;
using Spinit.Wpc.Synologen.Core.Domain.Model.Autogiro.CommonTypes;
using Spinit.Wpc.Synologen.Core.Domain.Services;
using Synologen.Test.Core;
using Synologen.LensSubscription.BGServiceCoordinator.App.Services;

namespace Synologen.LensSubscription.BGService.Test.TestHelpers
{
    public abstract class BGReceivedFileServiceTestBase : BehaviorTestBase<BGReceivedFileReaderService>
    {
        protected IFileIOService FileIOService;
        protected IBGServiceCoordinatorSettingsService BgServiceCoordinatorSettingsService;
        protected IFileSplitter FileSplitter;

        protected BGReceivedFileServiceTestBase()
        {
            FileIOService = A.Fake<IFileIOService>();
            BgServiceCoordinatorSettingsService = A.Fake<IBGServiceCoordinatorSettingsService>();
            FileSplitter = A.Fake<IFileSplitter>();
        }

        protected override BGReceivedFileReaderService GetTestModel()
        {
            return new BGReceivedFileReaderService(FileIOService, BgServiceCoordinatorSettingsService, BGFtpServiceType.Autogiro, FileSplitter);
        }

    }
}
