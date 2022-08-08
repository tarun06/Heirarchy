using Moq;
using Newtonsoft.Json;
using Prism.Regions;
using PrismApp.Modules.Users.Models;
using PrismApp.Modules.Users.ViewModels;
using PrismApp.Services.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace PrismApp.Modules.Users.Tests
{
    public class UserListViewModelTest
    {
        public const string url = "https://sanas-test-api.azurewebsites.net/";
        public List<SuperviserInfo> LoadJson()
        {
            using (StreamReader r = new StreamReader(Environment.CurrentDirectory + "/Data/Users.json"))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<List<SuperviserInfo>>(json);
            }
        }

        public IEnumerable<SuperviserInfo> _superviserInfo;
        public UserListViewModelTest()
        {
            _superviserInfo = LoadJson();
        }

        [Fact]
        public void ThrwArgumentNullExceptionWhenUserServiceIsNull()
        {
            var region = new Mock<IRegionManager>() { DefaultValue = DefaultValue.Mock };
            region.SetupAllProperties(); 
            
            // Asserts
            Assert.Throws<ArgumentNullException>(() =>
            {
                var vm = new UsersListViewModel(region.Object, null!, null!);
                return vm;
            });
        }

        [Fact]
        public async void LoadCommandtest()
        {
            var region = new Mock<IRegionManager>() { DefaultValue = DefaultValue.Mock};
            region.SetupAllProperties();

            var appConfig = new Mock<IAppConfiguration>() { DefaultValue = DefaultValue.Mock };
            appConfig.SetupAllProperties();
            appConfig.Setup(x => x.ServiceUrl).Returns(new Uri(url));

            var userService = new Mock<IUserService<SuperviserInfo>>() { DefaultValue = DefaultValue.Mock };
            region.SetupAllProperties();
            userService.Setup(x => x.GetUsers(It.IsAny<Uri>())).ReturnsAsync(_superviserInfo);

            var vm = new UsersListViewModel(region.Object, userService.Object, appConfig.Object);
            vm.LoadCommand.Execute();

            await Task.Delay(1000);

            Assert.NotEmpty(vm.Items);
            Assert.Equal(2, vm.Items.Count);
            Assert.Equal("MoisesE@univ.edu", vm.Items[0].Email);
            Assert.Equal(11, vm.Items[0].Members!.Count);
            Assert.Equal("HeadK@univ.edu", vm.Items[1].Email);
            Assert.Equal(6, vm.Items[1].Members!.Count);
        }
    }
}