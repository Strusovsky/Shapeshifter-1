﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autofac;
using Shapeshifter.UserInterface.WindowsDesktop.Factories.Interfaces;
using NSubstitute;
using Shapeshifter.Core.Data.Interfaces;
using Shapeshifter.UserInterface.WindowsDesktop.Controls.Clipboard.Factories.Interfaces;
using Shapeshifter.UserInterface.WindowsDesktop.Controls.Clipboard.Interfaces;
using System;
using Shapeshifter.Core.Data;
using Shapeshifter.UserInterface.WindowsDesktop.Services.Api;

namespace Shapeshifter.Tests.Factories
{
    [TestClass]
    public class FileClipboardDataControlFactoryTest : TestBase
    {
        [TestMethod]
        public void CanBuildDataReturnsTrueForSingleFileFormats()
        {
            var container = CreateContainer();

            var factory = container.Resolve<IFileClipboardDataControlFactory>();
            Assert.IsTrue(factory.CanBuildData(ClipboardApi.CF_HDROP));
        }

        [TestMethod]
        public void CanBuildDataReturnsFalseForNonFileFormats()
        {
            var container = CreateContainer();

            var factory = container.Resolve<IFileClipboardDataControlFactory>();
            Assert.IsFalse(factory.CanBuildData(uint.MaxValue));
        }

        [TestMethod]
        public void BuildsFileControlForSingleFileData()
        {
            var fakeSingleFileData = Substitute.For<IClipboardFileData>();
            var fakeControl = Substitute.For<IClipboardFileDataControl>();

            var container = CreateContainer(c =>
            {
                c.RegisterFake<IClipboardControlFactory<IClipboardFileData, IClipboardFileDataControl>>()
                .CreateControl(fakeSingleFileData)
                .Returns(fakeControl);
            });

            var factory = container.Resolve<IFileClipboardDataControlFactory>();
            var control = factory.BuildControl(fakeSingleFileData);

            Assert.AreSame(fakeControl, control);
        }

        [TestMethod]
        public void BuildsFileControlForMultipleFileData()
        {
            var fakeSingleFileData = Substitute.For<IClipboardFileCollectionData>();
            var fakeControl = Substitute.For<IClipboardFileCollectionDataControl>();

            var container = CreateContainer(c =>
            {
                c.RegisterFake<IClipboardControlFactory<IClipboardFileCollectionData, IClipboardFileCollectionDataControl>>()
                .CreateControl(fakeSingleFileData)
                .Returns(fakeControl);
            });

            var factory = container.Resolve<IFileClipboardDataControlFactory>();
            var control = factory.BuildControl(fakeSingleFileData);

            Assert.AreSame(fakeControl, control);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildControlForUnknownTypeThrowsException()
        {
            var fakeData = Substitute.For<IClipboardData>();

            var container = CreateContainer();

            var factory = container.Resolve<IFileClipboardDataControlFactory>();
            factory.BuildControl(fakeData);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildDataForIncompatibleFormatThrowsException()
        {
            var container = CreateContainer();

            var factory = container.Resolve<IFileClipboardDataControlFactory>();
            factory.BuildData(uint.MaxValue, new byte[0]);
        }

        [TestMethod]
        public void BuildDataForFileDropReturnsFileCollectionData()
        {
            var container = CreateContainer();

            var factory = container.Resolve<IFileClipboardDataControlFactory>();
            var data = factory.BuildData(ClipboardApi.CF_HDROP, new byte[0]);
            Assert.IsInstanceOfType(data, typeof(ClipboardFileCollectionData));
        }

        [TestMethod]
        public void CanBuildDataReturnsTrueForFileDropFormat()
        {
            var container = CreateContainer();

            var factory = container.Resolve<IFileClipboardDataControlFactory>();
            Assert.IsTrue(factory.CanBuildData(ClipboardApi.CF_HDROP));
        }

        [TestMethod]
        public void CanBuildDataReturnsFalseForInvalidFormat()
        {
            var container = CreateContainer();

            var factory = container.Resolve<IFileClipboardDataControlFactory>();
            Assert.IsFalse(factory.CanBuildData(uint.MaxValue));
        }

        [TestMethod]
        public void CanBuildControlReturnsFalseForNonFileData()
        {
            var container = CreateContainer();

            var factory = container.Resolve<IFileClipboardDataControlFactory>();
            Assert.IsFalse(factory.CanBuildControl(Substitute.For<IClipboardData>()));
        }

        [TestMethod]
        public void CanBuildControlReturnsTrueForFileData()
        {
            var container = CreateContainer();

            var factory = container.Resolve<IFileClipboardDataControlFactory>();
            Assert.IsTrue(factory.CanBuildControl(Substitute.For<IClipboardFileData>()));
        }

        [TestMethod]
        public void CanBuildControlReturnsTrueForFileCollectionData()
        {
            var container = CreateContainer();

            var factory = container.Resolve<IFileClipboardDataControlFactory>();
            Assert.IsTrue(factory.CanBuildControl(Substitute.For<IClipboardFileCollectionData>()));
        }
    }
}